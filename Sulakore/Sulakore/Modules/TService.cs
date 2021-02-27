using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Threading.Tasks;

using Sulakore.Habbo;
using Sulakore.Habbo.Packages;
using Sulakore.Network;
using Sulakore.Network.Protocol;

namespace Sulakore.Modules
{
    public class TService
    {
        private readonly IModule _container;

        private readonly IDictionary<int, HEntity> _entities;

        private readonly IDictionary<int, HFloorObject>                 _floorObjects;
        private readonly Dictionary<ushort, List<DataCaptureAttribute>> _outDataAttributes, _inDataAttributes;

        private readonly TService                   _parent;
        private readonly List<DataCaptureAttribute> _unknownDataAttributes;

        private readonly IDictionary<int, HWallItem> _wallItems;

        private IInstaller _installer;

        public TService(IModule container) : this(container, null, null) { }

        public TService(IModule container, IPEndPoint moduleServer) : this(container, null, moduleServer) { }

        protected TService() : this(null, null, null) { }

        protected TService(IPEndPoint moduleServer) : this(null, null, moduleServer) { }

        protected TService(TService parent) : this(null, parent, null) { }

        protected TService(TService parent, IPEndPoint moduleServer) : this(null, parent, moduleServer) { }

        private TService(IModule container, TService parent, IPEndPoint moduleServer)
        {
            _parent                = parent;
            _container             = container;
            _unknownDataAttributes = parent?._unknownDataAttributes ?? new List<DataCaptureAttribute>();
            _inDataAttributes      = parent?._inDataAttributes ?? new Dictionary<ushort, List<DataCaptureAttribute>>();
            _outDataAttributes     = parent?._outDataAttributes ?? new Dictionary<ushort, List<DataCaptureAttribute>>();

            _entities = new ConcurrentDictionary<int, HEntity>();
            Entities  = new ReadOnlyDictionary<int, HEntity>(_entities);

            _wallItems = new ConcurrentDictionary<int, HWallItem>();
            WallItems  = new ReadOnlyDictionary<int, HWallItem>(_wallItems);

            _floorObjects = new ConcurrentDictionary<int, HFloorObject>();
            FloorObjects  = new ReadOnlyDictionary<int, HFloorObject>(_floorObjects);

            Installer    = _container.Installer;
            IsStandalone = parent == null && _container.IsStandalone;

            if (LicenseManager.UsageMode != LicenseUsageMode.Runtime) return;

            foreach (var method in _container.GetType().GetAllMethods())
            foreach (var dataCaptureAtt in method.GetCustomAttributes<DataCaptureAttribute>())
            {
                dataCaptureAtt.Method = method;

                if (_unknownDataAttributes.Any(dca => dca.Equals(dataCaptureAtt))) continue;

                dataCaptureAtt.Target = _container;

                if (dataCaptureAtt.Id != null)
                    AddCallback(dataCaptureAtt, (ushort) dataCaptureAtt.Id);
                else _unknownDataAttributes.Add(dataCaptureAtt);
            }

            if (!IsStandalone || Assembly.GetAssembly(_container.GetType()) != Assembly.GetEntryAssembly()) return;

            while (true)
            {
                var installerNode = HNode.ConnectAsync(moduleServer ?? DefaultModuleServer).Result;

                if (installerNode != null)
                {
                    var infoPacketOut = new EvaWirePacket(0);
                    WriteModuleInfo(infoPacketOut);

                    installerNode.SendAsync(infoPacketOut).GetAwaiter().GetResult();
                    Installer = _container.Installer = new DummyInstaller(_container, installerNode);

                    break;
                }

                throw new Exception($"Failed to establish connection with the module server: {moduleServer}");
            }
        }

        public static IPEndPoint DefaultModuleServer { get; } = new(IPAddress.Parse("127.0.0.1"), 8055);

        public virtual bool IsStandalone { get; }

        public virtual IInstaller Installer
        {
            get => _parent?.Installer ?? _installer;
            set => _installer = value;
        }

        public IGame                                 Game         => Installer.Game;
        public IHConnection                          Connection   => Installer.Connection;
        public ReadOnlyDictionary<int, HEntity>      Entities     { get; }
        public ReadOnlyDictionary<int, HWallItem>    WallItems    { get; }
        public ReadOnlyDictionary<int, HFloorObject> FloorObjects { get; }

        public virtual void OnConnected()
        {
            ResolveMessageCallbacks();
        }

        private void ResolveMessageCallbacks()
        {
            // var unresolved = new Dictionary<string, IList<string>>();
            // foreach (PropertyInfo property in _container.GetType().GetAllProperties())
            // {
            //     var messageAtt = property.GetCustomAttribute<MessageAttribute>();
            //     if (string.IsNullOrWhiteSpace(messageAtt?.Identifier)) continue;
            //
            //     HMessage message = GetMessage(messageAtt.Identifier, messageAtt.IsOutgoing);
            //     if (message == null)
            //     {
            //         if (!unresolved.TryGetValue(messageAtt.Identifier, out IList<string> users))
            //         {
            //             users = new List<string>();
            //             unresolved.Add(messageAtt.Identifier, users);
            //         }
            //         users.Add($"Property({property.Name})");
            //     }
            //     else property.SetValue(_container, message);
            // }
            // foreach (DataCaptureAttribute dataCaptureAtt in _unknownDataAttributes)
            // {
            //     if (string.IsNullOrWhiteSpace(dataCaptureAtt.Identifier)) continue;
            //     HMessage message = GetMessage(dataCaptureAtt.Identifier, dataCaptureAtt.IsOutgoing);
            //     if (message == null)
            //     {
            //         if (!unresolved.TryGetValue(dataCaptureAtt.Identifier, out IList<string> users))
            //         {
            //             users = new List<string>();
            //             unresolved.Add(dataCaptureAtt.Identifier, users);
            //         }
            //         users.Add($"Method({dataCaptureAtt.Method})");
            //     }
            //     else AddCallback(dataCaptureAtt, message.Id);
            // }
            // if (unresolved.Count > 0)
            // {
            //     throw new MessageResolvingException(Game.Revision, unresolved);
            // }
        }

        private void WriteModuleInfo(HPacket packet)
        {
            var moduleAssembly = Assembly.GetAssembly(_container.GetType());

            var description = string.Empty;
            var name        = moduleAssembly.GetName().Name;
            var moduleAtt   = _container.GetType().GetCustomAttribute<ModuleAttribute>();

            if (moduleAtt != null)
            {
                name        = moduleAtt.Name;
                description = moduleAtt.Description;
            }

            packet.Write(moduleAssembly.GetName().Version.ToString());

            packet.Write(name);
            packet.Write(description);

            var authors     = new List<AuthorAttribute>();
            var authorsAtts = _container.GetType().GetCustomAttributes<AuthorAttribute>();
            if (authorsAtts != null) authors.AddRange(authorsAtts);

            packet.Write(authors.Count);
            foreach (var author in authors) packet.Write(author.Name);
        }

        private void AddCallback(DataCaptureAttribute attribute, ushort id)
        {
            var callbacks = attribute.IsOutgoing ? _outDataAttributes : _inDataAttributes;

            if (!callbacks.TryGetValue(id, out var attributes))
            {
                attributes = new List<DataCaptureAttribute>();
                callbacks.Add(id, attributes);
            }

            attributes.Add(attribute);
        }

        public virtual void Dispose()
        {
            _inDataAttributes.Clear();
            _outDataAttributes.Clear();
            _unknownDataAttributes.Clear();
        }

        private class DummyInstaller : IInstaller, IHConnection
        {
            private readonly HNode                               _installerNode;
            private readonly IModule                             _module;
            private readonly Dictionary<ushort, Action<HPacket>> _moduleEvents;

            public DummyInstaller(IModule module, HNode installerNode)
            {
                _module        = module;
                _installerNode = installerNode;
                _moduleEvents  = new Dictionary<ushort, Action<HPacket>> { [1] = HandleData, [2] = HandleOnConnected };
                _              = HandleInstallerDataAsync();
            }

            HNode IHConnection.Local  => throw new NotSupportedException();
            HNode IHConnection.Remote => throw new NotSupportedException();

            public ValueTask<int> SendToClientAsync(byte[] data)
            {
                return _installerNode.SendAsync(2, false, data.Length, data);
            }

            public ValueTask<int> SendToClientAsync(HPacket packet)
            {
                return SendToClientAsync(packet.ToBytes());
            }

            public ValueTask<int> SendToClientAsync(ushort id, params object[] values)
            {
                return SendToClientAsync(EvaWirePacket.Construct(id, values));
            }

            public ValueTask<int> SendToServerAsync(byte[] data)
            {
                return _installerNode.SendAsync(2, true, data.Length, data);
            }

            public ValueTask<int> SendToServerAsync(HPacket packet)
            {
                return SendToServerAsync(packet.ToBytes());
            }

            public ValueTask<int> SendToServerAsync(ushort id, params object[] values)
            {
                return SendToServerAsync(EvaWirePacket.Construct(id, values));
            }

            public IGame        Game       { get; set; }
            public IHConnection Connection => this;

            private void HandleData(HPacket packet)
            {
                var step        = packet.ReadInt32();
                var isOutgoing  = packet.ReadBoolean();
                var format      = HFormat.GetFormat(packet.ReadUTF8());
                var canContinue = packet.ReadBoolean();

                var ogDataLength = packet.ReadInt32();
                var ogData = packet.ReadBytes(ogDataLength);
                var args = new DataInterceptedEventArgs(format.CreatePacket(ogData), step, isOutgoing, ContinueAsync);

                var isOriginal = packet.ReadBoolean();

                if (!isOriginal)
                {
                    var packetLength = packet.ReadInt32();
                    var packetData   = packet.ReadBytes(packetLength);
                    args.Packet = format.CreatePacket(packetData);
                }

                try
                {
                    if (isOutgoing)
                        _module.HandleOutgoing(args);
                    else
                        _module.HandleIncoming(args);
                }
                catch
                {
                    if (args.IsOriginal != isOriginal) // Was this packet modified before throwing an error?
                        args.Restore();
                }

                if (!args.WasRelayed) _installerNode.SendAsync(CreateHandledDataPacket(args, false));
            }

            private void HandleOnConnected(HPacket packet)
            {
                var messagesJsonDataLength = packet.ReadInt32();
                var messagesJsonData       = packet.ReadBytes(messagesJsonDataLength);
                // TODO: Deserialize into the In, and Out properties

                _module.OnConnected();
            }

            private async Task HandleInstallerDataAsync()
            {
                try
                {
                    var packet = await _installerNode.ReceiveAsync().ConfigureAwait(false);
                    if (packet == null) Environment.Exit(0);

                    var handleInstallerDataTask = HandleInstallerDataAsync();
                    if (_moduleEvents.TryGetValue(packet.Id, out var handler)) handler(packet);
                }
                catch
                {
                    Environment.Exit(0);
                }
            }

            private async Task ContinueAsync(DataInterceptedEventArgs args)
            {
                var handledDataPacket = CreateHandledDataPacket(args, true);
                await _installerNode.SendAsync(handledDataPacket).ConfigureAwait(false);
            }

            private HPacket CreateHandledDataPacket(DataInterceptedEventArgs args, bool isContinuing)
            {
                var handledDataPacket = new EvaWirePacket(1);
                handledDataPacket.Write(args.Step + args.IsOutgoing.ToString());

                handledDataPacket.Write(isContinuing);

                if (isContinuing)
                    handledDataPacket.Write(args.WasRelayed);
                else
                {
                    var packetData = args.Packet.ToBytes();
                    handledDataPacket.Write(packetData.Length);
                    handledDataPacket.Write(packetData);
                    handledDataPacket.Write(args.IsBlocked);
                }

                return handledDataPacket;
            }
        }
    }
}