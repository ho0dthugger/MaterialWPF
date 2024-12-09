using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using MaterialWPF.File;
using MaterialWPF.Pages.Admin;
using MaterialWPF.Pages.User;

namespace MaterialWPF.Pages
{
    public partial class PageLogin : Page
    {
        private MaterialEntities dbContext;

        public PageLogin()
        {
            InitializeComponent();
            dbContext = new MaterialEntities();

            // Подписываемся на событие Click для кнопки "Закрыть приложение"
            btnClose.Click += btnClose_Click;

            // Подписываемся на событие Click для кнопки "Свернуть окно"
            btnMinimize.Click += btnMinimize_Click;
        }

        private void btnMinimize_Click(object sender, RoutedEventArgs e)
        {
            Window parentWindow = Window.GetWindow(this);
            if (parentWindow != null)
            {
                parentWindow.WindowState = WindowState.Minimized;
            }
        }

        private void btnClose_Click(object sender, RoutedEventArgs e)
        {
            // Закрываем приложение
            Application.Current.Shutdown();
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string username = txtUsername.Text;
            string password = txtPassword.Password;

            if (IsValidLogin(username, password, out string userRole))
            {
                txtStatus.Text = "Успешная авторизация!";
                txtStatus.Foreground = System.Windows.Media.Brushes.Green;

                if (userRole == "Администратор")
                {
                    NavigationService.Navigate(new PageAdmin());
                }
                else if (userRole == "Работник")
                {
                    NavigationService.Navigate(new PageUser());
                }
            }
            else
            {
                txtStatus.Text = "Неверный логин или пароль!";
                txtStatus.Foreground = System.Windows.Media.Brushes.Red;
            }
        }

        private bool IsValidLogin(string username, string password, out string userRole)
        {
            // Ищем пользователя в базе данных
            var user = dbContext.Пользователи.FirstOrDefault(u => u.Логин == username && u.Пароль == password);

            if (user != null)
            {
                userRole = user.Роль;
                return true;
            }

            userRole = null;
            return false;
        }
    }
}