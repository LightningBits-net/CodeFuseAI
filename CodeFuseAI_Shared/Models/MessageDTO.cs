using System;
using CodeFuseAI_Shared.Data;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

    namespace CodeFuseAI_Shared.Models
    {
        public class MessageDTO
        {
            public int Id { get; set; }
            public string Content { get; set; }
            public DateTime Timestamp { get; set; }
            public bool IsUserMessage { get; set; }
            public int ConversationId { get; set; }
            public bool IsFav { get; set; }

            public virtual Conversation Conversation { get; set; }
        }
    }

