using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProgramacionIII.Shared.DTO_Response;
using ProgramacionIII.Shared.DTO_Roles;

namespace ProgramacionIII.Repositorios.Implementaciones
{
    public interface IRolesServicio
    {
        public Task<Response<List<VerRolDTO>>> BuscarTodosLosRoles();
        public Task<Response<String>> CrearRol(string nombre);
    }
}
