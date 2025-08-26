namespace CMSProj.DataLayer.UrlServices
{
    public class LogMessageFactory
    {
        public LogMessage Create()
        {
            return new LogMessage();
        }
        public LogMessage Create(LogMessage message)
        {
            return new LogMessage()
            {
                Message = message.Message,
                LogLevel = message.LogLevel
            };
        }
    }
}
