using System;
namespace CodeFuseAI_Apps.Service.IService
{
	public interface IMindCraftProService
    {
        Task<string> SendMessageAsync(int conversationId, string prompt);
    }
}

