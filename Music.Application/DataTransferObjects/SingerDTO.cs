namespace Music.Application.DataTransferObjects;

public class SingerDTO : BaseDTO
{
	public string Name { get; set; }
	public ICollection<MusicDTO> Musics { get; set; }
}