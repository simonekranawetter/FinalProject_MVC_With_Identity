namespace FinalProject_MVC_With_Identity.Services
{
    public interface IMailService
    {
        void SendMessage(string to, string body);
    }
}
