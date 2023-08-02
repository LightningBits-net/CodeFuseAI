using System;



namespace CodeFuseAI.Service.IService
{
    public interface IClientService
    {
        Task<int> GetClientIdAsync();
    }
}

