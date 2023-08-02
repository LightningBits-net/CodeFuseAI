// LightningBits
using System;
using CodeFuseAI_Shared.Data;
using CodeFuseAI_Shared.Models;

namespace CodeFuseAI_Shared.Repository.IRepository
{
    public interface IProductRepository
    {
        public Task<ProductDTO> Create(ProductDTO objDTO);

        public Task<ProductDTO> Update(ProductDTO objDTO);

        public Task<int> Delete(int id);

        public Task<ProductDTO> Get(int id);

        public Task<IEnumerable<ProductDTO>> GetAll();

    }
}

