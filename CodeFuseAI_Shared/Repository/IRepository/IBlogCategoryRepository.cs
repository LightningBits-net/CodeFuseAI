// LightningBits
using System;
using CodeFuseAI_Shared.Data;
using CodeFuseAI_Shared.Models;

namespace CodeFuseAI_Shared.Repository.IRepository
{
	public interface IBlogCategoryRepository
	{
        public Task<BlogCategoryDTO> Create(BlogCategoryDTO objDTO);

        public Task<BlogCategoryDTO> Update(BlogCategoryDTO objDTO);

        public Task<int> Delete(int id);

        public Task<BlogCategoryDTO> Get(int id);

        public Task<IEnumerable<BlogCategoryDTO>> GetAll();
    }
}

