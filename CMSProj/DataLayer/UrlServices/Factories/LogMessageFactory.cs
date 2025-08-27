namespace CMSProj.DataLayer.UrlServices.Factories
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
