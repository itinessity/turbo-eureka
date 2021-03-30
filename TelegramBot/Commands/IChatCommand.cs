using System;
using System.Collections.Generic;
using System.Text;
using TelegramBot.EnglishTrainer.Model;

namespace TelegramBot
{
    public interface IChatCommand
    {
        bool CheckMessage(string message);
    }
}
