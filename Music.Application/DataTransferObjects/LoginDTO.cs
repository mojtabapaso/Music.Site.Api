using System.ComponentModel.DataAnnotations;

namespace Music.Application.DataTransferObjects;

public class LoginDTO:BaseDTO
{
	[Required]
	public string UserName { get; set; }
	[Required]
	public string Password { get; set; }
}