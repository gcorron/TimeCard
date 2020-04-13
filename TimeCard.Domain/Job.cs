using System;
namespace TimeCard.Domain
{
    public class Job
    {
        public int JobId { get; set; }
        public int ClientId { get; set; }
        public int ProjectId { get; set; }
        public int BillType { get; set; }
        public string BillTypeDescr { get; set; }
        public string Client { get; set; }
        public string Project { get; set; }
        public bool Active { get; set; }
    }
}
