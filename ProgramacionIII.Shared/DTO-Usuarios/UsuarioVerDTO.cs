namespace ProgramacionIII.Shared.DTO_Usuarios
{
    public class UsuarioVerDTO
    {
        public string Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Estado { get; set; }
        public List<string> roles { get; set; }
    }
}
