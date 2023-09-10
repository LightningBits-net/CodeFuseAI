using System;



namespace CodeFuseAI_Apps.Service.IService
{
    public interface IClientService
    {
        Task<int> GetClientIdAsync();
    }
}

