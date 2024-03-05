using Music.Domain.Common;

namespace Music.Domain.Entities;

public class Singer : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Music> Musics { get; set; } = new List<Music>();
}
