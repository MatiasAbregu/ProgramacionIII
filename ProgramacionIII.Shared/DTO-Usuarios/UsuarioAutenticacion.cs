using System.ComponentModel.DataAnnotations;

namespace ProgramacionIII.Shared.DTO_Usuarios
{
    public class UsuarioAutenticacion
    {
        [Required]
        public string NombreUsuario { get; set; }
        [Required]
        public string Contrasena { get; set; }
        [Required]
        public bool MantenerSesion { get; set; }
    }
}
