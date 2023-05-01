﻿using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Windows;

namespace Messenger.Classes
{
    internal class Authorization
    {
        private MessengerContext _context = new MessengerContext();

        /// <summary>
        /// Автоматическая авторизация в приложении
        /// </summary>
        internal void AutoLogin()
        {
            string login, password;
            bool? remember;
            try
            {
                if (Properties.Settings.Default.UserName != string.Empty)
                {
                    login = Properties.Settings.Default.UserName;
                    password = Properties.Settings.Default.Password;
                    remember = true;
                    Login(login, password, remember);
                }
            }
            catch
            {
                throw new Exception("Ошибка подключения");
            }
        }

        /// <summary>
        /// Авторизация в приложении
        /// </summary>
        /// <param name="remember">Параметр отвечающий за включение автоматической авторизации</param>
        /// <returns>Если пользователь был найден возвращает true, иначе false</returns>
        internal bool Login(string login, string password, bool? remember = false)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
                {
                    MessageBox.Show("Введите логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }

                User? user = GetUser(login, password);
                if (user is not null)
                {
                    if (remember == true)
                        Settings.RememberMe(login, password);
                    else
                        Settings.NotRememberMe();

                    LoggedUser.currentUser = user;
                    LoggedUser.SetUserType(user.UserTypeId);
                    return true;
                }
                else
                {
                    MessageBox.Show("Неправильный логин или пароль", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return false;
                }
            }
            catch
            {
                throw new Exception("Ошибка подключения");
            }
        }

        /// <summary>
        /// Обращается к базе данных для нахождения пользователя
        /// </summary>
        /// <returns>Возвращает найденного пользователя, если пользователь не был найден возвращает null</returns>
        private User? GetUser(string login, string password)
        {
            return _context.Users.Where(x => (x.Login == login || x.Email == login) && x.Password == password)
                .Include(x => x.Post)
                .FirstOrDefault();
        }
    }
}
