using Music.Domain.Common;

namespace Music.Domain.Entities;

// Music.Infrastructure
public class TypeMusic : BaseEntity
{
    public string Name { get; set; }

    public ICollection<MusicEntity> Musics { get; set; } = new List<MusicEntity>();
}
