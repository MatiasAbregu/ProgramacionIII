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
using ProgramacionIII.Shared.DTO_Usuarios;

namespace ProgramacionIII.Repositorios.Servicios
{
    public class UsuarioServicio : IUsuarioServicio
    {
        private readonly UserManager<ApplicationUser> userManager;

        public UsuarioServicio(UserManager<ApplicationUser> userManager)
        {
            this.userManager = userManager;
        }

        public async Task<Response<List<UsuarioVerDTO>>> BuscarUsuarios()
        {
            try
            {
                var usuarios = await userManager.Users.ToListAsync();
                var listaUsuariosDTO = new List<UsuarioVerDTO>();

                foreach (var u in usuarios)
                {
                    var roles = await userManager.GetRolesAsync(u);
                    bool estaActivo = u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow;

                    listaUsuariosDTO.Add(new UsuarioVerDTO
                    {
                        Id = u.Id,
                        NombreUsuario = u.UserName,
                        Estado = estaActivo ? "Activo" : "Desactivado", 
                        roles = roles.ToList()
                    });
                }

                return new Response<List<UsuarioVerDTO>>
                {
                    Estado = true,
                    Mensaje = listaUsuariosDTO.Any() ? "" : "No hay usuarios aún registrados en el sistema.",
                    Objeto = listaUsuariosDTO
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<List<UsuarioVerDTO>>
                    { Estado = false, Mensaje = "Hubo un error al cargar los datos", Objeto = null };
            }
        }

        public async Task<Response<UsuarioVerDTO>> BuscarUsuarioPorId(string id)
        {
            try
            {
                var u = await userManager.FindByIdAsync(id);
                if (u == null) return new Response<UsuarioVerDTO> { Estado = false, Objeto = null, Mensaje = "Usuario no encontrado." };

                var roles = await userManager.GetRolesAsync(u);
                bool estaActivo = u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow;
                
                return new Response<UsuarioVerDTO>
                {
                    Estado = true,
                    Objeto = new UsuarioVerDTO
                    {
                        Id = u.Id,
                        NombreUsuario = u.UserName,
                        roles = roles.ToList(),
                        Estado = estaActivo ? "Activo" : "Desactivado", 
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<UsuarioVerDTO>
                {
                    Objeto = null,
                    Mensaje = $"Ocurrio un error: {ex.Message}",
                    Estado = false
                };
            }
        }

        public async Task<Response<List<UsuarioVerDTO>>> BuscarUsuarioPorNombre(string nombre)
        {
            try
            {
                var usuarios = await userManager.Users
                    .Where(u => u.UserName.Contains(nombre))
                    .ToListAsync();
                var listaUsuariosDTO = new List<UsuarioVerDTO>();

                foreach (var u in usuarios)
                {
                    var roles = await userManager.GetRolesAsync(u);
                    bool estaActivo = u.LockoutEnd == null || u.LockoutEnd <= DateTimeOffset.UtcNow;

                    listaUsuariosDTO.Add(new UsuarioVerDTO
                    {
                        Id = u.Id,
                        NombreUsuario = u.UserName,
                        Estado = estaActivo ? "Activo" : "Desactivado", 
                        roles = roles.ToList()
                    });
                }
                return new Response<List<UsuarioVerDTO>>
                {
                    Estado = true,
                    Mensaje = listaUsuariosDTO.Any() ? "" : "No hay usuarios con ese nombre aún registrados en el sistema.",
                    Objeto = listaUsuariosDTO
                };
            }
            catch (Exception e)
            {
                Console.WriteLine("Error: " + e.Message);
                return new Response<List<UsuarioVerDTO>>
                    { Estado = false, Mensaje = "Hubo un error al cargar los usuarios por un nombre", Objeto = null };
            }
        }

        public async Task<Response<UsuarioVerDTO>> CrearNuevoUsuario(UsuarioCrearDTO usuarioDTO)
        {
            try
            {
                var usuario = new ApplicationUser { UserName = usuarioDTO.NombreUsuario };
                var resultado = await userManager.CreateAsync(usuario, usuarioDTO.Contrasena);

                if (!resultado.Succeeded)
                {
                    var errores = string.Join(", ", resultado.Errors.Select(e => e.Description));
                    return new Response<UsuarioVerDTO> { Estado = false, Mensaje = errores };
                }
                
                if (usuarioDTO.Roles != null && usuarioDTO.Roles.Any())
                {
                    await userManager.AddToRolesAsync(usuario, usuarioDTO.Roles);
                }

                var roles = await userManager.GetRolesAsync(usuario);
                
                return new Response<UsuarioVerDTO>
                {
                    Estado = true,
                    Mensaje = "Usuario creado con éxito",
                    Objeto = new UsuarioVerDTO { 
                        Id = usuario.Id,
                        NombreUsuario = usuario.UserName,
                        Estado = usuario.LockoutEnabled ? "Activo" : "Desactivado",
                        roles = roles.ToList()
                    }
                };
            }
            catch (Exception ex)
            {
                return new Response<UsuarioVerDTO> { Estado = false, Mensaje = ex.Message, Objeto = null};
            }
        }

        public async Task<Response<UsuarioVerDTO>> ActualizarUsuario(ActualizarUsuarioDTO usuarioDTO)
        {
            try
            {
                var usuario = await userManager.FindByIdAsync(usuarioDTO.Id.ToString());
                if (usuario == null) return new Response<UsuarioVerDTO> { Estado = false, Mensaje = "Usuario no encontrado" };

                if (!string.IsNullOrEmpty(usuarioDTO.NombreUsuario))
                    usuario.UserName = usuarioDTO.NombreUsuario;

                if (!string.IsNullOrEmpty(usuarioDTO.Contrasena))
                {
                    var token = await userManager.GeneratePasswordResetTokenAsync(usuario);
                    await userManager.ResetPasswordAsync(usuario, token, usuarioDTO.Contrasena);
                }
            
                if (usuarioDTO.Roles != null)
                {
                    var rolesActuales = await userManager.GetRolesAsync(usuario);
                    await userManager.RemoveFromRolesAsync(usuario, rolesActuales);
                    await userManager.AddToRolesAsync(usuario, usuarioDTO.Roles);
                }

                await userManager.UpdateAsync(usuario);
                return new Response<UsuarioVerDTO>
                {
                    Estado = true,
                    Mensaje = "Usuario actualizado con éxito.",
                    Objeto = new UsuarioVerDTO()
                    {
                        Id = usuario.Id,
                        NombreUsuario = usuario.UserName,
                        Estado = usuario.LockoutEnabled ? "Activo" : "Desactivado",
                        roles = usuarioDTO.Roles
                    }
                };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ocurrió un error {ex}");
                return new Response<UsuarioVerDTO>
                { Estado = false, Mensaje = $"Ocurrió un error: {ex.Message}", Objeto = null };
            }
        }

        public async Task<Response<UsuarioVerDTO>> CambiarEstadoUsuario(string id)
        {
            try
            {
                var usuario = await userManager.FindByIdAsync(id);
                if (usuario == null) return new Response<UsuarioVerDTO> { Estado = false, Mensaje = "Usuario no encontrado" };

                var estaBloqueado = await userManager.IsLockedOutAsync(usuario);
                
                if (estaBloqueado) await userManager.SetLockoutEndDateAsync(usuario, null);
                else await userManager.SetLockoutEndDateAsync(usuario, DateTimeOffset.UtcNow.AddYears(200));
                
                string msg = estaBloqueado ? "activado" : "desactivado";

                return new Response<UsuarioVerDTO>
                    { Estado = true, Mensaje = $"Usuario {msg} con éxito", Objeto = null };
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Ocurrió un error {ex}");
                return new Response<UsuarioVerDTO>
                    { Estado = false, Mensaje = $"Ocurrió un error: {ex.Message}", Objeto = null };
                    
            }
        }
    }
}