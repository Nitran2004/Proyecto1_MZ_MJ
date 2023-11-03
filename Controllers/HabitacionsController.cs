using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using Proyecto1_MZ_MJ.Data;
using Proyecto1_MZ_MJ.Models;
using X.PagedList;

namespace Proyecto1_MZ_MJ.Controllers
{
    public class HabitacionsController : Controller
    {
        private readonly Proyecto1_MZ_MJContext _context;

        public HabitacionsController(Proyecto1_MZ_MJContext context)
        {
            _context = context;
        }

        // GET: Habitacions
        public async Task<IActionResult> Index()
        {
              return _context.Habitacion != null ? 
                          View(await _context.Habitacion.ToListAsync()) :
                          Problem("Entity set 'Proyecto1_MZ_MJContext.Habitacion'  is null.");
        }
        [HttpGet]
        [Route("Proyecto1_MZ_MJ/Index")]
        // GET: Habitacions/Details/5
        public async Task<IActionResult> Index(string buscar, string ordenActual, int? numpag, string filtroActual)
        {
            var habitacions = from habitacion in _context.Habitacion select habitacion;

            if (buscar != null)
                numpag = 1;
            else
                buscar = filtroActual;

            ViewData["OrdenActual"] = ordenActual;
            ViewData["FiltroActual"] = filtroActual;
            ViewData["FiltroNum"] = ordenActual == "NumAscendente" ? "NumDescendente" : "NumAscendente";
            ViewData["FiltroPrecio"] = ordenActual == "PrecioAscendente" ? "PrecioDescendente" : "PrecioAscendente";
            ViewData["FiltroCapacidad"] = ordenActual == "CapacidadAscendente" ? "CapacidadDescendente" : "CapacidadAscendente";

            if (!String.IsNullOrEmpty(buscar))
            {
                string numHabitacionStr = buscar.ToLower(); // Convierte a minúsculas si es necesario
                habitacions = habitacions.Where(s => s.NumHabitacion != null && s.NumHabitacion.ToString().Contains(numHabitacionStr));
            }


            switch (ordenActual)
            {
                case "NumDescendente":
                    habitacions = habitacions.OrderByDescending(habitacion => habitacion.NumHabitacion);
                    break;
                case "PrecioAscendente":
                    habitacions = habitacions.OrderBy(habitacion => habitacion.PrecioPorNoche);
                    break;
                case "PrecioDescendente":
                    habitacions = habitacions.OrderByDescending(habitacion => habitacion.PrecioPorNoche);
                    break;
                case "CapacidadAscendente":
                    habitacions = habitacions.OrderBy(habitacion => habitacion.Capacidad);
                    break;
                case "CapacidadDescendente":
                    habitacions = habitacions.OrderByDescending(habitacion => habitacion.Capacidad);
                    break;
                default:
                    habitacions = habitacions.OrderBy(habitacion => habitacion.PrecioPorNoche);
                    break;
            }

            int cantidadregistros = 6;
            var paginatedList = await Paginacion<Habitacion>.CrearPaginacion(habitacions.AsNoTracking(), numpag ?? 1, cantidadregistros);
            return View(paginatedList);
        }





// GET: Habitacions/Create
public IActionResult Create()
        {
            return View();
        }

        // POST: Habitacions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("HabitacionId,NumHabitacion,Capacidad,PrecioPorNoche,Descripcion,Disponible,Tipo,Vistas")] Habitacion habitacion)
        {
            if (ModelState.IsValid)
            {
                _context.Add(habitacion);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(habitacion);
        }

        // GET: Habitacions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Habitacion == null)
            {
                return NotFound();
            }

            var habitacion = await _context.Habitacion.FindAsync(id);
            if (habitacion == null)
            {
                return NotFound();
            }
            return View(habitacion);
        }

        // POST: Habitacions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("HabitacionId,NumHabitacion,Capacidad,PrecioPorNoche,Descripcion,Disponible,Tipo,Vistas")] Habitacion habitacion)
        {
            if (id != habitacion.HabitacionId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(habitacion);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HabitacionExists(habitacion.HabitacionId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(habitacion);
        }

        // GET: Habitacions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Habitacion == null)
            {
                return NotFound();
            }

            var habitacion = await _context.Habitacion
                .FirstOrDefaultAsync(m => m.HabitacionId == id);
            if (habitacion == null)
            {
                return NotFound();
            }

            return View(habitacion);
        }

        // POST: Habitacions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Habitacion == null)
            {
                return Problem("Entity set 'Proyecto1_MZ_MJContext.Habitacion'  is null.");
            }
            var habitacion = await _context.Habitacion.FindAsync(id);
            if (habitacion != null)
            {
                _context.Habitacion.Remove(habitacion);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool HabitacionExists(int id)
        {
          return (_context.Habitacion?.Any(e => e.HabitacionId == id)).GetValueOrDefault();
        }







    }
}
