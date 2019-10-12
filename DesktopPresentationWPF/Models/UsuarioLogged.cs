namespace DesktopPresentationWPF.Models
{
    class UsuarioLogged : IUsuarioLogged
    {
        public string Username { get; set; }
        public string Token { get; set; }

        public void Set(string userName, string token)
        {
            Username = userName;
            Token = token;
        }

        public void LogOffUser()
        {
            Username = "";
            Token = "";
        }
    }
}
