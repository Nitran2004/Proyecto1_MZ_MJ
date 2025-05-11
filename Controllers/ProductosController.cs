using Microsoft.AspNetCore.Mvc;
using Proyecto1_MZ_MJ.Data;
using Microsoft.EntityFrameworkCore;
using Proyecto1_MZ_MJ.Models;
using Proyecto1_MZ_MJ.Models.DTOs;
using System.Threading.Tasks;
using System.Linq;
using System.Collections.Generic;

namespace Proyecto1_MZ_MJ.Controllers
{
    public class ProductosController : Controller
    {
        private readonly Proyecto1_MZ_MJContext _context;

        public ProductosController(Proyecto1_MZ_MJContext context)
        {
            _context = context;
        }

        // Este método ya existe, mantenerlo igual
        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        // Método para mostrar detalle mejorado de un producto
        public async Task<IActionResult> Detalle(int id)
        {
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null) return NotFound();
            return View(producto);
        }

        // Método para selección múltiple
        public async Task<IActionResult> SeleccionMultiple()
        {
            var productos = await _context.Productos.ToListAsync();
            var categorias = productos.Select(p => p.Categoria).Distinct().ToList();

            ViewBag.Categorias = categorias;

            return View(productos);
        }

        // Método para agregar productos seleccionados al carrito y seguir comprando
        [HttpPost]
        public IActionResult AgregarYSeguir(List<ProductoSeleccionadoInput> seleccionados)
        {
            // Filtrar solo productos seleccionados
            var seleccionadosValidos = seleccionados
                .Where(p => p.Seleccionado && p.Cantidad > 0)
                .ToList();

            if (!seleccionadosValidos.Any())
            {
                return RedirectToAction("SeleccionMultiple");
            }

            // Guardar datos en TempData para mostrar mensaje de éxito
            TempData["ProductosAgregados"] = seleccionadosValidos.Count;

            return RedirectToAction("SeleccionMultiple");
        }

        // Método para mostrar todos los productos por categoría pero funcional
        // (actualizado para funcionar como página independiente)
        public async Task<IActionResult> PorCategoria(string categoria)
        {
            var productos = await _context.Productos.ToListAsync();

            if (!string.IsNullOrEmpty(categoria) && categoria.ToLower() != "todas")
            {
                productos = productos.Where(p => p.Categoria == categoria).ToList();
            }

            ViewBag.CategoriaActual = categoria;
            ViewBag.Categorias = await _context.Productos
                .Select(p => p.Categoria)
                .Distinct()
                .ToListAsync();

            return View(productos);
        }

        // Métodos existentes para mostrar categorías específicas
        public async Task<IActionResult> Menu()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> Pizzas()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> Sanduches()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> Picadas()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> Bebidas()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> Promos()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> Cerveza()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> Cocteles()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        public async Task<IActionResult> Shots()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }
    }
}