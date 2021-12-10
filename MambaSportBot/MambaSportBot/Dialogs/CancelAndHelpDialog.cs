// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.
//
// Generated with Bot Builder V4 SDK Template for Visual Studio CoreBot v4.10.3

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Schema;

namespace MambaSportBot.Dialogs
{
    public class CancelAndHelpDialog : ComponentDialog
    {
        private const string HelpMsgText = "ѕриветсвую, это бот дл€ отслеживани€ информации о футбольных клубах и не только.\r\n\r\n** оманды, которыми вы можете воспользоватьс€:**\r\n\r\n**Ч help (?)** - _выводит это сообщение, где ¬ы можете посмотреть все доступные команды и начинает заново текущий диалог_\r\n\r\n" +
            "**Ч cancel (quit)** - _завершает текущий диалог и вызывает новый как по командe_ **fc**\r\n\r\n**Ч fc** - _диалог дл€ отслеживани€ информации о футбольных клубах_\r\n\r\n_ќбновлени€ в скором времени:_\r\n\r\n" +
            "Ч добавление Ћ„ и Ћ≈\r\n\r\nЧ добавление сборных\r\n\r\nЧ добавление информации о баскетбольных и хоккейных клубах";
        private const string CancelMsgText = "Cancelling...";

        public CancelAndHelpDialog(string id)
            : base(id)
        {
        }

        protected override async Task<DialogTurnResult> OnContinueDialogAsync(DialogContext innerDc, CancellationToken cancellationToken = default)
        {
            var result = await InterruptAsync(innerDc, cancellationToken);

            return await base.OnContinueDialogAsync(innerDc, cancellationToken);
        }

        private async Task<DialogTurnResult> InterruptAsync(DialogContext innerDc, CancellationToken cancellationToken)
        {
            if (innerDc.Context.Activity.Type == ActivityTypes.Message)
            {
                var text = innerDc.Context.Activity.Text.ToLowerInvariant();

                switch (text)
                {
                    case "help":
                    case "?":
                        var helpMessage = MessageFactory.Text(HelpMsgText, HelpMsgText, InputHints.ExpectingInput);
                        await innerDc.Context.SendActivityAsync(helpMessage, cancellationToken);
                        break;

                    case "cancel":
                    case "quit":
                        var cancelMessage = MessageFactory.Text(CancelMsgText, CancelMsgText, InputHints.IgnoringInput);
                        await innerDc.Context.SendActivityAsync(cancelMessage, cancellationToken);
                        return await innerDc.CancelAllDialogsAsync(cancellationToken);

                    case "fc":
                        return await innerDc.CancelAllDialogsAsync(cancellationToken);
                }
            }

            return null;
        }
    }
}
