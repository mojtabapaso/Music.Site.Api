using Music.Domain.Common;

namespace Music.Domain.Entities;

public class MusicEntity : BaseEntity
{
    public string Subtitle { get; set; }
    public string Description { get; set; }
    public string UrlDownload { get; set; }
    public Singer Singer { get; set; }
    public ICollection<TypeMusic> TypeMusics { get; set; } = new List<TypeMusic>();
    public string CategoryId { get; set; }
    public ICollection<Category> MusicCategories { get; set; } = new List<Category>();
}
