using Music.Domain.Entities;

namespace Music.Application.DataTransferObjects;

public class SubscriptionDTO:BaseDTO
{
    public ApplicationUser User { get; set; }
    public bool IsActive { get; set; }
    public DateTime Create { get; set; } = DateTime.UtcNow;
    public DateTime Expired { get; set; }
}