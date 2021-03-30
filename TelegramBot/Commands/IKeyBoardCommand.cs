using System;
using System.Collections.Generic;
using System.Text;
using Telegram.Bot;
using Telegram.Bot.Types.ReplyMarkups;

namespace TelegramBot.Commands
{
    interface IKeyBoardCommand
    {
        InlineKeyboardMarkup ReturnKeyBoard();

        void AddCallBack(Conversation chat);

        string InformationalMessage();

    }
}
