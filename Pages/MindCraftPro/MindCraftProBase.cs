using Microsoft.AspNetCore.Components;
using CodeFuseAI_Apps.Service.IService;
using CodeFuseAI_Shared.Models;
using CodeFuseAI_Shared.Repository.IRepository;
using MudBlazor;
using CodeFuseAI_Shared.Data;


namespace CodeFuseAI_Apps.Pages.MindCraftPro
{
    public class MindCraftProBase : CustomComponentBase
    {
        [Inject]
        protected IMessageRepository? _messageRepository { get; set; }

        [Inject]
        protected IConversationRepository? _conversationRepository { get; set; }

        [Inject]
        protected IMindCraftProService? _mindCraftProService { get; set; }

        [Inject]
        protected MudBlazor.ISnackbar? _snackBar { get; set; }


        [Parameter]
        public int ConversationId { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await base.OnInitializedAsync();

            if (ClientId != 0)
            {
                await LoadConversations();
                await LoadMessages();
            }
            else
            {
                // Handle the case where ClientId is null or zero (e.g., user is not logged in or there's no client associated with this user)
            }

            PageIsLoading = false;

            if (ConversationId == 0)
            {
                OpenDrawer(Anchor.Bottom);
            }
        }

        protected override async Task OnParametersSetAsync()
        {
            if (ConversationId != 0)
            {
                await LoadConversations();
                await LoadMessages();
                StateHasChanged();
            }
            else
            {
                // Handle the case where ConversationId is null or zero
            }

            PageIsLoading = false;

            if (ConversationId == 0)
            {
                OpenDrawer(Anchor.Bottom);
            }
        }


        #region Mudcard conversationList

        protected IEnumerable<ConversationDTO> Conversations { get; set; } = new List<ConversationDTO>();

        protected bool MessageIsProcessing { get; set; } = false;

        protected void NavigateToConversation(int conversationId)
        {
            _navigationManager.NavigateTo($"/mind-craft-pro/{conversationId}", false);
        }


        #endregion

        #region Mudcard Mudcarousel

        protected List<MessageDTO> ConversationMessages { get; set; } = new List<MessageDTO>();

        protected bool arrows = true;
        protected bool bullets = false;
        protected bool enableSwipeGesture = true;
        protected bool autocycle = false;
        protected Transition transition = Transition.Slide;

        protected bool IsCodeSnippet(string text)
        {
            // This is a simple example, you might need a more complex logic depending on your use case
            return text.Contains(";") || (text.Contains("{") && text.Contains("}")) || (text.Contains("<") && text.Contains(">"));

        }

        protected async Task ToggleFavorite(MessageDTO message)
        {
            var success = await _messageRepository.ToggleFavorite(message.Id);
            if (success)
            {
                message.IsFav = !message.IsFav;
            }
        }

        #endregion

        #region Mudcard Input

        protected MessageFormModel messageFormModel = new MessageFormModel();

        protected string newMessageContent = string.Empty;

        protected bool open;
        protected Anchor anchor;

        protected class MessageFormModel
        {
            public string? NewMessageContent { get; set; }
        }

        protected async Task SendMessage()
        {
            if (!string.IsNullOrWhiteSpace(messageFormModel.NewMessageContent))
            {
                MessageIsProcessing = true;

                var newMessage = new MessageDTO
                {
                    Content = messageFormModel.NewMessageContent,
                    ConversationId = ConversationId,
                    IsUserMessage = true,
                    Timestamp = DateTime.UtcNow
                };

                await _messageRepository.Create(newMessage);
                messageFormModel.NewMessageContent = string.Empty; // Reset the input field

                try
                {
                    // Call the GetAndUpdateContext method after saving the user's message
                    await _conversationRepository.GetAndUpdateContext(ConversationId);

                    var responseMessage = await _mindCraftProService.SendMessageAsync(ConversationId, newMessage.Content);

                    var responseMessageDto = new MessageDTO
                    {
                        Content = responseMessage,
                        ConversationId = ConversationId,
                        IsUserMessage = false,
                        Timestamp = DateTime.UtcNow
                    };

                    await _messageRepository.Create(responseMessageDto);

                    await LoadMessages(); // Refresh the Messages list

                    MessageIsProcessing = false;

                    // Reload the page
                    StateHasChanged();
                }
                catch (HttpRequestException)
                {
                    // Handle the error condition
                    _snackBar.Add("An error occurred. Please try again.", Severity.Error);
                }
            }
        }

        protected async Task LoadMessages()
        {
            var belongsToClient = await _conversationRepository.BelongsToClient(ConversationId, ClientId);

            if (belongsToClient)
            {
                ConversationMessages = (await _messageRepository.GetAllByConversationId(ConversationId)).ToList();
            }
            else
            {
                // Handle the error appropriately - conversation does not belong to the client.
                throw new UnauthorizedAccessException("The client does not have permission to access this conversation.");
            }
        }

        protected void OpenDrawer(Anchor anchor)
        {
            open = true;
            this.anchor = anchor;
        }

        #endregion

        #region Muddrawer 

        protected async Task LoadConversations()
        {
            Conversations = await _conversationRepository.GetAllByClientId(ClientId);
        }

        protected string newConversationName = string.Empty;
        protected int editingConversationId = -1;
        protected string editingConversationName = string.Empty;
        public bool ConversationIsLoading { get; set; } = false;
        protected MudChip[] selected;
        protected string customText;

        protected async Task CreateConversation()
        {
            if (!string.IsNullOrWhiteSpace(newConversationName))
            {
                // Check if conversation with the same name already exists
                if (Conversations.Any(c => c.Name == newConversationName))
                {
                    _snackBar.Add("Conversation with the same name already exists.", Severity.Error);
                    return;
                }

                // Start the spinner
                ConversationIsLoading = true;

                var newConversation = new ConversationDTO
                {
                    Name = newConversationName,
                    ClientId = ClientId
                };

                var createdConversation = await _conversationRepository.Create(newConversation);

                await SendDefaultMessage(createdConversation.Id);

                Conversations = await _conversationRepository.GetAllByClientId(ClientId);

                newConversationName = string.Empty;
                _snackBar.Add("Conversation created successfully.", Severity.Success);

                ConversationIsLoading = false;
            }
            else
            {
                _snackBar.Add("Conversation name cannot be empty.", Severity.Error);
            }
        }

        protected void StartEditingConversation(int conversationId)
        {
            editingConversationId = conversationId;
            var conversation = Conversations.FirstOrDefault(c => c.Id == conversationId);
            if (conversation != null)
            {
                editingConversationName = conversation.Name;
            }
        }

        protected void CancelEditingConversation()
        {
            editingConversationId = -1;
            editingConversationName = string.Empty;
        }

        protected async Task UpdateConversation(int conversationId)
        {
            if (!string.IsNullOrWhiteSpace(editingConversationName))
            {
                var conversation = Conversations.FirstOrDefault(c => c.Id == conversationId);
                if (conversation != null)
                {
                    conversation.Name = editingConversationName;
                    await _conversationRepository.Update(conversation);
                   _snackBar.Add("Conversation Updated successfully.", Severity.Success);
                }
                else
                {
                    _snackBar.Add("Failed to update conversation.", Severity.Error);
                }

                editingConversationId = -1;
                editingConversationName = string.Empty;
            }
        }

        protected async Task DeleteConversation(int conversationId)
        {
            var result = await _conversationRepository.Delete(conversationId);

            if (result > 0) // if conversation was deleted
            {
                Conversations = await _conversationRepository.GetAllByClientId(ClientId);
                _snackBar.Add("Conversation deleted successfully.", Severity.Success);
            }
            else
            {
                _snackBar.Add("Failed to delete conversation.", Severity.Error);
            }
        }

        protected async Task SendDefaultMessage(int conversationId)
        {
            try
            {
                var conversation = await _conversationRepository.Get(conversationId);
                if (conversation == null)
                {
                    Console.WriteLine("Conversation not found.");
                    return;
                }

                var defaultMessage = new MessageDTO
                {
                    Content = $"Hello!, lets Start conversation about '{newConversationName}'.",
                    //Content = "Hello",
                    ConversationId = conversationId,
                    IsUserMessage = true,
                    Timestamp = DateTime.UtcNow
                };

                await _messageRepository.Create(defaultMessage);

                var responseMessage = await _mindCraftProService.SendMessageAsync(conversationId, defaultMessage.Content);
                if (string.IsNullOrEmpty(responseMessage))
                {
                    Console.WriteLine("AI response is null or empty.");
                    return;
                }

                var responseMessageDto = new MessageDTO
                {
                    Content = responseMessage,
                    ConversationId = conversationId,
                    IsUserMessage = false,
                    Timestamp = DateTime.UtcNow
                };

                await _messageRepository.Create(responseMessageDto);

                await LoadMessages(); 
                StateHasChanged();   
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SendDefaultMessage: {ex.Message}");
            }
        }

        protected string GetSystemMessage(int conversationId)
        {
            try
            {
                var conversation = Conversations.FirstOrDefault(c => c.Id == conversationId);
                return conversation?.SystemMessage ?? string.Empty;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred while retrieving system message: {ex.Message}");
                return "An error occurred while retrieving the system message.";
            }
        }

        protected async Task UpdateSystemMessage(int conversationId, string customText)
        {
            var systemMessage = string.Join(", ", selected.Select(chip => chip.Text));

            // Add the customText to the systemMessage if it is not null or empty.
            if (!string.IsNullOrWhiteSpace(customText))
            {
                systemMessage += ", " + customText;
            }

            var conversation = Conversations.FirstOrDefault(c => c.Id == conversationId);
            if (conversation != null)
            {
                conversation.SystemMessage = systemMessage;
                await _conversationRepository.Update(conversation);
                _snackBar.Add("System message updated successfully.", Severity.Success);
            }
            else
            {
                _snackBar.Add("Failed to update system message.", Severity.Error);
            }
        }

        #endregion

    }
}

