using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Principal;

namespace GetKey
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Console.ForegroundColor = ConsoleColor.Green;

            // Console.WriteLine();

            Console.WriteLine(GetUniqueId(Environment.UserName));

            Console.ReadLine();
        }

        private static string GetUniqueId([NotNull] string id)
        {
            var f         = new NTAccount(Environment.UserName);
            var s         = (SecurityIdentifier) f.Translate(typeof(SecurityIdentifier));
            var sidString = s.ToString();

            return sidString;
        }
    }
}