public class Claim
{
    public int ClaimID { get; set; }
    public int LecturerID { get; set; }
    public decimal HoursWorked { get; set; }
    public decimal HourlyRate { get; set; }
    public string Status { get; set; }

    public virtual Lecturer Lecturer { get; set; }
}
