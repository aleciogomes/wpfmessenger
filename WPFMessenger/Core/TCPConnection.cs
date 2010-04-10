using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Net;

namespace WPFMessenger.Core
{

    public delegate bool TCPConnectionCaller();

    public class TCPConnection
    {
        /*USUARIOS E SENHAS
         * 3299 / 12qnmn
         * 7237 / ht7mxh
         * 
         */
        private string returnString;

        private int port = 1012;

        private string errorMsg = "Erro: Id inválido: ";
        private string serverURL = "larc.inf.furb.br";
        private string getUsrString = "GET USRS ";

        private TcpClient tcpclnt;

        public TCPConnection()
        {
            tcpclnt = new TcpClient();
        }

        public IList<MSNUser> GetListUsers()
        {

            IList<MSNUser> list = new List<MSNUser>();
            MSNUser user = null;

            if (!String.IsNullOrEmpty(returnString))
            {
                string[] returnVector = returnString.Split(new char[] { ':' });
                string value = "";

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

            /*
            user = new MSNUser();
            user.UserID = 45;
            user.UserName = "Teste";
            list.Add(user);
            */

            return list;

        }

        public bool Connect()
        {
            string command = String.Format("{0}{1}:{2}", this.getUsrString, MSNSession.User.UserID, MSNSession.User.UserPassword);
            returnString = StabilishConnection(command);

            if (returnString.Equals(String.Format("{0}{1}", errorMsg, command)))
            {
                return false;
            }
            else
            {
                return true;
            }

        }

        public String GetMyMessages()
        {
            string command = String.Format("{0}{1}:{2}", "GET MSG ", MSNSession.User.UserID, MSNSession.User.UserPassword);
            return StabilishConnection(command);
        }

        private String StabilishConnection(String command)
        {
            try
            {
                //Console.WriteLine("Conectando.....");
                tcpclnt.Connect(serverURL, port);
                //Console.WriteLine("Conectado!");
                Stream stm = tcpclnt.GetStream();

                ASCIIEncoding asen = new ASCIIEncoding();

                byte[] ba = asen.GetBytes(command);

                //Console.WriteLine("Transmitindo.....");

                stm.Write(ba, 0, ba.Length);

                string message = ConvertMessage(stm);

                tcpclnt.Close();

                return message;
            }
            catch (Exception erro)
            {
                Console.WriteLine("Erro: " + erro.StackTrace);
                return errorMsg;
            }
        }

        private String ConvertMessage(Stream stream)
        {
            byte[] bb = new byte[1000];
            int index = stream.Read(bb, 0, 100);

            StringBuilder message = new StringBuilder();

            for (int i = 0; i < index; i++)
            {
                message.Append(Convert.ToChar(bb[i]).ToString());
            }

            return message.ToString();
        }

    }
}
