using System.ComponentModel.DataAnnotations;

namespace Music.Application.DataTransferObjects;

public class RegisterDTO
{
	[Required]
	public string UserName { get; set; }

	[Required]
	public string Password { get; set; }
}
