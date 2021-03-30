using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Telegram.Bot.Types;
using TelegramBot.EnglishTrainer.Model;

namespace TelegramBot
{
    public class Conversation
    {
        private Chat telegramChat;

        private List<Message> telegramMessages;

        public Dictionary<string, Word> dictionary;

        public bool IsAddingInProcess;

        public bool IsTraningInProcess;

        public Conversation(Chat chat)
        {
            telegramChat = chat;
            telegramMessages = new List<Message>();
            dictionary = new Dictionary<string, Word>();
        }

        public void AddMessage(Message message)
        {
            telegramMessages.Add(message);
        }

        public void AddWord(string key, Word word)
        {
            dictionary.Add(key, word);
        }

        public void ClearHistory()
        {
            telegramMessages.Clear();
        }

        public List<string> GetTextMessages()
        {
            var textMessages = new List<string>();

            foreach(var message in telegramMessages)
            {
                if (message.Text != null)
                {
                    textMessages.Add(message.Text);
                }
            }

            return textMessages;
        }

        public long GetId() => telegramChat.Id;

        public string GetLastMessage() => telegramMessages[telegramMessages.Count - 1].Text;

        public string GetTrainingWord(TrainingType type)
        {
            var rand = new Random();
            var item = rand.Next(0, dictionary.Count);

            var randomword = dictionary.Values.AsEnumerable().ElementAt(item);

            var text = string.Empty;

            switch (type)
            {
                case TrainingType.EngToRus:
                    text =  randomword.English;
                    break;

                case TrainingType.RusToEng:
                    text = randomword.Russian;
                    break;
            }

            return text;
        }

        public bool CheckWord(TrainingType type, string word, string answer)
        {
            Word control;

            var result = false;

            switch (type)
            {

                case TrainingType.EngToRus:

                    control = dictionary.Values.FirstOrDefault(x => x.English == word);

                    result = control.Russian == answer;

                    break;

                case TrainingType.RusToEng:
                    control = dictionary[word];

                    result = control.English == answer;

                    break;
            }

            return result; 
        }

    }
}
