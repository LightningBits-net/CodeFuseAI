using System;
namespace CodeFuseAI.Service.IService
{
    public interface IOpenAiApiService
    {
        Task<string> SendMessageAsync(int conversationId, string prompt);
    }

}

