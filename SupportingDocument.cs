public class SupportingDocument
{
    public int DocumentID { get; set; }
    public int ClaimID { get; set; }
    public string FilePath { get; set; }

    public virtual Claim Claim { get; set; }
}
