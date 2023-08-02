// LightningBits
using System;
using System.ComponentModel.DataAnnotations;

namespace CodeFuseAI_Shared.Models
{
    public class CategoryDTO
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Please enter name")]
        public string Name { get; set; }

    }
}

