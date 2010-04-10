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

        public TalkWindow addTalk(MSNUser destiny)
        {
            TalkWindow selectedWindow;

            if (!talkList.ContainsKey(destiny))
            {
                selectedWindow = new TalkWindow(destiny);
                talkList.Add(destiny, selectedWindow);
            }
            else
            {
                selectedWindow = talkList[destiny];
            }

            return selectedWindow;
        }

    }
}
