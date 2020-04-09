using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TimeCard.Repo
{
    public class BaseRepo
    {
        protected readonly string ConnectionString;
        public BaseRepo(string connectionString)
        {
            ConnectionString = connectionString;
        }

        protected SqlConnection GetOpenConnection()
        {
            var conn = new SqlConnection(ConnectionString);
            if (conn.State != System.Data.ConnectionState.Open)
            {
                conn.Open();
            }
            return conn;
        }
    }
}
