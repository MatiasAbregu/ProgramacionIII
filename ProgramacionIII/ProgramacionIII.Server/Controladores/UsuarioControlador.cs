using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OutputCaching;
using ProgramacionIII.Repositorios.Implementaciones;
using ProgramacionIII.Shared.Constantes;
using ProgramacionIII.Shared.DTO_Response;
using ProgramacionIII.Shared.DTO_Usuarios;

namespace ProgramacionIII.Controladores
{
    [Route("api/usuarios")]
    [ApiController]
    public class UsuarioControlador : ControllerBase
    {
        private readonly IUsuarioServicio usuarioServicio;
        private readonly IOutputCacheStore outputCache;
        private const string cacheKey = "UsuariosCache";
        
        public UsuarioControlador(IUsuarioServicio usuarioServicio, IOutputCacheStore outputCache)
        {
            this.usuarioServicio = usuarioServicio;
            this.outputCache = outputCache;
        }

        [HttpGet]
        [AllowAnonymous]
        [OutputCache(Tags = [cacheKey])]
        public async Task<ActionResult> VisualizarUsuarios()
        {
            Response<List<UsuarioVerDTO>> res
                = await usuarioServicio.BuscarUsuarios();


            if (res.Estado == true)
            {
                Response.Headers["Cache-Control"] = $"public,max-age={ConstantesGlobales.DuracionCacheEnSegundos}";
                return StatusCode(200, res);
            }
            else return StatusCode(500, res);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> ObtenerUsuarioPorId(string id)
        {
            Response<UsuarioVerDTO> res = await usuarioServicio.BuscarUsuarioPorId(id);

            if (res.Estado == true) return StatusCode(200, res);
            return StatusCode(500, res);
        }

        [HttpGet("nombre/{nombreUsuario}")]
        public async Task<ActionResult> ObtenerUsuarioPorNombre(string nombreUsuario)
        {
            Response<List<UsuarioVerDTO>> res =
                await usuarioServicio.BuscarUsuarioPorNombre(nombreUsuario);

            if (res.Estado == true) return StatusCode(200, res);
            return StatusCode(500, res);
        }

        [HttpPost]
        public async Task<ActionResult> CrearNuevoUsuario(UsuarioCrearDTO usuarioDTO)
        {
            if (usuarioDTO == null) return StatusCode(400, new Response<UsuarioVerDTO>
            { Estado = false, Mensaje = "Es necesario cargar los datos del usuario a crear.", Objeto = null });

            Response<UsuarioVerDTO> res = await usuarioServicio.CrearNuevoUsuario(usuarioDTO);

            await outputCache.EvictByTagAsync(cacheKey, default);
            if (res.Estado == true) return StatusCode(200, res);
            return StatusCode(500, res);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> ActualizarUsuario(ActualizarUsuarioDTO usuarioDTO, string id)
        {
            if (usuarioDTO.Id != id) return StatusCode(409, new Response<String>
            { Estado = false, Mensaje = "Hubo un error en el servidor, intentelo más tarde.", Objeto = null });

            Response<UsuarioVerDTO> res = await usuarioServicio.ActualizarUsuario(usuarioDTO);

            if (res.Estado == true) return StatusCode(200, res);
            return StatusCode(500, res);
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> CambiarEstadoUsuario(string id)
        {
            Response<UsuarioVerDTO> res = await usuarioServicio.CambiarEstadoUsuario(id);

            if (res.Estado == true) return StatusCode(200, res);
            return StatusCode(500, res);
        }
    }
}