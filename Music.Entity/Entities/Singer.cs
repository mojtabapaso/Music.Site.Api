using Music.Domain.Common;

namespace Music.Domain.Entities;

public class Singer : BaseEntity
{
    public ICollection<Music> Musics { get; set; } = new List<Music>();
}
// Music.Infrastructure
public class TypeMusic : BaseEntity
{
    public string Name { get; set; }
    public ICollection<Music> Musics { get; set; } = new List<Music>();
}
public class Music : BaseEntity
{
    public string Subtitle { get; set; }
    public string Description { get; set; }
    public string UrlDownload { get; set; }
    public Singer Singer { get; set; }
    public ICollection<TypeMusic> TypeMusics { get; set; } = new List<TypeMusic>();
    public string CategoryId { get; set; }
    public ICollection<Category> MusicCategories { get; set; } = new List<Category>();
}
public class Category : BaseEntity
{
    public string Title { get; set; }
    public string MusicId { get; set; }
    public Music Music { get; set; }
}
