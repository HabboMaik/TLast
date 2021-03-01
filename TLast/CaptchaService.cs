using System.Collections.Generic;

using TLast.Models;

using WebSocketSharp;
using WebSocketSharp.Server;

namespace TLast
{
    internal class CaptchaService : WebSocketBehavior
    {
        private readonly Queue<AccountModel> _accounts;
        private readonly BotHandler _handler;
        private readonly LogFrm _logFrm;

        public CaptchaService(Queue<AccountModel> accounts, BotHandler handler, LogFrm logFrm)
        {
            _accounts = accounts;
            _handler = handler;
            _logFrm = logFrm;
        }

        protected override void OnMessage(MessageEventArgs e)
        {
            if (e.IsBinary || string.IsNullOrWhiteSpace(e.Data) || _accounts.Count == 0)
            {
                Context.WebSocket.Close();

                return;
            }

            _logFrm.LogInfo("Novo captcha recebido.");

            var currentAccount = _accounts.Dequeue();
            var captchaToken = e.Data;

            _handler.AddBotByAccount(currentAccount, captchaToken);

            if (_accounts.Count == 0) _logFrm.LogWarning("Todas as contas foram usadas.");
        }
    }
}