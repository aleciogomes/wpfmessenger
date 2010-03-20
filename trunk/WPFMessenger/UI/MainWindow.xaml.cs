using System.Collections.Generic;
using System.Windows;
using WPFMessenger.Core;
using System.Windows.Controls;
using System;
using System.Diagnostics;
using System.Windows.Documents;

namespace WPFMessenger
{
    public partial class MainWindow : Window
    {

        private IList<MSNUser> listUsers;
        private string rootTitle;

        internal IList<MSNUser> ListUsers
        {
            get { return listUsers; }
            set { listUsers = value; }
        }

        public MainWindow()
        {
            InitializeComponent();
            rootTitle = treeItemRoot.Header.ToString();
            LoadRSS();
        }


        private void Hyperlink_RequestNavigate(object sender, System.Windows.Navigation.RequestNavigateEventArgs e)
        {
            Process.Start(new ProcessStartInfo(e.Uri.AbsoluteUri));
            e.Handled = true;
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTreeView();
        }

        private void LoadRSS()
        {
            RSSReader r = new RSSReader();
            r.Read();

            Run texto = null;
            Hyperlink link = null;
            TextBlock block;

            for (int i = 0; i < r.ListNews.Count && i < 5; i++)
            {
                texto = new Run(String.Format("{0}...", r.ListNews[i].Title.Remove(45)));
                link = new Hyperlink(texto);
                link.NavigateUri = new Uri(r.ListNews[i].Link);
                link.RequestNavigate += Hyperlink_RequestNavigate;

                link.Style = (Style)FindResource("LinkStyle");

                block = new TextBlock();
                block.Style = (Style)FindResource("TBStyle");
                block.Inlines.Add(link);
                panelRSS.Children.Add(block);
            }
        }

        private void LoadTreeView()
        {
            TreeViewItem node;

            foreach (MSNUser user in listUsers)
            {
               node = new TreeViewItem();
               node.Header = String.Format("{0} (usuário {1})",user.UserName, user.UserID);
               treeItemRoot.Items.Add(node);
            }
        }

    }
}
