using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;
using TelegramBot.Commands;

namespace TelegramBot
{
    public class Messenger
    {
        private ITelegramBotClient botClient;
        private CommandParser parser;

        public Messenger(ITelegramBotClient botClient)
        {
            this.botClient = botClient;
            parser = new CommandParser();

            RegisterCommands();
        }

        private void RegisterCommands()
        {
            parser.AddCommand(new SayHiCommand());
            parser.AddCommand(new PoemButtonCommand(botClient));
            parser.AddCommand(new AddWordCommand(botClient));
            parser.AddCommand(new DeleteWordCommand());
            parser.AddCommand(new TrainingCommand(botClient));
            parser.AddCommand(new StopTrainingCommand());
        }

        public async Task MakeAnswer(Conversation chat)
        {
            var lastmessage = chat.GetLastMessage();

            if (chat.IsTraningInProcess && !parser.IsTextCommand(lastmessage))
            {
                parser.ContinueTraining(lastmessage, chat);

                return;
            }

            if (chat.IsAddingInProcess)
            {
                parser.NextStage(lastmessage, chat);

                return;
            }

            if (parser.IsMessageCommand(lastmessage))
            {
                await ExecCommand(chat, lastmessage);
            }
            else
            {
                var text = CreateTextMessage();

                await SendText(chat, text);
            }

        }


        public async Task ExecCommand(Conversation chat, string command)
        {
            if (parser.IsTextCommand(command))
            {
                var text = parser.GetMessageText(command, chat);

                await SendText(chat, text);
            }

            if (parser.IsButtonCommand(command))
            {
                var keys = parser.GetKeyBoard(command);
                var text = parser.GetInformationalMeggase(command);
                parser.AddCallback(command, chat);

                await SendTextWithKeyBoard(chat, text, keys);

            }
           
            if(parser.IsAddingCommand(command))
            {
                chat.IsAddingInProcess = true; 
                parser.StartAddingWord(command, chat);
            }
        }

        private string CreateTextMessage()
        {
            var text = "Not a command";

            return text;
        }

        private async Task SendText(Conversation chat, string text)
        {
            await botClient.SendTextMessageAsync(
                  chatId: chat.GetId(),
                  text: text
                );
        }

        private async Task SendTextWithKeyBoard(Conversation chat, string text, InlineKeyboardMarkup keyboard)
        {
            await botClient.SendTextMessageAsync(
                  chatId: chat.GetId(),
                  text: text,
                  replyMarkup: keyboard
                );
        }
    }
}
