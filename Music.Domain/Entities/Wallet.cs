using Music.Domain.Common;

namespace Music.Domain.Entities;

public class Wallet:BaseEntity
{
    public string IdUser { get; set; }
    public ApplicationUser User { get; set; }
    public UInt32 Amount { get; set; }
}