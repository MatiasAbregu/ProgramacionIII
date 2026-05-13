using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramacionIII.Shared.DTO_Response;
using ProgramacionIII.Shared.DTO_Usuarios;

namespace ProgramacionIII.Repositorios.Implementaciones
{
    public interface IUsuarioServicio
    {
        public Task<Response<List<UsuarioVerDTO>>> BuscarUsuarios();
        public Task<Response<UsuarioVerDTO>> BuscarUsuarioPorId(string id);
        public Task<Response<List<UsuarioVerDTO>>> BuscarUsuarioPorNombre(string nombre);
        public Task<Response<UsuarioVerDTO>> CrearNuevoUsuario(UsuarioCrearDTO usuarioDTO);
        public Task<Response<UsuarioVerDTO>> ActualizarUsuario(ActualizarUsuarioDTO usuarioDTO);
        public Task<Response<UsuarioVerDTO>> CambiarEstadoUsuario(string id);
   
    }
}