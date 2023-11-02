using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proyecto1_MZ_MJ.Models;

namespace Proyecto1_MZ_MJ.Data
{
    public class Proyecto1_MZ_MJContext : DbContext
    {
        public Proyecto1_MZ_MJContext (DbContextOptions<Proyecto1_MZ_MJContext> options)
            : base(options)
        {
        }

        public DbSet<Proyecto1_MZ_MJ.Models.Habitacion> Habitacion { get; set; } = default!;

        public DbSet<Proyecto1_MZ_MJ.Models.Comentario>? Comentario { get; set; }

        public DbSet<Proyecto1_MZ_MJ.Models.Pago>? Pago { get; set; }
    }
}
