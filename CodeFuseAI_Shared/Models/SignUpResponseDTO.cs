using System;
namespace CodeFuseAI_Shared.Models
{
	public class SignUpResponseDTO
	{
        public bool IsRegisterationSuccessful { get; set; }
        public IEnumerable<string> Errors { get; set; }
    }
}

