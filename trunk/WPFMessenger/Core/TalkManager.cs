using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFMessenger.UI;

namespace WPFMessenger.Core
{
    class TalkManager
    {

        private MSNUser currentUser;

        private Dictionary<MSNUser, TalkWindow> talkList;

        internal MSNUser User
        {
            get { return currentUser; }
            set { currentUser = value; }
        }

        public Dictionary<MSNUser, TalkWindow> TalkList
        {
            get { return talkList; }
            set { talkList = value; }
        }

        public TalkManager(MSNUser me)
        {
            User = me;
            talkList = new Dictionary<MSNUser, TalkWindow>();
        }

        public void addTalk(MSNUser destiny)
        {
            TalkWindow selectedWindow;

            if (!talkList.ContainsKey(destiny))
            {
                selectedWindow = new TalkWindow(currentUser, destiny);
                talkList.Add(destiny, selectedWindow);
            }
            else
            {
                selectedWindow = talkList[destiny];
            }

            selectedWindow.Show();
        }

    }
}
