// LightningBits
using System;
using CodeFuseAI_Shared.Data;
using CodeFuseAI_Shared.Models;

namespace CodeFuseAI_Shared.Repository.IRepository
{
    public interface IToDoItemRepository
    {
        public Task<ToDoItemDTO> Create(ToDoItemDTO objDTO);

        public Task<ToDoItemDTO> Update(ToDoItemDTO objDTO);

        public Task<int> Delete(int id);

        public Task<ToDoItemDTO> Get(int id);

        public Task<IEnumerable<ToDoItemDTO>> GetAll();
    }
}

