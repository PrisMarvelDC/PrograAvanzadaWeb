using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;

namespace ProyectoPAW.Areas.Identity.Data;

// Add profile data for application users by adding properties to the AplicationUser class
public class ApplicationUser : IdentityUser
{
    public string Nombre { get; set; }
    public string Apellidos { get; set; }
    public string Cedula { get; set; }
    public string Telefono { get; set; }
}

