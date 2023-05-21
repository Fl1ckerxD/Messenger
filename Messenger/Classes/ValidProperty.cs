using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Messenger.Classes
{
    internal class ValidProperty
    {
        private static readonly Regex regex = new Regex("^[+]?[0-9]+$");
        /// <summary>
        /// Определяет, является ли получаемый текст почтой
        /// </summary>
        /// <returns>Возвращает true если является почтой, иначе false</returns>
        public static bool IsValidEmail(string email)
        {
            var trimmedEmail = email.Trim();

            if (trimmedEmail.EndsWith("."))
            {
                return false;
            }
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == trimmedEmail;
            }
            catch
            {
                return false;
            }
        }
        public static bool IsValidPhone(string phone)
        {
            return regex.IsMatch(phone);
        }
    }
}
