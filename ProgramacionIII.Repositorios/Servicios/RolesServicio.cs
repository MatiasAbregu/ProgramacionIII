using ProgramacionIII.Repositorios.Implementaciones;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using ProgramacionIII.BD;
using ProgramacionIII.Shared.DTO_Response;
using ProgramacionIII.Shared.DTO_Roles;

namespace ProgramacionIII.Repositorios.Servicios
{
    public class RolesServicio : IRolesServicio
    {
        private readonly RoleManager<IdentityRole> roleManager;
        
        public RolesServicio(RoleManager<IdentityRole> roleManager)
        {
            this.roleManager = roleManager;
        }

        public async Task<Response<List<VerRolDTO>>> BuscarTodosLosRoles()
        {
            var roles = await roleManager.Roles
                .Select(r => new VerRolDTO
                {
                    NombreRol = r.Name 
                }).ToListAsync();

            return new Response<List<VerRolDTO>>
            {
                Objeto = roles,
                Estado = true,
                Mensaje = roles.Any() ? "" : "No hay roles registrados."
            };
        }

        public async Task<Response<string>> CrearRol(string nombre)
        {
            try
            {
                var rolExiste = await roleManager.RoleExistsAsync(nombre);
                if (rolExiste)
                {
                    return new Response<string>
                    {
                        Estado = false,
                        Mensaje = $"El rol '{nombre}' ya existe.",
                        Objeto = null
                    };
                }
                
                var nuevoRol = new IdentityRole(nombre);
                var resultado = await roleManager.CreateAsync(nuevoRol);

                if (resultado.Succeeded)
                {
                    return new Response<string>
                    {
                        Estado = true,
                        Mensaje = "Rol creado con éxito.",
                        Objeto = nuevoRol.Name
                    };
                }
                
                var errores = string.Join(", ", resultado.Errors.Select(e => e.Description));
                return new Response<string>
                {
                    Estado = false,
                    Mensaje = $"Error al crear el rol: {errores}",
                    Objeto = null
                };
            }
            catch (Exception ex)
            {
                return new Response<string>
                {
                    Estado = false,
                    Mensaje = $"Ocurrió un error inesperado: {ex.Message}",
                    Objeto = null
                };
            }
        }
    }
}
