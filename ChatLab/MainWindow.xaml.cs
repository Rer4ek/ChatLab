using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using MessageBox = System.Windows.MessageBox;

namespace ChatLab
{

    public partial class MainWindow : Window
    {
        DataBase dataBase = new DataBase();
        Window1 window1 = new Window1();
        bool delete = false;

        private void Window_Loaded()
        {
            System.Windows.Threading.DispatcherTimer timer = new System.Windows.Threading.DispatcherTimer();

            timer.Tick += new EventHandler(timerTick);
            timer.Interval = new TimeSpan(0, 0, 5);
            timer.Start();
        }

        private void timerTick(object sender, EventArgs e)
        {
            PrintHistory();
        }

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
            Window_Loaded();
            PrintHistory();
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
                CreateMessageBox(Username.Text, Message.Text, $"[{DateTime.Now}]", $"idx{Message.Text.Length.ToString()}", ListBoxMesagges.Items);
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
            messageBox.TextWrapping = TextWrapping.Wrap;
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

        private void DeleteMessageButton_Click(object sender, RoutedEventArgs e)
        {
            if (DeleteMessageButton.Content.ToString() == "Удалить сообщение") DeleteMessageButton.Content = "Выберите сообщение";
            else if (DeleteMessageButton.Content.ToString() == "Выберите сообщение") DeleteMessageButton.Content = "Удалить сообщение";
            delete = !delete;
        }

        private async void ListBoxMesagges_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (delete)
            {
                List<List<string>> messages = new List<List<string>>();
                messages = await dataBase.DBOutputMessagesCommand();
                for (int i = 1; i < messages[0].Count; i++)
                {
                    for (int j = 0; j < ListBoxMesagges.Items.Count; j++)
                    if (messages[0][i] == Username.Text && $"System.Windows.Controls.TextBox: Пользователь {messages[0][i]}:\n{messages[1][i]}\n\n{messages[2][i]}" == ListBoxMesagges.Items[ListBoxMesagges.SelectedIndex].ToString())
                    {
                            string messageBoxText = "Вы хотите удалить сообщение?";
                            string caption = "Этаче";
                            MessageBoxButton button = MessageBoxButton.YesNo;
                            MessageBoxResult result = MessageBox.Show(messageBoxText, caption, button);
                            if (result == MessageBoxResult.Yes)
                            {
                                // Выполните действие для кнопки "Да"
                                object selectedItem = ListBoxMesagges.Items[ListBoxMesagges.SelectedIndex];
                                System.Windows.Controls.TextBox textBox = selectedItem as System.Windows.Controls.TextBox;
                                string[] name = textBox.Name.Split('x');
                                dataBase.DBInputCommand($"DELETE FROM messages WHERE id = '{name[1]}';");
                                ListBoxMesagges.Items.RemoveAt(ListBoxMesagges.SelectedIndex);
                            }
                            if(result == MessageBoxResult.No)
                            {
                                continue;
                            }
                            // Другие варианты
                            ListBoxMesagges.Items.Refresh();
                            DeleteMessageButton.Content = "Удалить сообщение";
                            delete = !delete;
                            return;
                    }
                }
                ListBoxMesagges.Items.Refresh();
            }
        }
    }
}
