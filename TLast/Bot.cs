using System;
using System.Linq;
using System.Threading.Tasks;

using Sulakore.Cryptography;
using Sulakore.Cryptography.Ciphers;
using Sulakore.Network;
using Sulakore.Network.Protocol;

namespace TLast
{
    public class Bot
    {
        private readonly HKeyExchange _keyExchange;

        private readonly Random _random;
        private readonly string _sso;

        private HNode _clientSocket;

        private string _hexKey;

        public Action<Bot> BotConnected;
        public Action<Bot> BotDisconnected;
        public Action<Bot> BotTryingReconnect;
        public int         CurrentTries = 0;
        public int         MaxTries     = 3;

        public Bot(int id, string sso)
        {
            _sso = sso;
            Id   = id;

            _random = new Random();

            _keyExchange =
                new HKeyExchange(65537,
                                 "BD214E4F036D35B75FEE36000F24EBBEF15D56614756D7AFBD4D186EF5445F758B284647FEB773927418EF70B95387B80B961EA56D8441D410440E3D3295539A3E86E7707609A274C02614CC2C7DF7D7720068F072E098744AFE68485C6297893F3D2BA3D7AAAAF7FA8EBF5D7AF0BA2D42E0D565B89D332DE4CF898D666096CE61698DE0FAB03A8A5E12430CB427C97194CBD221843D162C9F3ACF74DA1D80EBC37FDE442B68A0814DFEA3989FDF8129C120A8418248D7EE85D0B79FA818422E496D6FA7B5BD5DB77E588F8400CDA1A8D82EFED6C86B434BAFA6D07DFCC459D35D773F8DFAF523DFED8FCA45908D0F9ED0D4BCEAC3743AF39F11310EAF3DFF45");
        }

        public int  Id          { get; }
        public bool IsConnected { get; private set; }

        #region Bot Functions

        public async void JoinRoom(int roomId)
        {
            await _clientSocket.SendAsync(Header.GetOutgoingHeader("FlatOpc"), 0, roomId, "", -1, -1);
        }

        public async void AntiAfk()
        {
            await _clientSocket.SendAsync(Header.GetOutgoingHeader("Move"), 1337, 1337);
        }

        public async void ChangeFigure(string figure, string gender)
        {
            await _clientSocket.SendAsync(Header.GetOutgoingHeader("UpdateAvatar"), gender, figure);
        }

        #endregion

        #region Bot Connection

        public async void Connect()
        {
            try
            {
                _hexKey = GetRandomHexNumber();

                _clientSocket = await HNode.ConnectAsync(await HotelEndPoint.ParseAsync("game-br.habbo.com", 30001));
                _clientSocket.ReceiveFormat = HFormat.EvaWire;
                _clientSocket.SendFormat = HFormat.EvaWire;

                _clientSocket.IsWebSocket = true;
                await _clientSocket.UpgradeWebSocketAsClientAsync();

                await _clientSocket.SendAsync(Header.GetOutgoingHeader("Hello"), _hexKey, "UNITY1", 0, 0);

                await _clientSocket.SendAsync(Header.GetOutgoingHeader("InitDhHandshake"));

                HandlePacketAsync(await _clientSocket.ReceiveAsync());
            }
            catch
            {
                Connect();
            }
        }

        private async Task HandlePacketAsync(HPacket packet)
        {
            try
            {
                if (!_clientSocket.IsConnected) return;

                if (packet.Id == Header.GetIncomingHeader("DhInitHandshake"))
                    await VerifyPrimesAsync(packet.ReadUTF8(), packet.ReadUTF8());
                else if (packet.Id == Header.GetIncomingHeader("DhCompleteHandshake"))
                    await CryptConnectionAsync(packet.ReadUTF8());
                else if (packet.Id == Header.GetIncomingHeader("Ping"))
                    await _clientSocket.SendAsync(Header.GetOutgoingHeader("Pong"));
                else if (packet.Id == Header.GetIncomingHeader("Ok"))
                {
                    IsConnected = true;
                    BotConnected?.Invoke(this);
                }

                await HandlePacketAsync(await _clientSocket.ReceiveAsync());
            }
            catch
            {
                Disconnect();
            }
        }

        private void Disconnect()
        {
            IsConnected = false;

            if (CurrentTries < MaxTries)
            {
                BotTryingReconnect?.Invoke(this);

                Connect();

                return;
            }

            BotDisconnected?.Invoke(this);
        }

        private async Task SendStuffAsync()
        {
            await _clientSocket.SendAsync(Header.GetOutgoingHeader("GetIdentityAgreementTypes"));

            await _clientSocket.SendAsync(Header.GetOutgoingHeader("VersionCheck"), 0, Config.ProductVersion, "");


            await _clientSocket.SendAsync(Header.GetOutgoingHeader("UniqueMachineId"), GetRandomHexNumber(76).ToLower(),
                                          "n/a", "Chrome 88", "n/a");


            await _clientSocket.SendAsync(Header.GetOutgoingHeader("LoginWithTicket"), _sso, 0);
        }

        private async Task CryptConnectionAsync(string key)
        {
            var nonce = GetNonce(_hexKey);

            var chacha    = new byte[32];
            var sharedKey = _keyExchange.GetSharedKey(key);

            Buffer.BlockCopy(sharedKey, 0, chacha, 0, sharedKey.Length);

            _clientSocket.Decrypter = new ChaCha20(chacha, nonce);
            _clientSocket.Encrypter = new ChaCha20(chacha, nonce);

            await SendStuffAsync();
        }

        private async Task VerifyPrimesAsync(string p, string g)
        {
            _keyExchange.VerifyDHPrimes(p, g);
            _keyExchange.Padding = PKCSPadding.RandomByte;

            await _clientSocket.SendAsync(Header.GetOutgoingHeader("CompleteDhHandshake"), _keyExchange.GetPublicKey());
        }

        private byte[] GetNonce(string str)
        {
            var nonce                         = string.Empty;
            for (var i = 0; i < 8; i++) nonce += str.Substring(i * 3, 2);

            return Convert.FromHexString(nonce);
        }

        private string GetRandomHexNumber(int digits = 24)
        {
            var buffer = new byte[digits / 2];
            _random.NextBytes(buffer);
            var result = string.Concat(buffer.Select(x => x.ToString("X2")).ToArray());

            if (digits % 2 == 0)
                return result;

            return result + _random.Next(16).ToString("X");
        }

        #endregion
    }
}