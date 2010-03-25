using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using WPFMessenger.UI;

namespace WPFMessenger.Core
{
    class TalkManager
    {
        private Dictionary<MSNUser, TalkWindow> talkList;

        public Dictionary<MSNUser, TalkWindow> TalkList
        {
            get { return talkList; }
            set { talkList = value; }
        }

        public TalkManager()
        {

            talkList = new Dictionary<MSNUser, TalkWindow>();
        }

        public void addTalk(MSNUser user)
        {
            TalkWindow selectedWindow;

            if (!talkList.ContainsKey(user))
            {
                selectedWindow = new TalkWindow();
                talkList.Add(user, selectedWindow);
            }
            else
            {
                selectedWindow = talkList[user];
            }

            selectedWindow.Show();
        }

    }
}
