using System;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace CodeFuseAI_Shared.Data
{
	public class ApplicationUser : IdentityUser
	{
		public string Name { get; set; }
	}
}

