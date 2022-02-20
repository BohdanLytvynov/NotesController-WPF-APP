using System;
using System.Collections.Generic;
using System.Text;
using System.Security;


namespace Models
{
    public class User
    {
        private string m_login;

        private int[] m_pass;//user password

        public string Login { get => m_login; set => m_login = value; }

        public int[] Pass { get => m_pass; set => m_pass = value; }        
    }
}
