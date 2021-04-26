using HotelFinalProgramacionAvanzada.DataAccess.Data;
using HotelFinalProgramacionAvanzada.DataAccess.Repositorio.IRepositorio;
using HotelFinalProgramacionAvanzada.Models;
using HotelFinalProgramacionAvanzada.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelFinalProgramacionAvanzada
{
    public static class Setup
    {

        public static async Task<bool> InitAsync(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            bool estado = false;
            var RoleManager = roleManager;
            var UserManager = userManager;

            if (!await RoleManager.RoleExistsAsync(SD.Roles.Administrador))
            {
                await RoleManager.CreateAsync(new IdentityRole(SD.Roles.Administrador));
                estado = true;
            }
            if (!await RoleManager.RoleExistsAsync(SD.Roles.Empleado))
            {
                await RoleManager.CreateAsync(new IdentityRole(SD.Roles.Empleado));
            }
            if (!await RoleManager.RoleExistsAsync(SD.Roles.Cliente))
            {
                await RoleManager.CreateAsync(new IdentityRole(SD.Roles.Cliente));
            }
            var user =
                new Usuario
                {
                    UserName = "admin@admin.com",
                    Email = "admin@admin.com",
                    Nombre = "admin",
                    PhoneNumber = "12345",
                };
            if (!UserManager.Users.Select(u => u.Email == "admin@admin.com").FirstOrDefault())
            {
                await UserManager.CreateAsync(user, "Admin-2020");
                await UserManager.AddToRoleAsync(user, SD.Roles.Administrador);
            }
            return estado;
        }
    }
}