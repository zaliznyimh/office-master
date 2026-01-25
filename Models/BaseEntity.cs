namespace OfficeMaster.Models;

public abstract class BaseEntity
{
    public long Id { get; set; }
    public DateTime CreatedOn { get; set; } = DateTime.Now;
    public DateTime UpdatedOn { get; set; } = DateTime.Now;
}