using System;
using System.Collections.Generic;
using System.Text;

namespace TelegramBot
{
    public interface IChatCommand
    {
        bool CheckMessage(string message);
    }
}
