using System;
using System.Text;
using System.Net.Sockets;
using System.IO;

namespace WPFMessenger.Core
{

    public delegate bool TCPConnectionCaller();

    class TCPConnection
    {

        private User user;

        public User User
        {
            get { return user; }
            set { user = value; }
        }

        private string serverURL = "larc.inf.furb.br";

        private string getUsrString = " GET USRS ";

        public bool Connect()
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
                Console.WriteLine("Conectando.....");

                tcpclnt.Connect(serverURL, 1012);

                Console.WriteLine("Conectado!");

                Stream stm = tcpclnt.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();
                byte[] ba = asen.GetBytes(String.Format("{0} {1}:{2}", this.getUsrString, user.IdUsuario, user.SenhaUsuario));

                Console.WriteLine("Transmitindo.....");

                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                for (int i = 0; i < k; i++)
                    Console.Write(Convert.ToChar(bb[i]));

                tcpclnt.Close();

                return true;
            }

            catch (Exception erro)
            {
                Console.WriteLine("Erro: " + erro.StackTrace);
                return false;
            }

        }

    }
}
