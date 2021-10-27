using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace It_tools
{
    abstract class ConnString
    {
        protected  MySqlConnection connectTomysql = new MySqlConnection();
        protected string connectionParametr = null;
        protected  void SetConnStrForMysql()
        {
            connectionParametr = ConfigurationManager.ConnectionStrings["muzarimConnectionString"].ToString();
            connectTomysql.ConnectionString = (connectionParametr);
        }
    }
}
