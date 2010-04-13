using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Net;

namespace WPFMessenger.Core
{

    public delegate bool TCPConnectionCaller();

    public static class TCPConnection
    {

        private static int port = 1012;

        private static string errorMsg = "Erro: Id inválido: ";
        private static string serverURL = "larc.inf.furb.br";
        private static string getUsrString = "GET USRS ";

        public static IList<MSNUser> GetListUsers()
        {

            IList<MSNUser> list = new List<MSNUser>();
            MSNUser user = null;

            string returnString = GetUsers();

            if (ValidetConnect(returnString) && !returnString.Equals("0::"))
            {
                string[] returnVector = returnString.Split(new char[] { ':' });
                string value = null;

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

        public static bool Connect()
        {

            IList<MSNUser> lista = GetListUsers();

            if (lista.Count > 0)
            {

                foreach (MSNUser user in lista)
                {
                    if (user.UserID == MSNSession.User.UserID)
                    {
                        MSNSession.User.UserName = user.UserName;
                        return true;
                    }
                }

                return false;
            }
            else
            {

                return false;
            }
        }

        private static bool ValidetConnect(string returnString)
        {
            if (!String.IsNullOrEmpty(returnString) && !returnString.Equals(errorMsg))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        private static String GetUsers()
        {
            string cmd = String.Format("{0}{1}:{2}", getUsrString, MSNSession.User.UserID, MSNSession.User.UserPassword);
            return StabilishConnection(cmd);
        }

        public static IList<MSNMessage> GetMyMessages()
        {
            string cmd = String.Format("{0}{1}:{2}", "GET MSG ", MSNSession.User.UserID, MSNSession.User.UserPassword);

            string messageString = StabilishConnection(cmd);
            string value = null;

            IList<MSNMessage> lista = new List<MSNMessage>();
            MSNMessage message = null;

            while (!messageString.Equals("0:"))
            {
                string[] returnVector = messageString.Split(new char[] { ':' });

                message = new MSNMessage();

                for (int i = 0; i < returnVector.Length; i++)
                {
                    value = returnVector[i].ToString();

                    if (!String.IsNullOrEmpty(value))
                    {
                        if (i % 2 != 0)
                        {
                            message.Message = value;
                        }
                        else
                        {
                            message.Forwarder = Int32.Parse(value); ;
                        }
                    }

                }

                lista.Add(message);
                messageString = StabilishConnection(cmd);

            }

            return lista;
        }

        private static String StabilishConnection(String command)
        {
            try
            {
                TcpClient tcpclnt = new TcpClient();
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

        private static String ConvertMessage(Stream stream)
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
