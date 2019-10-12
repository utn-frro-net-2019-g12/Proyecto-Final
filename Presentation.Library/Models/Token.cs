namespace Presentation.Library.Models
{
    public class Token
    {
        public string UserName { get; set; }
        public string Access_token { get; set; }
        public string Token_type { get; set; }
        public string Expires_in { get; set; }

        public string FullToken
        {
            get
            {
                return $"{Token_type} {Access_token}";
            }
        }
    }
}