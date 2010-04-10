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
        private UDPConnection udp;

        private MSNUser destinyUser;

        internal MSNUser DestinyUser
        {
            get { return destinyUser; }
            set { 
                    destinyUser = value;
                    this.Title = String.Format("{0} - Conversa", destinyUser.UserName);
                }
        }

        public TalkWindow(MSNUser destiny)
        {
            InitializeComponent();

            DestinyUser = destiny;

            Closing += Window_Closing;
            KeyDown += Window_KeyDown;
            tcp = new TCPConnection();
            udp = new UDPConnection();
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if(e != null)
                e.Cancel = true;

            Visibility = Visibility.Collapsed;
        }

        private void Window_KeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {

            if (e.Key.ToString() == "Escape")
                this.Window_Closing(sender, null);

        }

        private void btEnviar_Click(object sender, RoutedEventArgs e)
        {
            string message = msgBox.Text.ToString();

            udp.SendMessage(this.destinyUser, message);
            msgBox.Clear();

            textBoard.Text += message;
            textBoard.Text += System.Environment.NewLine;

            Console.WriteLine(tcp.GetMyMessages());
        }

    }
}
