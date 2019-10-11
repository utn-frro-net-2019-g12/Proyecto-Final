namespace Presentation.Library.Models
{
    public class Usuario {
        public int Id { get; set; }

        public string Username { get; set; }

        public int? Legajo { get; set; }
        public string Matricula { get; set; }
        public bool IsAdmin { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public int? Phone1 { get; set; }
        public int? Phone2 { get; set; }
    }
}
