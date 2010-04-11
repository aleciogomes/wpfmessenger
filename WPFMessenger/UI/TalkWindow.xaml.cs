using System.Windows;
using WPFMessenger.Core;
using System;
using System.Windows.Controls;

namespace WPFMessenger.UI
{
    /// <summary>
    /// Interaction logic for TalkWindow.xaml
    /// </summary>
    public partial class TalkWindow : Window
    {
        private UDPConnection udp;

        private MSNUser destinyUser;

        internal MSNUser DestinyUser
        {
            get { return destinyUser; }
            set { 
                    destinyUser = value;
                    this.Title = String.Format("{0} - Conversa", destinyUser.UserName);
                    this.SetLabelText(lblDestinyUser, destinyUser.UserName);
                }
        }

        public TalkWindow(MSNUser destiny)
        {
            InitializeComponent();

            DestinyUser = destiny;

            Closing += Window_Closing;
            KeyDown += Window_KeyDown;
            udp = new UDPConnection();

            this.SetLabelText(lblCurrentUser, MSNSession.User.UserName);
        }

        private void SetLabelText(TextBlock lbl, string text)
        {
            int startIndex = (text.Length - 1 >= 15 ? 15 : text.Length - 1);

            string nome = String.Format("{0}...", text.Remove(startIndex));
            lbl.Text = nome;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(e != null)
                e.Cancel = true;

            Visibility = Visibility.Collapsed;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key.ToString().Equals("Escape"))
            {
                this.Window_Closing(sender, null);
            }
            else if (msgBox.IsFocused && e.Key.ToString().Equals("Return"))
            {
                btEnviar.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
            }

        }


        private void btEnviar_Click(object sender, RoutedEventArgs e)
        {
            string message = msgBox.Text.ToString();

            if (!message.Equals(String.Empty))
            {
                udp.SendMessage(this.destinyUser, message);
                msgBox.Clear();

                this.InsertMessage(MSNSession.User, message);
            }

        }

        public void InsertMessage(MSNUser user, string newMessage)
        {
            string message = String.Format("({0}) {1} diz:", System.DateTime.Now, user.UserName);

            message += System.Environment.NewLine;

            //tab
            message += "\t";

            message += newMessage;
            message += System.Environment.NewLine;

            textBoard.Text += message;
        }

    }
}
