// LightningBits
using System;
using CodeFuseAI_Shared.Data;
using CodeFuseAI_Shared.Models;

namespace CodeFuseAI_Shared.Repository.IRepository
{
    public interface IMessageRepository
    {
        Task<IEnumerable<MessageDTO>> GetAllByConversationId(int conversationId);
        //Task<IEnumerable<object>> GetMessagesForApiRequest(int conversationId);
        Task<MessageDTO> Create(MessageDTO objDTO);
        Task<MessageDTO> Update(MessageDTO objDTO);
        Task<int> Delete(int id);
        Task<bool> ToggleFavorite(int id);
        //Task PurgeMessages(int conversationId);
        //Task DeleteMessagesWithDefaultResponseAndPrompts(int conversationId);
    }
}

