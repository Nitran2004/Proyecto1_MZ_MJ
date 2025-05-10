using Microsoft.AspNetCore.Mvc;
using Proyecto1_MZ_MJ.Data;
using Microsoft.EntityFrameworkCore;
using Proyecto1_MZ_MJ.Models;
using Proyecto1_MZ_MJ.Models.DTOs;


namespace Proyecto1_MZ_MJ.Controllers
{
    public class ProductosController : Controller
    {

        private readonly Proyecto1_MZ_MJContext _context;


        public ProductosController(Proyecto1_MZ_MJContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto, IFormFile? imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.Length > 0)
                {
                    using var memoryStream = new MemoryStream();
                    await imageFile.CopyToAsync(memoryStream);
                    producto.Imagen = memoryStream.ToArray();
                }

                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

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

        public async Task<IActionResult> Recoleccion()
        {
            var productos = await _context.Productos.ToListAsync();
            return View(productos);
        }

        //public async Task<IActionResult> Detalle(int id)
        //{
        //    var producto = await _context.Productos.FindAsync(id);
        //    if (producto == null) return NotFound();
        //    return View(producto);  // Buscará Views/Productos/Detalle.cshtml
        //}

        // ProductosController.cs
        public async Task<IActionResult> Detalle(int id)
        {
            // Find the product by its ID
            var producto = await _context.Productos.FindAsync(id);

            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }

        [HttpPost]
        public async Task<IActionResult> CrearPedidoConUbicacion([FromBody] PedidoConUbicacionDTO datos)
        {
            if (datos == null)
                return BadRequest("Datos nulos");

            if (datos.ProductosIdsSeleccionados == null || !datos.ProductosIdsSeleccionados.Any())
                return BadRequest("No se enviaron productos seleccionados.");

            var productos = await _context.Productos
                .Where(p => datos.ProductosIdsSeleccionados.Contains(p.Id))
                .ToListAsync();

            if (productos.Count == 0)
                return BadRequest("No se encontraron productos con los IDs proporcionados.");

            var sucursales = await _context.Sucursales.ToListAsync();
            if (sucursales.Count == 0)
                return BadRequest("No hay sucursales registradas.");

            // Continúa como antes...


            // Calcula la sucursal más cercana
            Sucursal sucursalMasCercana = null;
            double menorDistancia = double.MaxValue;

            foreach (var sucursal in sucursales)
            {
                double distancia = CalcularDistancia(
                    datos.Latitud, datos.Longitud,
                    sucursal.Latitud, sucursal.Longitud);

                if (distancia < menorDistancia)
                {
                    menorDistancia = distancia;
                    sucursalMasCercana = sucursal;
                }
            }

            // Aquí puedes guardar el pedido. Por ejemplo:
            var pedido = new Pedido
            {
                Fecha = DateTime.Now,
                Productos = productos,
                Sucursal = sucursalMasCercana
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            return Ok();
        }

        private double CalcularDistancia(double lat1, double lon1, double lat2, double lon2)
        {
            double R = 6371; // radio de la Tierra en km
            double dLat = (lat2 - lat1) * Math.PI / 180;
            double dLon = (lon2 - lon1) * Math.PI / 180;

            double a = Math.Sin(dLat / 2) * Math.Sin(dLat / 2) +
                       Math.Cos(lat1 * Math.PI / 180) * Math.Cos(lat2 * Math.PI / 180) *
                       Math.Sin(dLon / 2) * Math.Sin(dLon / 2);

            double c = 2 * Math.Atan2(Math.Sqrt(a), Math.Sqrt(1 - a));
            return R * c; // distancia en km
        }

    }
}
