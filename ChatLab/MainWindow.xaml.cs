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
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ChatLab
{
    public partial class MainWindow : Window
    {
        string historyPath = "history.txt";

        void GoToEndBox(TextBox box)
        {
            box.Focus();
            box.CaretIndex = box.Text.Length;
            box.ScrollToEnd();
        }

        void PrintHistory()
        {

            ChatHistory.Text = string.Empty;
            using (StreamReader reader = new StreamReader(historyPath))
            {
                string line;
                while ((line =  reader.ReadLine()) != null)
                {
                    ChatHistory.Text += $"{line}\n";
                }
                reader.Close();
            }
            GoToEndBox(ChatHistory);
        }

        public MainWindow()
        {
            InitializeComponent();
            PrintHistory();

        }

        private void ChatHistory_TextChanged(object sender, TextChangedEventArgs e)
        {

        }


        private void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (Username.Text == string.Empty || Username.Text == "Пишите имя")
            {
                Username.Text = "Anonim";
            }

            using (StreamWriter writer = new StreamWriter(historyPath, true))
            {
                writer.WriteLineAsync($"{Username.Text} >> {Message.Text}    [{DateTime.Now}]");
                writer.Close();
            }

            ChatHistory.Text +=  $"{Username.Text} >> {Message.Text}    [{DateTime.Now}]\n";
            Message.Text = string.Empty;
        }

        private void Message_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                SendMessage_Click(sender, e);
            }
        }
    }
}
