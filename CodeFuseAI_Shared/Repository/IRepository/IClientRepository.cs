using System;
using CodeFuseAI_Shared.Data;
using CodeFuseAI_Shared.Models;

namespace CodeFuseAI_Shared.Repository.IRepository
{
    public interface IClientRepository
    {
        public Task<ClientDTO> Create(ClientDTO objDTO);

        public Task<ClientDTO> Update(ClientDTO objDTO);

        public Task<int> Delete(int id);

        public Task<ClientDTO> Get(int id);

        public Task<IEnumerable<ClientDTO>> GetAll();

        public Task<ClientFrontendDTO> GetClientFrontendData(int clientId);

    }
}


