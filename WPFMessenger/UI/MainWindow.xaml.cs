using System.Collections.Generic;
using System.Windows;
using WPFMessenger.Core;
using System.Windows.Controls;
using System;
using System.Diagnostics;
using System.Windows.Documents;
using WPFMessenger.UI;
using System.ComponentModel;
using System.Threading;

namespace WPFMessenger
{
    public partial class MainWindow : Window
    {

        public IList<MSNUser> listUsers;

        public TCPConnection TCP { get; set; }

        private string rootTitle;

        private Dictionary<string, MSNUser> dicTreeItems;

        private TalkManager talkManager;

        //atualiza lista de usuários a cada 6 segundos
        private int timeRefreshUsers = 6000;

        private bool firstRefresh = true;

        public MainWindow()
        {
            InitializeComponent();

            Closing += Window_Closing;

            rootTitle = treeItemRoot.Header.ToString();

            dicTreeItems = new Dictionary<string, MSNUser>();

            talkManager = new TalkManager();
            LoadRSS();
        }

        private void IntializerRefresher()
        {
            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += RefreshUsers;
            bw.RunWorkerCompleted += LoadTreeView;
            bw.RunWorkerAsync();
        }

        private void RefreshUsers(object sender, DoWorkEventArgs e)
        {
            if (!firstRefresh)
            {
                Thread.Sleep(timeRefreshUsers);
            }
            else
            {
                firstRefresh = false;
            }

            listUsers = TCP.GetListUsers();

            Console.WriteLine(String.Format("Atualizou lista de usuários: {0}", System.DateTime.Now));
        }


        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            Application.Current.Shutdown();
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            IntializerRefresher();
        }

        private void LoadRSS()
        {
            RSSReader r = new RSSReader();
            r.Read();

            Run texto = null;
            Hyperlink link = null;
            TextBlock block;

            int startIndex;

            RSSNews news;

            for (int i = 0; i < r.ListNews.Count && i < 5; i++)
            {

                news = r.ListNews[i];

                startIndex = (news.Title.Length-1 >= 45 ? 45 : news.Title.Length-1);

                texto = new Run(String.Format("{0}...", news.Title.Remove(startIndex)));
                link = new Hyperlink(texto);
                link.NavigateUri = new Uri(news.Link);
                link.RequestNavigate += Hyperlink_RequestNavigate;

                link.Style = (Style)FindResource("LinkStyle");

                block = new TextBlock();
                block.Style = (Style)FindResource("TBStyle");
                block.Inlines.Add(link);
                panelRSS.Children.Add(block);
            }
        }

        public void LoadTreeView(object sender, RunWorkerCompletedEventArgs e)
        {
            TreeViewItem node;

            String userDisplay = null;

            foreach (MSNUser user in listUsers)
            {
               userDisplay = FormatUserDisplay(user);

               if (!dicTreeItems.ContainsKey(userDisplay))
               {
                   node = new TreeViewItem();
                   node.Header = userDisplay;
                   node.FontSize = 12;
                   node.Selected += ShowTalkWindow;
                   treeItemRoot.Items.Add(node);
                   dicTreeItems.Add(node.Header.ToString(), user);

                   Console.WriteLine(String.Format("Usuário adicionado: {0}", user.UserName));
               }
            }

            treeItemRoot.IsExpanded = true;
            rootTitle = rootTitle.Replace("(x)", String.Format("({0})", treeItemRoot.Items.Count));

            treeItemRoot.Header = rootTitle;

            IntializerRefresher();
        }

        private void ShowTalkWindow(object sender, RoutedEventArgs e)
        {
            TreeViewItem selectedItem = (TreeViewItem)treeUsers.SelectedItem;

            TalkWindow selectedWindow = talkManager.addTalk(dicTreeItems[selectedItem.Header.ToString()]);

            selectedWindow.Show();
            selectedWindow.Focus();
        }

        private String FormatUserDisplay(MSNUser user)
        {
            return String.Format("{0} (id: {1})", user.UserName, user.UserID);
        }

    }
}
