using System;

public class Job    
{
    public int jobId { get; set; }
    public int clientId { get; set; }
    public int projectId { get; set; }
    public int billType { get; set; }
    public int workType { get; set; }
    public string billTypeDescr { get; set; }
    public string workTypeDescr { get; set; }
    public string client { get; set; }
    public string project { get; set; }
    public bool active { get; set; }
}
