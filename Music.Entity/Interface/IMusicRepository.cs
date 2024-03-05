namespace Music.Domain.Interface;

public interface IMusicRepository
{
    IEnumerable<Entities.Music> GetMusices();

}
