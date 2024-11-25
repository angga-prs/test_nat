using System.Collections.Generic;

public class EmployeeReview
{
    public string EmployeeName { get; set; }
    public string Position { get; set; }
    public List<Criterion> Criteria { get; set; }
    public double TotalScore { get; set; }
    public string GeneralFeedback { get; set; }
}

public class Criterion
{
    public string Name { get; set; }
    public int Score { get; set; }
    public string Feedback { get; set; }
}
