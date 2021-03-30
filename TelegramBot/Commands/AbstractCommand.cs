namespace TelegramBot
{
    public abstract class AbstractCommand : IChatCommand
    {
        public string CommandText;

        public virtual bool CheckMessage(string message)
        {
            return CommandText == message;
        }
    }
}
