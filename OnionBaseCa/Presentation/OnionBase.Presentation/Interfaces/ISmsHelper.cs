namespace OnionBase.Presentation.Interfaces
{
    public interface ISmsHelper
    {
        Task<bool> SendSms(string message, string target);
    }
}
