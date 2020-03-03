using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;
using util.BRLight;

namespace SINJ_MetaMiner.AD
{
    public class db
    {
        private IDbConnection _dbcon;

        public db()
        {
            _dbcon = new SqlConnection(Config.ValorChave("StringConnectionLexml"));
        }

        public void openConnetion()
        {
            Console.SetCursorPosition(0, 20);
            Console.WriteLine("ConnectionState: " + _dbcon.State);
            if (_dbcon.State == ConnectionState.Closed || _dbcon.State == ConnectionState.Broken)
            {
                closeConnection();
                _dbcon.Open();
            }
        }

        public IDbConnection getConnection()
        {
            openConnetion();
            return _dbcon;
        }

        public void closeConnection()
        {
            if (_dbcon.State != ConnectionState.Closed)
            {
                _dbcon.Close();
            }
        }

    }
}
