using Microsoft.AspNetCore.Mvc;
using ProgramacionIII.Repositorios.Implementaciones;
using ProgramacionIII.Shared.DTO_Response;
using ProgramacionIII.Shared.DTO_Roles;

namespace ProgramacionIII.Controladores
{
    [Route("api/roles")]
    [ApiController]
    public class RolesControlador : ControllerBase
    {
        private readonly IRolesServicio rolesServicio;
        public RolesControlador(IRolesServicio rolesServicio) 
        {
            this.rolesServicio = rolesServicio;
        }

        [HttpGet]
        public async Task<ActionResult> ObtenerRoles() 
        {
            Response<List<VerRolDTO>> res = await rolesServicio.BuscarTodosLosRoles();
            return StatusCode(200, res);
        }

        [HttpPost]
        public async Task<ActionResult> CrearRol([FromBody] string nombre)
        {
            var res = await rolesServicio.CrearRol(nombre);
            return StatusCode(200, res);
        }
    }
}
