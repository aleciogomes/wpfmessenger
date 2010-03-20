using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;

namespace WPFMessenger.Core
{

    public delegate bool TCPConnectionCaller();

    class TCPConnection
    {
        /*USUARIOS E SENHAS
         * 3299 / 12qnmn
         * 7237 / ht7mxh
         * 
         */
        private static string errorMsg = "Erro: Id inválido: ";
        private string returnString;
        private MSNUser user;

        public MSNUser User
        {
            get { return user; }
            set { user = value; }
        }

        private string serverURL = "larc.inf.furb.br";

        private string getUsrString = "GET USRS ";

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
                string command = String.Format("{0}{1}:{2}", this.getUsrString, user.UserID, user.UserPassword);

                byte[] ba = asen.GetBytes(command);

                Console.WriteLine("Transmitindo.....");

                stm.Write(ba, 0, ba.Length);

                byte[] bb = new byte[100];
                int k = stm.Read(bb, 0, 100);

                StringBuilder retorno = new StringBuilder();

                for (int i = 0; i < k; i++)
                {
                    retorno.Append(Convert.ToChar(bb[i]).ToString());
                }

                tcpclnt.Close();

                //Erro: Id inválido: GET USRS  111:teste123

                returnString = retorno.ToString();

                if (returnString.Equals(String.Format("{0}{1}", errorMsg, command)))
                {
                    return false;
                }

                return true;
            }

            catch (Exception erro)
            {
                Console.WriteLine("Erro: " + erro.StackTrace);
                return false;
            }

        }

        public IList<MSNUser> GetListUsers()
        {

            IList<MSNUser> list = new List<MSNUser>();

            if (!String.IsNullOrEmpty(returnString))
            {
                string[] returnVector = returnString.Split(new char[] { ':' });
                string value = "";

                MSNUser user = null;

                for (int i = 0; i < returnVector.Length; i++)
                {
                    value = returnVector[i].ToString();

                    if (!String.IsNullOrEmpty(value))
                    {
                        if (i % 2 == 0)
                        {
                            user = new MSNUser();
                            user.UserID = Int32.Parse(value);
                        }
                        else
                        {
                            user.UserName = value;
                            list.Add(user);
                        }
                    }
                }
            }

            return list;

        }

    }
}
