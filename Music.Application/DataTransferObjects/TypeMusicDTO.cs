namespace Music.Application.DataTransferObjects;

public class TypeMusicDTO:BaseDTO
{
	public string Name { get; set; }
	public ICollection<MusicDTO> Musics { get; set; }
}