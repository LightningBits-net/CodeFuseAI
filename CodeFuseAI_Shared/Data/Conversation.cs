using System;
using CodeFuseAI_Shared.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using CodeFuseAI_Shared.Models;

namespace CodeFuseAI_Shared.Data
{
    public class Conversation
    {
        public int Id { get; set; }
        public int ClientId { get; set; }
        public string Name { get; set; }
        public string? Context { get; set; }
        public string? SystemMessage { get; set; }

        [ForeignKey("ClientId")]
        public Client Client { get; set; } 

        public virtual ICollection<Message> Messages { get; set; }
    }
}

