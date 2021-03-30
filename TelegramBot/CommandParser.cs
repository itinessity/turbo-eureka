using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Commands;
using TelegramBot.EnglishTrainer.Model;

namespace TelegramBot
{
    public class CommandParser
    {
        private List<IChatCommand> Command;

        private AddingController addingController;

        public CommandParser()
        {
            Command = new List<IChatCommand>();
            addingController = new AddingController();
        }

        public void AddCommand(IChatCommand chatCommand)
        {
            Command.Add(chatCommand);
        }

        public bool IsMessageCommand(string message)
        {
           return Command.Exists(x => x.CheckMessage(message));
        }

        public bool IsTextCommand(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message));

            return command is IChatTextCommand;
        }

        public bool IsButtonCommand(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message));

            return command is IKeyBoardCommand;
        }

        public string GetMessageText(string message, Conversation chat)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as IChatTextCommand;

            if (command is IChatTextCommandWithAction)
            {
                if (!(command as IChatTextCommandWithAction).DoAction(chat))
                {
                    return "Ошибка выполнения команды!";
                };
            }

            return command.ReturnText();
        }

        public string GetInformationalMeggase(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;

            return command.InformationalMessage();
        }

        public InlineKeyboardMarkup GetKeyBoard(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;

            return command.ReturnKeyBoard();
        }

        public void AddCallback(string message, Conversation chat)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as IKeyBoardCommand;
            command.AddCallBack(chat);
        }

        public bool IsAddingCommand(string message)
        {
            var command = Command.Find(x => x.CheckMessage(message));

            return command is AddWordCommand;
        }

        public void StartAddingWord(string message, Conversation chat)
        {
            var command = Command.Find(x => x.CheckMessage(message)) as AddWordCommand;

            addingController.AddFirstState(chat);
            command.StartProcessAsync(chat);

        }

        public void NextStage(string message, Conversation chat)
        {
            var command = Command.Find(x => x is AddWordCommand) as AddWordCommand;

            command.DoForStageAsync(addingController.GetStage(chat), chat, message);

            addingController.NextStage(message, chat);

        }


        public void ContinueTraining(string message, Conversation chat)
        {
            var command = Command.Find(x => x is TrainingCommand) as TrainingCommand;

            command.NextStepAsync(chat, message);

        }

    }
}
