﻿using System.Collections.Generic;
using System.Windows;
using WPFMessenger.Core;
using System.Windows.Controls;
using System;

namespace WPFMessenger
{
    public partial class MainWindow : Window
    {

        private IList<MSNUser> listUsers;

        internal IList<MSNUser> ListUsers
        {
            get { return listUsers; }
            set { listUsers = value; }
        }

        public MainWindow()
        {
            InitializeComponent();

            RSSReader r = new RSSReader();
            r.Read();

            ListBoxItem item = null;
            foreach (RSSNews news in r.ListNews)
            {
                item = new ListBoxItem();
                item.Content = news.Title;
                listBox.Items.Add(item);
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            LoadTreeView();
        }

        private void LoadTreeView()
        {
            TreeViewItem node;

            foreach (MSNUser user in listUsers)
            {
               node = new TreeViewItem();
               node.Header = String.Format("{0} ({1})",user.NomeUsuario, user.IdUsuario);
               treeItemRoot.Items.Add(node);
            }
        }

    }
}
