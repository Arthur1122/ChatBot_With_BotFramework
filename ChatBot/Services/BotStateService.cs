using ChatBot.Models;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ChatBot.Services
{
    public class BotStateService
    {
        #region Variables
        // State Variables
        public UserState UserState { get; }
        public ConversationState ConversationState { get; }

        // IDs
        public static string UserProfileId { get; } = $"{nameof(BotStateService)}.UserProfile";
        public static string ConversationDataId { get; } = $"{nameof(BotStateService)}.ConversationData";
        public static string DialogStateId { get; } = $"{nameof(BotStateService)}.DialogState";

        // Accessors 
        public IStatePropertyAccessor<UserProfile> UserProfileAccessor { get; set; }

        public IStatePropertyAccessor<ConversationData> ConversationDataAccessor { get; set; }
        public IStatePropertyAccessor<DialogState> DialogStateAccessor { get; set; }
        #endregion

        public BotStateService(UserState userState, ConversationState conversationState)
        {
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));
            ConversationState = conversationState ?? throw new ArgumentNullException(nameof(conversationState));
            InitializeAccessors();
        }

        public void InitializeAccessors()
        {
            // Initialze User State
            UserProfileAccessor = UserState.CreateProperty<UserProfile>(UserProfileId);

            // Initialize Conversation State
            ConversationDataAccessor = ConversationState.CreateProperty<ConversationData>(ConversationDataId);
            DialogStateAccessor = ConversationState.CreateProperty<DialogState>(DialogStateId);
        }
    }
}
