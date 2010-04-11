using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFMessenger.UI;
using System.ComponentModel;
using System.Windows;
using System.Media;
using System.Windows.Media;

namespace WPFMessenger.Core
{
    class TalkManager
    {

        private MediaPlayer mp;

        private IList<MSNMessage> messageList;

        public Dictionary<MSNUser, TalkWindow> TalkList { get; set; }
        public Dictionary<int, MSNUser> UserList { get; set; }

        public TalkManager()
        {
            TalkList = new Dictionary<MSNUser, TalkWindow>();
            UserList = new Dictionary<int, MSNUser>();

            mp = new MediaPlayer();
            Uri mp3Adress = new Uri(@".\resources\alert.mp3", UriKind.Relative);
            mp.Open(mp3Adress);
        }

        public TalkWindow addTalk(MSNUser destiny)
        {
            TalkWindow selectedWindow;

            if (!TalkList.ContainsKey(destiny))
            {
                selectedWindow = new TalkWindow(destiny);
                TalkList.Add(destiny, selectedWindow);
            }
            else
            {
                selectedWindow = TalkList[destiny];
            }

            return selectedWindow;
        }

        public void IntializerMsgRefresher()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += GetMessages;
            bw.RunWorkerCompleted += RefreshMessagesBoard;
            bw.RunWorkerAsync();
        }

        private void GetMessages(object sender, DoWorkEventArgs e)
        {
            messageList = TCPConnection.GetMyMessages();
        }


        private void RefreshMessagesBoard(object sender, RunWorkerCompletedEventArgs e)
        {

            MSNUser forwarder = null;

            foreach (MSNMessage mensagem in messageList)
            {

                forwarder = UserList[mensagem.Forwarder];

                if (forwarder != null)
                {
                    this.ActivateWindow(forwarder, mensagem.Message);
                }
            }

        }

        private void ActivateWindow(MSNUser ownerUser, string message)
        {

            mp.Stop();
            mp.Play();

            TalkWindow window = TalkList[ownerUser];

            if (window == null)
            {
                window = addTalk(ownerUser);   
            }

            if (!window.IsVisible)
            {
                window.Visibility = Visibility.Visible;
                window.WindowState = WindowState.Minimized;
            }

            window.InsertMessage(ownerUser, message);

        }

    }
}
