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
using System.Windows.Shapes;

namespace ChatLab
{
    /// <summary>
    /// Логика взаимодействия для Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {
        DataBase dataBase = new DataBase();
        
        public Window2()
        {
            InitializeComponent();
        }

        private void RegistrationLoginButton_Click(object sender, RoutedEventArgs e)
        {
            Window1 window1 = new Window1();
            window1.Show();
            Close();
        }

        private async void RegisrtationButton_Click(object sender, RoutedEventArgs e)
        {
            List<List<string>> users = new List<List<string>>();
            if (RegistrationName.Text != null && RegistrationPassword.Text != null && RegistrationPassword.Text.Length >= 6 && RegistrationPassword.Text.Length < 21 && RegistrationName.Text.Length >= 2 && RegistrationName.Text.Length < 16)
            {
                users = await dataBase.DBOutputUsersCommand();
                for (int i = 1; i < users[0].Count(); i++)
                {
                    if (users[0][i] == RegistrationName.Text && users[1][i] == RegistrationPassword.Text)
                    {
                        MessageBox.Show("Такой профиль уже есть! Выберите функцию входа");
                        return;
                    }
                    else if (users[0][i] == RegistrationName.Text)
                    {
                        MessageBox.Show("Профиль с таким именем уже есть!");
                        return;
                    }
                }
                dataBase.DBInputCommand($"INSERT INTO users (name, password) VALUES " +
                    $"('{RegistrationName.Text}', '{RegistrationPassword.Text}') ON CONFLICT (name) DO NOTHING;");
                MainWindow mainWindow = new MainWindow();
                //PostOffice.Package = RegistrationName.Text;
                mainWindow.Username.Text = RegistrationName.Text;
                mainWindow.Show();
                Close();
            }
            else
            {
                MessageBox.Show("Поля не должны быть путсыми, а так же:\nДлина имени - не менее 2 символов и не более 15\nДлина пароля - не менее 6 символов и не более 20");
            }
        }


        private void RegistrationName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void RegistrationName_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RegisrtationButton_Click(sender, e);
            }
        }

        private void RegistrationPassword_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                RegisrtationButton_Click(sender, e);
            }
        }
    }
}
