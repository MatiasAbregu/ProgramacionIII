using System.ComponentModel.DataAnnotations;

namespace ProgramacionIII.Shared.DTO_Usuarios
{
    public class ActualizarUsuarioDTO
    {
        [Required(ErrorMessage = "Es necesario un ID para actualizar el registro.")]
        public string Id { get; set; }
        public string? NombreUsuario { get; set; }
        public string? Contrasena { get; set; }
        public List<string>? Roles { get; set; }
    }
}
