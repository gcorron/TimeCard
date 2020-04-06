using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TimeCard.Domain;

namespace TimeCard.Repo
{
    public class WorkRepo : BaseRepo   
    {
        public WorkRepo(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Work>GetWork(int contractorId, decimal workDay, bool payCycle)
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                OpenConnection(conn);
                return conn.Query<Work>("sWork", new { contractorId, workDay, payCycle }, null, true, null, System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<LookupShort> GetJobs()
        {
            using (var conn = new SqlConnection(ConnectionString))
            {
                OpenConnection(conn);
                return conn.Query<LookupShort>("sJobs", null, null, true, null, System.Data.CommandType.StoredProcedure);
            }
        }
    }
}
