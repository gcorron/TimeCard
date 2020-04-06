using System;

public class Work
{
    public const DateTime BaselineDate=12/22/2018;
    public int workId { get; set; }
    public int contractorId { get; set; }
    public int jobId { get; set; }
    public decimal workDay { get; set; }
    public string descr { get; set; }
    public decimal hours { get; set; }
    public string workDate
    {
        get
        {
            int cycle = Decimal.Floor(workDay);
            return BaselineDate.AddDays(cycle * 14 + (workDay - cycle) * 100);
        }
    }
}
