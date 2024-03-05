using Music.Domain.Common;

namespace Music.Domain.Entities;

// Music.Infrastructure
public class TypeMusic : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Music> Musics { get; set; } = new List<Music>();
}
