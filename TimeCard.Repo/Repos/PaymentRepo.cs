using Dapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TimeCard.Domain;

namespace TimeCard.Repo.Repos
{
    public class PaymentRepo : BaseRepo
    {
        public PaymentRepo(string connectionString) : base(connectionString)
        {
        }
        public IEnumerable<PaymentSummary> GetSummary(int contractorId)
        {
            using (var conn = GetOpenConnection())
            {
                return conn.Query<PaymentSummary>("sPaymentSummary", new { contractorId }, null, true, null, System.Data.CommandType.StoredProcedure);
            }
        }

    }
}
