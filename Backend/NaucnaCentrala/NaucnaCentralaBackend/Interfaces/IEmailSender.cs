namespace NaucnaCentralaBackend.Interfaces
{
    public interface IEmailSender
    {
        void Send(string emailSubject, string emailMessage);
    }
}
