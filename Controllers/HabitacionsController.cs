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
        public async Task<IActionResult> Index(string buscar, string ordenActual, int? numpag, string filtroActual)
        {
            var habitacions = from habitacion in _context.Habitacion select habitacion;

            if (buscar != null)
                numpag = 1;
            else
                buscar = filtroActual;

            if (!String.IsNullOrEmpty(buscar))
            {
                habitacions = habitacions.Where(s => s.Nombre!.Contains(buscar));
            }
            ViewData["OrdenActual"] = ordenActual;
            ViewData["FiltroActual"] = filtroActual;
            ViewData["FiltroNombre"] = String.IsNullOrEmpty(ordenActual) ? "NombreDescendente" : "";
            ViewData["FiltroPrecio"] = ordenActual == "PrecioAscendente" ? "PrecioDescendente" : "PrecioAscendente";
            ViewData["FiltroCapacidad"] = ordenActual == "CapacidadAscendente" ? "CapacidadDescendente" : "CapacidadAscendente";

            switch (ordenActual)
            {
                case "NombreDescendente":
                    habitacions = habitacions.OrderByDescending(habitacion => habitacion.Nombre);
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
            // Agrega este código para crear la paginación y pasarla a la vista
            var paginatedList = await Paginacion<Habitacion>.CrearPaginacion(habitacions.AsNoTracking(), numpag ?? 1, cantidadregistros);
            return View(paginatedList);
        }

        // GET: Habitacions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
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
        public async Task<IActionResult> Create([Bind("HabitacionId,Nombre,Capacidad,PrecioPorNoche,Descripcion,Disponible,Tipo,Vistas")] Habitacion habitacion)
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
        public async Task<IActionResult> Edit(int id, [Bind("HabitacionId,Nombre,Capacidad,PrecioPorNoche,Descripcion,Disponible,Tipo,Vistas")] Habitacion habitacion)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Habitacion == null)
            {
                return Problem("Entity set 'Proyecto1_MZContext.Habitacion' is null.");
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }


        // Cambia el nombre del método DeleteConfirmed a DeleteConfirmedAsync, por ejemplo
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedAsync(int id)
        {
            if (_context.Habitacion == null)
            {
                return Problem("Entity set 'Proyecto1_MZContext.Habitacion' is null.");
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

        // GET: Habitacions/Search


        public IActionResult Search(string nombre)
        {
            if (string.IsNullOrWhiteSpace(nombre))
            {
                // Si no se proporciona un nombre, redirigir a la página de índice
                return RedirectToAction(nameof(Index));
            }

            // Realizar la búsqueda en la base de datos por nombre
            var habitaciones = _context.Habitacion
                .Where(h => h.Nombre != null && h.Nombre.Contains(nombre))
                .ToList();

            return View("Index", habitaciones);
        }






    }
}
