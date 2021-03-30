using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot.Commands
{
    interface IChatTextCommandWithAction: IChatTextCommand
    {
        bool DoAction(Conversation chat);
    }
}
