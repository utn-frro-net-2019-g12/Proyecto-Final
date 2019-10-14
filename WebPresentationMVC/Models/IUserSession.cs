namespace Presentation.Web.MVC.Models
{
    public interface IUserSession
    {
        string Username { get; }
        string BearerToken { get; }
    }
}