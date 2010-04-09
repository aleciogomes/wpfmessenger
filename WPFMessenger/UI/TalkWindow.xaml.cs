using System.Windows;
using WPFMessenger.Core;
using System;

namespace WPFMessenger.UI
{
    /// <summary>
    /// Interaction logic for TalkWindow.xaml
    /// </summary>
    public partial class TalkWindow : Window
    {

        private TCPConnection tcp;

        private MSNUser currentUser;

        private MSNUser destinyUser;

        internal MSNUser User
        {
            get { return currentUser; }
            set{ currentUser = value;}
        }

        internal MSNUser DestinyUser
        {
            get { return destinyUser; }
            set { 
                    destinyUser = value;
                    this.Title = String.Format("{0} - Conversa", destinyUser.UserName);
                }
        }

        internal TCPConnection Tcp
        {
            get { return tcp; }
            set { tcp = value; }
        }

        public TalkWindow(MSNUser me, MSNUser destiny)
        {
            User = me;
            DestinyUser = destiny;

            InitializeComponent();
            Closing += Window_Closing;
            tcp = new TCPConnection();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            e.Cancel = true;
            Visibility = Visibility.Collapsed;
        }

        private void btEnviar_Click(object sender, RoutedEventArgs e)
        {
            string message = msgBox.Text.ToString();

            tcp.SendMessage(this.destinyUser, message);
            msgBox.Clear();

            textBoard.Text += message;
            textBoard.Text += System.Environment.NewLine;

            tcp.teste(currentUser);
        }

    }
}
