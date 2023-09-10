using System;
namespace CodeFuseAI_Apps.Service.IService
{
    public interface IOpenAiApiService
    {
        Task<string> SendMessageAsync(int conversationId, string prompt);
    }

}

