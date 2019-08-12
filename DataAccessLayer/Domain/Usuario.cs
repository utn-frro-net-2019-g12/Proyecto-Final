using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Drawing;

namespace DataAccessLayer {
    public class Usuario {
        public int Id { get; set; }

        // Username --> FK(Users_IdentityFramework)
        // Por ahora la dejo como atributo normal (abajo comentada la FK)
        [Required]
        [StringLength(50)]
        public string Username { get; set; }

        // Legajo != Null --> Es Alumno
        public int? Legajo { get; set; }
        // Matrícula != Null --> Es Profesor
        [StringLength(50)]
        public string Matricula { get; set; }
        [Required]
        public bool IsAdmin { get; set; }
        [Required]
        [StringLength(50)]
        public string Firstname { get; set; }
        [Required]
        [StringLength(50)]
        public string Surname { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        public int? Phone { get; set; }
        // public Bitmap Photo { get; set; } --> Consultar

        // [ForeignKey("User_IdentityFramework")]
        // public string Username { get; set; }
        // public virtual IdentityFramework... { get; set; }
    }
}
