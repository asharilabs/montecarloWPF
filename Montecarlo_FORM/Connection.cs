using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;

namespace Montecarlo_FORM
{
    class Connection
    {
        public static string host = "localhost";
        public static string db = "db_simulasi";
        public static string user = "root";
        public static string pass = "";

        public static MySqlConnection InitConnection()
        {
            return new MySqlConnection("server=" + host +
                ";database=" + db +
                ";uid=" + user +
                ";pwd=" + pass);
        }
    }
}
