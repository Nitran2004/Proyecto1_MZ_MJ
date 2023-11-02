using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Microsoft.EntityFrameworkCore;

namespace Proyecto1_MZ_MJ.Data
{
    public class Paginacion<T> : List<T>
    {
        public int PaginaInicio { get; private set; }
        public int PaginasTotales { get; private set; }

        public Paginacion(List<T> items, int contador, int paginaInicio, int cantidadregistros) {
            
            PaginaInicio = paginaInicio;
            
            PaginasTotales = (int)Math.Ceiling(contador/(double) cantidadregistros);

            this.AddRange(items);
            
        
        }
        public bool PaginasAnteriores => PaginaInicio > 1;
        public bool PaginasPosteriores => PaginaInicio < PaginasTotales;

        public static async Task<Paginacion<T>> CrearPaginacion(IQueryable<T> fuente, int paginaInicio, int cantidadregistros)
        {
            var count = fuente.Count();
            var items = await fuente.Skip((paginaInicio - 1) * cantidadregistros).Take(cantidadregistros).ToListAsync();
            return new Paginacion<T>(items, count, paginaInicio, cantidadregistros); // Pasa 'count' como argumento
        }

    }
}
