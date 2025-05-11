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

        public IActionResult PorCategoria(string categoria)
        {
            var productosFiltrados = _context.Productos
                .Where(p => p.Categoria == categoria)
                .ToList();

            return View(productosFiltrados);
        }

        [HttpPost]
        public IActionResult PorCategoria(List<Producto> productosSeleccionados)
        {
            if (!ModelState.IsValid)
            {
                return View(productosSeleccionados);
            }

            // Procesar y redirigir
            return RedirectToAction("Seleccionar", "Recoleccion");
        }

        // 1. Primero, vamos a crear un nuevo método en ProductosController.cs para la página de selección múltiple

        // Añade este método al ProductosController.cs
        public async Task<IActionResult> SeleccionMultiple()
        {
            var productos = await _context.Productos.ToListAsync();
            var categorias = productos.Select(p => p.Categoria).Distinct().ToList();

            ViewBag.Categorias = categorias;

            return View(productos);
        }

        // 2. Ahora creamos un ViewModel para gestionar los productos seleccionados
        // Crea un nuevo archivo en Models/CarritoViewModel.cs

        /*
        namespace Proyecto1_MZ_MJ.Models
        {
            public class CarritoViewModel
            {
                public List<ProductoSeleccionado> ProductosSeleccionados { get; set; } = new List<ProductoSeleccionado>();
                public decimal Total => ProductosSeleccionados.Sum(p => p.Precio * p.Cantidad);
                public int CantidadTotal => ProductosSeleccionados.Sum(p => p.Cantidad);
            }
        }
        */

        // 3. Añade este método al PedidosController.cs para procesar múltiples productos

        [HttpPost]
        public async Task<IActionResult> ProcesarSeleccionMultiple(List<ProductoSeleccionadoInput> seleccionados)
        {
            // Filtrar solo los productos que han sido seleccionados y tienen cantidad > 0
            var seleccionadosValidos = seleccionados
                .Where(p => p.Seleccionado && p.Cantidad > 0)
                .ToList();

            if (!seleccionadosValidos.Any())
            {
                return RedirectToAction("SeleccionMultiple", "Productos");
            }

            // Obtener la sucursal (asumimos que existe al menos una)
            var sucursal = await _context.Sucursales.FirstOrDefaultAsync();
            if (sucursal == null)
            {
                // Crear una sucursal predeterminada si no existe ninguna
                sucursal = new Sucursal
                {
                    Nombre = "Verace Pizza",
                    Direccion = "Av. de los Shyris N35-52",
                    Latitud = -0.180653,
                    Longitud = -78.487834
                };
                _context.Sucursales.Add(sucursal);
                await _context.SaveChangesAsync();
            }

            // Crear el pedido
            var pedido = new Pedido
            {
                Fecha = DateTime.Now,
                SucursalId = sucursal.Id,
                PedidoProductos = new List<PedidoProducto>(),
                Estado = "Preparándose"
            };

            // Agregar productos al pedido
            decimal total = 0;
            foreach (var item in seleccionadosValidos)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto != null)
                {
                    decimal subtotal = producto.Precio * item.Cantidad;
                    total += subtotal;

                    pedido.PedidoProductos.Add(new PedidoProducto
                    {
                        ProductoId = producto.Id,
                        Cantidad = item.Cantidad,
                        Precio = producto.Precio
                    });
                }
            }

            pedido.Total = total;

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            // Guardar ID del pedido en una cookie
            CookieOptions options = new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddMinutes(30)
            };
            Response.Cookies.Append("PedidoTemporalId", pedido.Id.ToString(), options);

            return RedirectToAction("Resumen", new { id = pedido.Id });
        }


        //[HttpPost]
        //public async Task<IActionResult> AgregarSeleccionados(List<ProductoSeleccionadoInput> seleccionados, int? puntoRecoleccionId = null)
        //{
        //    var seleccionadosValidos = seleccionados
        //        .Where(p => p.Seleccionado && p.Cantidad > 0)
        //        .ToList();

        //    if (!seleccionadosValidos.Any())
        //    {
        //        return BadRequest("Datos inválidos");
        //    }

        //    // Si estamos en el flujo normal (primera parte), redirigimos a Recoleccion/Seleccionar
        //    if (puntoRecoleccionId == null)
        //    {
        //        // Guardar los productos seleccionados en sesión o TempData para recuperarlos después
        //        var productosSeleccionados = seleccionadosValidos.Select(p => new { p.ProductoId, p.Cantidad }).ToList();
        //        HttpContext.Session.SetString("ProductosSeleccionados", System.Text.Json.JsonSerializer.Serialize(productosSeleccionados));

        //        return RedirectToAction("Seleccionar", "Recoleccion");
        //    }

        //    // Si llegamos aquí es porque ya tenemos un punto de recolección seleccionado
        //    var puntoRecoleccion = await _context.CollectionPoints.FindAsync(puntoRecoleccionId);

        //    // Obtener la sucursal asociada al punto de recolección o la primera disponible
        //    var sucursalId = puntoRecoleccion?.SucursalId;

        //    if (sucursalId == null)
        //    {
        //        var sucursal = await _context.Sucursales.FirstOrDefaultAsync();
        //        if (sucursal == null)
        //        {
        //            // Crear una sucursal si no existe ninguna
        //            sucursal = new Sucursal
        //            {
        //                Nombre = "Verace Pizza",
        //                Direccion = "Av. de los Shyris N35-52",
        //                Latitud = -0.180653,
        //                Longitud = -78.487834
        //            };
        //            _context.Sucursales.Add(sucursal);
        //            await _context.SaveChangesAsync();
        //        }
        //        sucursalId = sucursal.Id;
        //    }

        //    var pedido = new Pedido
        //    {
        //        Fecha = DateTime.Now,
        //        SucursalId = sucursalId.Value,
        //        PedidoProductos = new List<PedidoProducto>(),
        //        Estado = "Preparándose"
        //    };

        //    // Si nuestro modelo tiene un campo para punto de recolección, lo asignamos
        //    if (puntoRecoleccionId.HasValue)
        //    {
        //        // Descomenta esto si tienes un campo PuntoRecoleccionId en tu modelo Pedido
        //        // pedido.PuntoRecoleccionId = puntoRecoleccionId.Value;
        //    }

        //    decimal total = 0;
        //    foreach (var item in seleccionadosValidos)
        //    {
        //        var producto = await _context.Productos.FindAsync(item.ProductoId);
        //        if (producto != null)
        //        {
        //            decimal subtotal = producto.Precio * item.Cantidad;
        //            total += subtotal;

        //            pedido.PedidoProductos.Add(new PedidoProducto
        //            {
        //                ProductoId = producto.Id,
        //                Cantidad = item.Cantidad,
        //                Precio = producto.Precio
        //            });
        //        }
        //    }

        //    pedido.Total = total;

        //    _context.Pedidos.Add(pedido);
        //    await _context.SaveChangesAsync();

        //    // Guardamos el ID del pedido en una cookie
        //    CookieOptions options = new CookieOptions
        //    {
        //        Expires = DateTimeOffset.Now.AddMinutes(30)
        //    };
        //    Response.Cookies.Append("PedidoTemporalId", pedido.Id.ToString(), options);

        //    // Limpiamos la sesión de productos seleccionados
        //    HttpContext.Session.Remove("ProductosSeleccionados");

        //    // Redirigimos al resumen del pedido
        //    return RedirectToAction("Resumen", new { id = pedido.Id });
        //}
    }
}
