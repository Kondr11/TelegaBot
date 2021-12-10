using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using MambaSportBot.Models;
using MambaSportBot.MyHeroCards;

namespace MambaSportBot.Dialogs
{
    public class FC : CancelAndHelpDialog
    {
        HeroCards HeroCards { get; set; }

        public FC()
            : base(nameof(FC))
        {
            var waterfallSteps = new WaterfallStep[]
            {
                DialogInfo,
                StepBack,
                FinalStepAsync,
            };
            AddDialog(new TextPrompt(nameof(FC)));
            AddDialog(new ConfirmPrompt(nameof(ConfirmPrompt)));
            AddDialog(new WaterfallDialog(nameof(WaterfallDialog), waterfallSteps));

            // The initial child Dialog to run.
            InitialDialogId = nameof(WaterfallDialog);
        }

        public async Task<DialogTurnResult> DialogInfo(WaterfallStepContext dc, CancellationToken cancellationToken)
        {
            if (HeroCards == null)
                HeroCards = new HeroCards(nameof(FC), dc);
            var activity = dc.Context.Activity;
            string command = string.Empty;
            if (activity.Text != null)
                command = dc.Context.Activity.Text.Trim().ToLower();
            return await HeroCards.HeroCardSwitch(dc, command, activity, cancellationToken);
        }

        private async Task<DialogTurnResult> StepBack(WaterfallStepContext dc, CancellationToken cancellationToken)
        {
            return await dc.BeginDialogAsync(InitialDialogId, (List<Team>)dc.Options, cancellationToken);
        }

        private async Task<DialogTurnResult> FinalStepAsync(WaterfallStepContext dc, CancellationToken cancellationToken)
        {
            await DialogInfo(dc, cancellationToken);
            return await dc.EndDialogAsync(cancellationToken);
        }
    }
}
