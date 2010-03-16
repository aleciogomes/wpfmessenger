using System;
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

        private void idUsuario_GotFocus(object sender, RoutedEventArgs e)
        {
            idUsuario.Clear();
            idUsuario.Foreground = new SolidColorBrush(Colors.Black);
            idUsuario.GotFocus -= idUsuario_GotFocus;
        }

        private void senhaUsuario_GotFocus(object sender, RoutedEventArgs e)
        {
            senhaUsuario.Clear();
            senhaUsuario.Foreground = new SolidColorBrush(Colors.Black);
            senhaUsuario.GotFocus -= senhaUsuario_GotFocus;
        }

        private void btLogin_Click(object sender, RoutedEventArgs e)
        {
            lblError.Visibility = Visibility.Hidden;
            idUsuario.Focus();
            senhaUsuario.Focus();

            if (String.IsNullOrEmpty(senhaUsuario.Text.ToString())|| String.IsNullOrEmpty(idUsuario.Text.ToString()))
            {
                senhaUsuario.MoveFocus(new TraversalRequest(FocusNavigationDirection.Next));
                MessageBox.Show("Informe seu ID e sua Senha.");
                return;
            }

            int idUsuarioValor;

            if (!int.TryParse(idUsuario.Text.ToString(), out idUsuarioValor))
            {
                MessageBox.Show("O valor informado no campo ID deve ser númerico.");
                return;
            }

            user.IdUsuario = idUsuarioValor;
            user.SenhaUsuario = senhaUsuario.Text.ToString();

            btLogin.IsEnabled = false;
            idUsuario.IsEnabled = false;
            senhaUsuario.IsEnabled = false;

            Duration duration = new Duration(TimeSpan.FromSeconds(1));
            DoubleAnimation doubleanimation = new DoubleAnimation(100.0, duration);
            doubleanimation.RepeatBehavior = RepeatBehavior.Forever;

            loginBar.Visibility = Visibility.Visible;
            loginBar.BeginAnimation(ProgressBar.ValueProperty, doubleanimation);

            BackgroundWorker bw = new BackgroundWorker();
            bw.DoWork += ValidateConnect;
            bw.RunWorkerCompleted += GetValidationConnect;
            bw.RunWorkerAsync();
        }

        private void ValidateConnect(object sender, DoWorkEventArgs e)
        {

            e.Result = tcp.Connect();
            //e.Result = true;

        }


        private void GetValidationConnect(object sender, RunWorkerCompletedEventArgs e)
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
                idUsuario.IsEnabled = true;
                senhaUsuario.IsEnabled = true;
                lblError.Visibility = Visibility.Visible;
                loginBar.Visibility = Visibility.Hidden;
                loginBar.BeginAnimation(ProgressBar.ValueProperty, null);
            }

        }

    }
}
