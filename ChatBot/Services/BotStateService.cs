using ChatBot.Models;
using Microsoft.Bot.Builder;
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

        // IDs
        public static string UserProfileId { get; } = $"{nameof(BotStateService)}.UserProfile";

        // Accessors 
        public IStatePropertyAccessor<UserProfile> UserProfileAccessor { get; set; }
        #endregion

        public BotStateService(UserState userState)
        {
            UserState = userState ?? throw new ArgumentNullException(nameof(userState));

            InitializeAccessors();
        }

        public void InitializeAccessors()
        {
            UserProfileAccessor = UserState.CreateProperty<UserProfile>(UserProfileId);
        }
    }
}
