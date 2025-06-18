using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;




namespace DemoEmelyanov.Windows
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            string login = BoxLogin.Text.Trim();
            string password = BoxPassword.Text; // если у тебя PasswordBox. Если TextBox — просто .Text

            if (string.IsNullOrEmpty(login))
            {
                MessageBox.Show("Вы не ввели логин");
                BoxLogin.Focus();
                return;
            }

            if (string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Вы не ввели пароль");
                BoxPassword.Focus();
                return;
            }

            // Ищем пользователя только по логину — чтобы проверять блокировку и счётчик попыток
            var user = UserDatabase.GetAllUsers()
                .FirstOrDefault(u => u.Login == login);

            if (user == null)
            {
                MessageBox.Show("Вы ввели неверный логин или пароль. Пожалуйста, проверьте ещё раз введённые данные.");
                return;
            }

            // Проверяем блокировку по флагу
            if (user.IsBlocked)
            {
                MessageBox.Show("Вы заблокированы. Обратитесь к администратору.");
                return;
            }

            // Проверяем блокировку по времени последнего входа (больше 30 дней — блокируем)
            if ((DateTime.Now - user.LastLogin).TotalDays > 30 && user.LastLogin != DateTime.MinValue)
            {
                user.IsBlocked = false;
                MessageBox.Show("Ваша учётная запись заблокирована из-за долгого отсутствия. Обратитесь к администратору.");
                return;
            }

            // Проверяем пароль
            if (user.Password != password)
            {
                user.FailedAttempts++;

                if (user.FailedAttempts >= 3)
                {
                    user.IsBlocked = false;
                    MessageBox.Show("Вы заблокированы после 3-х неудачных попыток. Обратитесь к администратору.");
                    return;
                }
                else
                {
                    MessageBox.Show($"Вы ввели неверный пароль. Осталось попыток: {3 - user.FailedAttempts}");
                    return;
                }
            }

            // Если пароль правильный — сбрасываем счётчик ошибок и обновляем дату последнего входа
            user.FailedAttempts = 0;
            user.LastLogin = DateTime.Now;

            MessageBox.Show("Вы успешно авторизовались");

            if (user.Role == "admin")
            {
                AdminPanel panel = new AdminPanel();
                panel.Show();
            }
            else
            {
                MessageBox.Show("Добро пожаловать, пользователь!");
                new ChangePassword(user).Show();
            }

            this.Close();
        }
    }
}