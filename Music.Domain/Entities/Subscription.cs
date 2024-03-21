using Music.Domain.Common;

namespace Music.Domain.Entities;

public class Subscription : BaseEntity
{
	public ApplicationUser User { get; set; }
	public bool IsActive { get; set; }
	public DateTime Create { get; set; } = DateTime.UtcNow;
	public DateTime Expired { get; set; }
}

