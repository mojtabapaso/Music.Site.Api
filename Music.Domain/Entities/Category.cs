using Music.Domain.Common;

namespace Music.Domain.Entities;

public class Category : BaseEntity
{
    public string Title { get; set; }
    public string MusicId { get; set; }
    public MusicEntity Music { get; set; }
}
