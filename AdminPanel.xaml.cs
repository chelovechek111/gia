using System.Windows;
using System.Collections.Generic;

namespace DemoEmelyanov.Windows
{
    public partial class AdminPanel : Window
    {
        private List<AppUser> users;

        public AdminPanel()
        {
            InitializeComponent();
            users = UserDatabase.GetAllUsers();
            UsersListBox.ItemsSource = users;
        }

        private void UnblockUser_Click(object sender, RoutedEventArgs e)
        {
            var selectedUser = UsersListBox.SelectedItem as AppUser;
            if (selectedUser == null)
            {
                MessageBox.Show("Пожалуйста, выберите пользователя для разблокировки.");
                return;
            }

            if (!selectedUser.IsBlocked)
            {
                MessageBox.Show("Этот пользователь уже разблокирован.");
                return;
            }

            selectedUser.IsBlocked = false;
            selectedUser.FailedAttempts = 0;
            MessageBox.Show($"Пользователь {selectedUser.Login} разблокирован.");

            UsersListBox.ItemsSource = null;
            UsersListBox.ItemsSource = users;
        }
        private void ReturnToAuth_Click(object sender, RoutedEventArgs e)
        {
            Authorization authWindow = new Authorization();
            authWindow.Show();
            this.Close();
        }
    }
}
