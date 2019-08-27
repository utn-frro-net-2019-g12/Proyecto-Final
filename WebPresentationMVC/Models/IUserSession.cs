namespace WebPresentationMVC.Models
{
    public interface IUserSession
    {
        string Username { get; }
        string BearerToken { get; }
    }
}