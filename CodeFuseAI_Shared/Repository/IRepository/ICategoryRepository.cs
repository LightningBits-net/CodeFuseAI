// LightningBits
using System;
using CodeFuseAI_Shared.Data;
using CodeFuseAI_Shared.Models;

namespace CodeFuseAI_Shared.Repository.IRepository
{
    public interface ICategoryRepository
    {
        public Task<CategoryDTO> Create(CategoryDTO objDTO);

        public Task<CategoryDTO> Update(CategoryDTO objDTO);

        public Task<int> Delete(int id);

        public Task<CategoryDTO> Get(int id);

        public Task<IEnumerable<CategoryDTO>> GetAll();

    }
}

