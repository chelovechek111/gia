using System.Windows;

namespace DemoEmelyanov.Windows
{
    public partial class ChangePassword : Window
    {
        private AppUser currentUser;

        public ChangePassword(AppUser user)
        {
            InitializeComponent();
            currentUser = user;
        }

        private void BtnChangePassword_Click(object sender, RoutedEventArgs e)
        {
            string oldPass = BoxOldPassword.Text;
            string newPass = BoxNewPassword.Text;
            string confirmPass = ApplyPassword.Text;

            if (string.IsNullOrWhiteSpace(oldPass) || string.IsNullOrWhiteSpace(newPass) || string.IsNullOrWhiteSpace(confirmPass))
            {
                MessageBox.Show("Все поля обязательны для заполнения");
                return;
            }

            if (oldPass != currentUser.Password)
            {
                currentUser.FailedAttempts++;
                if (currentUser.FailedAttempts >= 3)
                {
                    currentUser.IsBlocked = true;
                    MessageBox.Show("Вы заблокированы. Обратитесь к администратору.");
                    this.Close();
                    return;
                }
                MessageBox.Show($"Старый пароль введён неверно. Попытка {currentUser.FailedAttempts} из 3.");
                return;
            }

            if (newPass != confirmPass)
            {
                MessageBox.Show("Новый пароль и подтверждение не совпадают");
                return;
            }

            // Всё верно — меняем пароль
            currentUser.Password = newPass;
            currentUser.FailedAttempts = 0; // сбрасываем счетчик неудачных попыток
            MessageBox.Show("Пароль успешно изменён");
            this.Close();
        }
    }
}
