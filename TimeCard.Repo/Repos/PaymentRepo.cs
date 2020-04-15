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
                return QuerySp<PaymentSummary>("sPaymentSummary", new { contractorId });
        }

        public IEnumerable<Payment> GetPayments(int contractorId, int jobId)
        {
            return QuerySp<Payment>("sPayment", new { contractorId, jobId });
        }

        public void SavePayment(Payment payment)
        {
            ExecuteSp("uPayment", new { payment.PayId, payment.ContractorId, payment.JobId, payment.Hours, payment.PayDate, payment.CheckNo, payment.WorkDay }) ;
        }

        public void DeletePayment(int payId)
        {
            ExecuteSp("dPayment", new { payId });
        }

        public bool JobIsTimeCard(int jobId)
        {
            return QuerySingleSp<bool>("sJobIsTimeCard", new { jobId });
        }

        public IEnumerable<TimeCardUnpaid> GetJobTimeCardUnpaidCycles(int contractorId, int jobId)
        {
            return QuerySp<TimeCardUnpaid>("sJobTimeCardUnpaidCycles",new { contractorId, jobId });
        }
    }
}
