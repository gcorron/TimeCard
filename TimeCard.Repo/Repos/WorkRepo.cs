using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using TimeCard.Domain;

namespace TimeCard.Repo.Repos
{
    public class WorkRepo : BaseRepo   
    {
        public WorkRepo(string connectionString) : base(connectionString)
        {
        }

        public IEnumerable<Work>GetWork(int contractorId, decimal workDay, bool payCycle)
        {
            using (var conn = GetOpenConnection())
            {
                return conn.Query<Work>("sWork", new { contractorId, workDay, payCycle }, null, true, null, System.Data.CommandType.StoredProcedure);
            }
        }

        public void SaveWork(Work work)
        {
            using (var conn = GetOpenConnection())
            {
                conn.Execute("uWork", new { work.WorkId, work.ContractorId, work.JobId, work.WorkDay, work.Descr, work.Hours, work.WorkType }, null, null, System.Data.CommandType.StoredProcedure);
            }
        }

        public void DeleteWork(int workId)
        {
            using (var conn = GetOpenConnection())
            {
                conn.Execute("dWork", new { workId }, null, null, System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<WorkExtended> GetWorkExtended(int contractorId, decimal workDay, bool payCycle, int payCycles)
        {
            using (var conn = GetOpenConnection())
            {
                return conn.Query<WorkExtended>("sWorkExtended", new { contractorId, workDay, payCycle, payCycles }, null, true, null, System.Data.CommandType.StoredProcedure);
            }
        }

        public IEnumerable<Lookup> GetJobs(string addFirstRow = null)
        {
            using (var conn = GetOpenConnection())
            {
                var data = conn.Query<Lookup>("sJob", null, null, true, null, System.Data.CommandType.StoredProcedure);
                if (addFirstRow != null)
                {
                    return new Lookup[] { new Lookup { Id = 0, Descr = addFirstRow } }.Union(data);
                }
                else
                {
                    return data;
                }
            }
        }
    }
}
