using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WPFMessenger.Core
{
    public class MSNUser
    {
        private string nomeUsuario;
        private string senhaUsuario;
        private int idUsuario;

        public string NomeUsuario
        {
            get { return nomeUsuario; }
            set { nomeUsuario = value; }
        }

        public string SenhaUsuario
        {
            get { return senhaUsuario; }
            set { senhaUsuario = value; }
        }

        public int IdUsuario
        {
            get { return idUsuario; }
            set { idUsuario = value; }
        }

    }
}
