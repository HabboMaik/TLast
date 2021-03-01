using System;

namespace TLast.Models
{
    internal class AccountModel
    {
        public string Email;
        public string Password;

        public AccountModel(string account)
        {
            var split = account.Split(':', StringSplitOptions.RemoveEmptyEntries);

            if (split.Length != 2) return;

            Email    = split[0];
            Password = split[1];
        }

        public bool IsValid => Email != null && Password != null;
    }
}