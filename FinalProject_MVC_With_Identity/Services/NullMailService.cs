namespace FinalProject_MVC_With_Identity.Services
{
    public class NullMailService : IMailService
    {
        private readonly ILogger<NullMailService> _logger;
        public NullMailService(ILogger<NullMailService> logger)
        {
            _logger = logger;
        }
        public void SendMessage(string to, string body)
        {
            _logger.LogInformation($"To: {to} Body: {body}");
        }
    }
}
