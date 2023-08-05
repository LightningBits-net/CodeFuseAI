// LightningBits
using System;
using CodeFuseAI_Shared.Data;
using CodeFuseAI_Shared.Models;

namespace CodeFuseAI_Shared.Repository.IRepository
{
        public interface IConversationRepository
        {
            Task<bool> BelongsToClient(int conversationId, int clientId);
            Task<ConversationDTO> Get(int id);
            Task<IEnumerable<ConversationDTO>> GetAll();
            Task<int> Delete(int id);
            Task<ConversationDTO> Create(ConversationDTO objDTO);
            Task<ConversationDTO> Update(ConversationDTO objDTO);

            Task<ConversationDTO> GetByName(string name);
            Task<IEnumerable<ConversationDTO>> GetAllByClientId(int clientId);
            Task<ConversationDTO> GetAndUpdateContext(int id);

    }
}

