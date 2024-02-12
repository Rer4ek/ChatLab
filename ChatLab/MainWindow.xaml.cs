using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Forms;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using static System.Net.Mime.MediaTypeNames;

namespace ChatLab
{
    public partial class MainWindow : Window
    {
        DataBase dataBase = new DataBase();
        Window1 window1 = new Window1();

        void GoToEndBox(System.Windows.Controls.ListBox ListBoxMesagges)
        {
            ListBoxMesagges.SelectedIndex = ListBoxMesagges.Items.Count - 1;
            ListBoxMesagges.ScrollIntoView(ListBoxMesagges.SelectedItem);
        }

        async void PrintHistory()
        {
            List<List<string>> messages = new List<List<string>>();
            messages = await dataBase.DBOutputMessagesCommand();
            for (int i = 1; i < messages[0].Count; i++)
            {
                CreateMessageBox(messages[0][i], messages[1][i], messages[2][i], ListBoxMesagges.Items);
            }
            GoToEndBox(ListBoxMesagges);
        }

        public MainWindow()
        {
            InitializeComponent();
            PrintHistory();
            //Username.Text = PostOffice.Package;
        }

        private void ChatHistory_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (Message.Text.Length > 0)
            {
                dataBase.DBInputCommand($"INSERT INTO messages (name, textmessages, date) VALUES ('{Username.Text}', '{Message.Text}', '[{DateTime.Now}]');");
                CreateMessageBox(Username.Text, Message.Text, $"[{DateTime.Now}]", ListBoxMesagges.Items);
                Message.Text = string.Empty;
                GoToEndBox(ListBoxMesagges);
            }
        }

        private void Message_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage_Click(sender, e);
                GoToEndBox(ListBoxMesagges);
            }
        }

        private void CreateMessageBox(string name, string text, string date, ItemCollection collection)
        {
            System.Windows.Controls.TextBox messageBox = new System.Windows.Controls.TextBox();
            messageBox.Width = 400;
            messageBox.Height = 80;
            messageBox.TextWrapping = TextWrapping.Wrap;
            messageBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            messageBox.AcceptsReturn = true;
            messageBox.IsReadOnly = true;
            messageBox.FontFamily = new FontFamily("Comic Sans MS");
            messageBox.Text = $"Пользователь {name}:\n{text}\n\n[{DateTime.Now}]";
            collection.Add(messageBox);
        }

        private void Message_GotFocus(object sender, RoutedEventArgs e)
        {
            Message.Text = string.Empty;
        }

        private void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //ListBoxMesagges.Items.Add(ChatHistory);
        }
    }
}
