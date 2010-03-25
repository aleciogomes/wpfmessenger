﻿using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using System.Windows.Media.Animation;
using System.Threading;
using WPFMessenger.Core;
using System.ComponentModel;
using System.Windows.Controls.Primitives;


namespace WPFMessenger
{

    public partial class LoginWindow : Window
    {
        private MSNUser user;
        private TCPConnection tcp;

        public LoginWindow()
        {
            user = new MSNUser();
            tcp = new TCPConnection();

            tcp.User = user;

            InitializeComponent();
        }

        private void userID_GotFocus(object sender, RoutedEventArgs e)
        {
            userID.Clear();
            userID.Foreground = new SolidColorBrush(Colors.Black);
            userID.GotFocus -= userID_GotFocus;

            userID.Text = "7237";
        }

        private void userPassword_GotFocus(object sender, RoutedEventArgs e)
        {
            userPassword.Clear();
            userPassword.Foreground = new SolidColorBrush(Colors.Black);
            userPassword.GotFocus -= userPassword_GotFocus;

            userPassword.Text = "ht7mxh";
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
 
           lblError.Visibility = Visibility.Hidden;
           userID.Focus();
           userPassword.Focus();

           if (String.IsNullOrEmpty(userPassword.Text.ToString())|| String.IsNullOrEmpty(userID.Text.ToString()))
           {
               userPassword.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
               MessageBox.Show("Informe seu ID e sua Senha.");
               return;
           }

           int userIDValor;

           if (!int.TryParse(userID.Text.ToString(), out userIDValor))
           {
               MessageBox.Show("O valor informado no campo ID deve ser númerico.");
               return;
           }

           user.UserID = userIDValor;
           user.UserPassword = userPassword.Text.ToString();

           btLogin.IsEnabled = false;
           userID.IsEnabled = false;
           userPassword.IsEnabled = false;

           Duration duration = new Duration(TimeSpan.FromSeconds(1));
           DoubleAnimation doubleanimation = new DoubleAnimation(100.0, duration);
           doubleanimation.RepeatBehavior = RepeatBehavior.Forever;

           loginBar.Visibility = Visibility.Visible;
           loginBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);

           BackgroundWorker bw = new BackgroundWorker();
           bw.DoWork += ValidateConnect;
           bw.RunWorkerCompleted += GetConnectionValidation;
           bw.RunWorkerAsync();

        }

        private void ValidateConnect(object sender, DoWorkEventArgs e)
        {

            //e.Result = tcp.Connect();
            e.Result = true;

        }


        private void GetConnectionValidation(object sender, RunWorkerCompletedEventArgs e)
        {

            bool connected = Boolean.Parse(e.Result.ToString());

            if (connected)
            {
                MainWindow main = new MainWindow();
                main.ListUsers = tcp.GetListUsers();
                main.Show();

                this.Close();
            }
            else
            {
                btLogin.IsEnabled = true;
                userID.IsEnabled = true;
                userPassword.IsEnabled = true;
                lblError.Visibility = Visibility.Visible;
                loginBar.Visibility = Visibility.Hidden;
                loginBar.BeginAnimation(ProgressBar.ValueProperty, null);
            }

        }

    }
}
