namespace Music.Application.DataTransferObjects;

public class CategoryDTO : BaseDTO
{
	public string Title { get; set; }
	public MusicDTO Music { get; set; }
}