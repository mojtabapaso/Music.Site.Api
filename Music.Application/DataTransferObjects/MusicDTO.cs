using Music.Domain.Entities;

namespace Music.Application.DataTransferObjects;

public class MusicDTO : BaseDTO
{
	public string Subtitle { get; set; }
	public string Description { get; set; }
	public string UrlDownload { get; set; }
	public SingerDTO Singer { get; set; }
	public string TypeMusicId { get; set; }
	public bool NeedSubscription { get; set; }
	public ICollection<TypeMusicDTO> TypeMusics { get; set; }
	public virtual string CategoryId { get; set; }
	public virtual ICollection<CategoryDTO> MusicCategories { get; set; }
}

