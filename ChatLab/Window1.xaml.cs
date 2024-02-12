using Microsoft.Win32;
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
using Npgsql;
using System.Data.Common;
using System.Collections.ObjectModel;
using Npgsql.Internal;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.StartPanel;

namespace ChatLab
{
    public partial class Window1 : Window
    {
        DataBase dataBase = new DataBase();
        public Window1()
        {
            InitializeComponent();
            //dataBase.DBInputCommand($"INSERT INTO users (name, id) VALUES ('{SignText.Text}', ROW_NUMBER() over())");
        }



        private async void Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            List<List<string>> users = new List<List<string>>();
            if (SignText.Text != null && SignPasswordText.Text != null && SignPasswordText.Text.Length >= 6 && SignPasswordText.Text.Length < 21 && SignText.Text.Length >= 2 && SignText.Text.Length < 16)
            {
                users = await dataBase.DBOutputUsersCommand();
                for (int i = 1; i < users[0].Count(); i++)
                {
                    if (users[0][i] == SignText.Text && users[1][i] == SignPasswordText.Text)
                    {
                        mainWindow.Show();
                        //PostOffice.Package = SignText.Text;
                        //Console.WriteLine($"{PostOffice.Package}    {SignText.Text}");
                        mainWindow.Username.Text = SignText.Text;
                        Close();
                        return;
                    }
                    else if (users[0][i] == SignText.Text)
                    {
                        MessageBox.Show("Введите пароль!");
                        return;
                    }
                }
                MessageBox.Show("Нет профился с таким именем!");
            }
            else
            {
                MessageBox.Show("Поля не должны быть путсыми, а так же:\nДлина имени - не менее 2 символов и не более 15\nДлина пароля - не менее 6 символов и не более 20");
            }
        }

        private void TextBox_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void GoToRegistrationButton_Click(object sender, RoutedEventArgs e)
        {
            Window2 window2 = new Window2();
            window2.Show();
            Close();
        }


        private void Grid_KeyUp(object sender, KeyEventArgs e)
        {

        }

        private void SignPasswordText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }

        private void SignText_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                Button_Click(sender, e);
            }
        }
    }

    
}
