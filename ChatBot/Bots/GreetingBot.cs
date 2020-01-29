using ChatBot.Models;
using ChatBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatBot.Bots
{
    public class GreetingBot : ActivityHandler
    {
        #region Variables
        private readonly BotStateService _botStateService;

        #endregion
        public GreetingBot(BotStateService botStateService)
        {
            _botStateService = botStateService ?? throw new ArgumentNullException(nameof(botStateService));
        }
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            await GetName(turnContext, cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            foreach (var member in membersAdded)
            {
                if(member.Id != turnContext.Activity.Recipient.Id)
                {
                    await GetName(turnContext, cancellationToken);
                }
            }
        }

        private async Task GetName(ITurnContext turnContext, CancellationToken cancellationToken)
        {
            UserProfile userProfile = await _botStateService.UserProfileAccessor.GetAsync(turnContext, () => new UserProfile());
            ConversationData conversationData = await _botStateService.ConversationDataAccessor.GetAsync(turnContext, () => new ConversationData()); 

            if(!String.IsNullOrEmpty(userProfile.Name))
            {
                await turnContext.SendActivityAsync(MessageFactory.Text(String.Format("Hi {0}. How Can I help you ?",userProfile.Name)),cancellationToken);
            }
            else
            {
                if(conversationData.PromptedUserForName)
                {
                    // Set the name to what the user provided
                    userProfile.Name = turnContext.Activity.Text?.Trim();

                    // Acknowledge thet we got their name
                    await turnContext.SendActivityAsync(MessageFactory.Text(String.Format("Hi {0}. How can I help you today ?", userProfile.Name)), cancellationToken);

                    // Reset the flag to allow the bot to go though the cycle again
                    conversationData.PromptedUserForName = false;
                }
                else
                {
                    // Prompt the user for their Name
                    await turnContext.SendActivityAsync(MessageFactory.Text($"What is your name ?"), cancellationToken);

                    // Set the flag to true, so we don't prompt in the next turn
                    conversationData.PromptedUserForName = true;
                }

                // Save any state changes that might have occured during turn

                await _botStateService.UserProfileAccessor.SetAsync(turnContext, userProfile);
                await _botStateService.ConversationDataAccessor.SetAsync(turnContext, conversationData);

                await _botStateService.UserState.SaveChangesAsync(turnContext);
                await _botStateService.ConversationState.SaveChangesAsync(turnContext);
            }
        }
    }
}
