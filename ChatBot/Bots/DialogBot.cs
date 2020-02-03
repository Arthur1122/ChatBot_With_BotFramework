using ChatBot.Helpers;
using ChatBot.Services;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace ChatBot.Bots
{
    public class DialogBot<T> : ActivityHandler where T : Dialog
    {
        #region  Variables
        protected BotStateService _botStateService;
        protected Dialog _dialog;
        protected ILogger _logger;
        #endregion

        public DialogBot(BotStateService botStateService, T dialog, ILogger<DialogBot<T>> logger)
        {
            _botStateService = botStateService ?? throw new ArgumentNullException(nameof(botStateService));
            _dialog = dialog ?? throw new ArgumentNullException(nameof(dialog));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public override async Task OnTurnAsync(ITurnContext turnContext, CancellationToken cancellationToken = default)
        {
            await base.OnTurnAsync(turnContext, cancellationToken);

            // Save any state changes that might have ocured durind the turn

            await _botStateService.UserState.SaveChangesAsync(turnContext, false, cancellationToken);
            await _botStateService.ConversationState.SaveChangesAsync(turnContext, false, cancellationToken);
        }

        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Running dialog with Message Activity");

            // Run the Dialog with the new message Activity.
            await _dialog.Run(turnContext, _botStateService.DialogStateAccessor, cancellationToken);
        }
    }
}
