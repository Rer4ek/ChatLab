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
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ToolTip;

namespace ChatLab
{
    public partial class MainWindow : Window
    {
        DataBase dataBase = new DataBase();
        Window1 window1 = new Window1();
        bool delete = false;

        void GoToEndBox(System.Windows.Controls.ListBox ListBoxMesagges)
        {
            ListBoxMesagges.SelectedIndex = ListBoxMesagges.Items.Count - 1;
            ListBoxMesagges.ScrollIntoView(ListBoxMesagges.SelectedItem);
        }

        async void PrintHistory()
        {
            List<List<string>> messages = new List<List<string>>();
            messages = await dataBase.DBOutputMessagesCommand();
            ListBoxMesagges.Items.Clear();
            for (int i = 1; i < messages[0].Count; i++)
            {
                CreateMessageBox(messages[0][i], messages[1][i], messages[2][i], $"idx{messages[3][i]}", ListBoxMesagges.Items);
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


        private async void SendMessage_Click(object sender, RoutedEventArgs e)
        {
            if (Message.Text.Length > 0)
            {
                List<List<string>> messages = new List<List<string>>();
                messages = await dataBase.DBOutputMessagesCommand();
                List<List<string>> messagesID = new List<List<string>>();
                messagesID = await dataBase.DBOutputMessagesID();
                dataBase.DBInputCommand($"INSERT INTO messages (name, textmessages, date, id) VALUES ('{Username.Text}', '{Message.Text}', '[{DateTime.Now}]', '{(messagesID[0].Count()).ToString()}');");
                dataBase.DBInputCommand($"INSERT INTO messageID (id) VALUES ('{messagesID[0].Count()}');");
                CreateMessageBox(Username.Text, Message.Text, $"[{DateTime.Now}]", $"idx{(messagesID[0].Count()).ToString()}", ListBoxMesagges.Items);
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

        private void CreateMessageBox(string name, string text, string date, string id, ItemCollection collection)
        {
            System.Windows.Controls.TextBox messageBox = new System.Windows.Controls.TextBox();
            messageBox.Width = 400;
            //messageBox.Height = 80;
            //messageBox.TextWrapping = true;
            messageBox.TextWrapping = TextWrapping.Wrap;
            //messageBox.VerticalScrollBarVisibility = ScrollBarVisibility.Visible;
            messageBox.AcceptsReturn = true;
            messageBox.IsReadOnly = true;
            messageBox.FontFamily = new FontFamily("Comic Sans MS");
            messageBox.Name = id;
            messageBox.Text = $"Пользователь {name}:\n{text}\n\n{date}";
            collection.Add(messageBox);
        }

        private void Message_GotFocus(object sender, RoutedEventArgs e)
        {
            Message.Text = string.Empty;
            ListBoxMesagges.Items.Refresh();
        }

        private async void ListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
           
        }

        private async void DeleteMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteMessageButton.Content.ToString() == "Удалить сообщение") DeleteMessageButton.Content = "Перестать удалять";
            else if (DeleteMessageButton.Content.ToString() == "Перестать удалять") DeleteMessageButton.Content = "Удалить сообщение";
            delete = !delete;
        }


        private async void ListBoxMesagges_GotFocus(object sender, RoutedEventArgs e)
        {

        }

        private async void ListBoxMesagges_FocusableChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
        }

        private async void ListBoxMesagges_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (delete)
            {
                List<List<string>> messages = new List<List<string>>();
                messages = await dataBase.DBOutputMessagesCommand();
                for (int i = 1; i < messages[0].Count; i++)
                {
                    Console.WriteLine($"FIRST - {ListBoxMesagges.SelectedIndex}");
                    for (int j = 0; j < ListBoxMesagges.Items.Count; j++)
                    //{
                    //    Console.WriteLine($"{ListBoxMesagges.Items[j]} {j}");
                    //}
                    //Console.WriteLine($"SECOND - System.Windows.Controls.TextBox: Пользователь {messages[0][i]}:\n{messages[1][i]}\n\n{messages[2][i]}");
                    //if (messages[0][i] == Username.Text) Console.WriteLine("USERNAME CHECK");
                    //if ($"System.Windows.Controls.TextBox: Пользователь {messages[0][i]}:\n{messages[1][i]}\n\n{messages[2][i]}" == ListBoxMesagges.Items[ListBoxMesagges.SelectedIndex].ToString()) Console.WriteLine("MESSAGE CHECK");
                    if (messages[0][i] == Username.Text && $"System.Windows.Controls.TextBox: Пользователь {messages[0][i]}:\n{messages[1][i]}\n\n{messages[2][i]}" == ListBoxMesagges.Items[ListBoxMesagges.SelectedIndex].ToString())
                    {
                        Console.WriteLine("DELETE ELEMENT AFTER");
                        object selectedItem = ListBoxMesagges.Items[ListBoxMesagges.SelectedIndex];
                        System.Windows.Controls.TextBox textBox = selectedItem as System.Windows.Controls.TextBox;
                        string[] name = textBox.Name.Split('x');
                        Console.WriteLine(name[1]);
                        dataBase.DBInputCommand($"DELETE FROM messages WHERE id = '{name[1]}';");
                        Console.WriteLine("Second");
                        ListBoxMesagges.Items.RemoveAt(ListBoxMesagges.SelectedIndex);
                        ListBoxMesagges.Items.Refresh();
                        return;
                    }
                }
                ListBoxMesagges.Items.Refresh();
            }
        }
    }
}
