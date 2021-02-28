using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TLast
{
    public class BotHandler
    {
        private readonly List<Bot> _bots = new();
        private readonly LogFrm    _logFrm;

        private bool _antiAfk;
        private int  _currentBotId;

        public Action<int> BotCountUpdated;

        public BotHandler(LogFrm logFrm)
        {
            _logFrm         =  logFrm;
            _logFrm.AntiAFK += AntiAfkChanged;

            AntiAfkChanged(true);
        }

        private void AntiAfkChanged(bool antiAfkEnabled)
        {
            _antiAfk = antiAfkEnabled;

            if (_antiAfk) new Thread(RunAntiAfkTask).Start();
        }

        private async void RunAntiAfkTask()
        {
            while (_antiAfk)
            {
                try
                {
                    DoBotAction(x => x.AntiAfk());
                }
                catch
                {
                    // ignored
                }

                await Task.Delay(TimeSpan.FromMinutes(2));
            }
        }

        public void AddBot(string sso)
        {
            var bot = new Bot(++_currentBotId, sso);
            bot.BotConnected       += BotConnected;
            bot.BotDisconnected    += BotDisconnected;
            bot.BotTryingReconnect += BotTryingReconnect;

            _logFrm.LogInfo($"Bot #{bot.Id} Conectando...");

            bot.Connect();
            _bots.Add(bot);
        }

        private void BotTryingReconnect(Bot sender)
        {
            _logFrm.LogWarning($"Bot #{sender.Id} Reconectando ({sender.CurrentTries + 1}/{sender.MaxTries})...");
        }

        private void DoBotAction(Action<Bot> callback)
        {
            try
            {
                var connectedBots = _bots.Where(x => x.IsConnected);

                if (!connectedBots.Any()) return;

                foreach (var bot in connectedBots) callback(bot);
            }
            catch
            {
                // ignored
            }
        }

        public void JoinRoom(int roomId)
        {
            DoBotAction(x => x.JoinRoom(roomId));
        }

        public void ChangeFigure(string figure, string gender)
        {
            DoBotAction(x => x.ChangeFigure(figure, gender));
        }

        private void BotDisconnected(Bot sender)
        {
            _logFrm.LogError($"Bot #{sender.Id} Desconectado.");
            _bots.Remove(sender);
            sender.Dispose();

            BotCountUpdated?.Invoke(_bots.Count(x => x.IsConnected));
        }

        private void BotConnected(Bot sender)
        {
            _logFrm.LogSuccess($"Bot #{sender.Id} Conectado.");

            BotCountUpdated?.Invoke(_bots.Count(x => x.IsConnected));
        }
    }
}