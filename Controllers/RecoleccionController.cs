using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto1_MZ_MJ.Data;
using Proyecto1_MZ_MJ.Models;
using System.Text.Json;

namespace Proyecto1_MZ_MJ.Controllers
{
    public class RecoleccionController : Controller
    {
        private readonly Proyecto1_MZ_MJContext _context;

        public RecoleccionController(Proyecto1_MZ_MJContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Seleccionar()
        {
            // Obtener todos los puntos de recolección
            var puntosRecoleccion = await _context.CollectionPoints
                .Include(p => p.Sucursal)
                .ToListAsync();

            // Verificar que existan puntos de recolección
            if (!puntosRecoleccion.Any())
            {
                return RedirectToAction("Index", "Home", new { mensaje = "No hay puntos de recolección disponibles" });
            }

            return View(puntosRecoleccion);
        }

        [HttpPost]
        public async Task<IActionResult> Confirmar(int id)
        {
            // Obtener el punto de recolección
            var puntoRecoleccion = await _context.CollectionPoints
                .Include(p => p.Sucursal)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (puntoRecoleccion == null)
            {
                return NotFound();
            }

            ViewBag.PuntoRecoleccionId = id;
            return View(puntoRecoleccion);
        }

        [HttpPost]
        public async Task<IActionResult> FinalizarPedido(int puntoRecoleccionId)
        {
            var puntoRecoleccion = await _context.CollectionPoints
                .Include(p => p.Sucursal)
                .FirstOrDefaultAsync(p => p.Id == puntoRecoleccionId);

            if (puntoRecoleccion == null)
            {
                return NotFound();
            }

            int pedidoId = 0;

            // Verificamos si viene de selección múltiple
            if (TempData.ContainsKey("ProductosSeleccionados"))
            {
                string productosJson = TempData["ProductosSeleccionados"].ToString();
                var productosSeleccionados = System.Text.Json.JsonSerializer.Deserialize<List<ProductoSeleccionadoInput>>(productosJson);
                // Necesitas implementar este método en PedidosController o en un servicio
                pedidoId = await CrearPedidoDesdeSeleccionMultiple(productosSeleccionados, puntoRecoleccion.SucursalId);
            }
            // Verificamos si viene del carrito
            else if (TempData.ContainsKey("DatosCarrito"))
            {
                string carritoJson = TempData["DatosCarrito"].ToString();
                // Necesitas implementar este método en PedidosController o en un servicio
                pedidoId = await CrearPedidoDesdeCarrito(carritoJson, puntoRecoleccion.SucursalId);
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

            // Guardar el ID del pedido actual para "Ver mi pedido"
            HttpContext.Session.SetInt32("PedidoActualId", pedidoId);

            // Guardar también en una cookie para mayor persistencia
            Response.Cookies.Append("PedidoActualId", pedidoId.ToString(), new CookieOptions
            {
                Expires = DateTimeOffset.Now.AddDays(1)
            });

            return RedirectToAction("Resumen", "Pedidos", new { id = pedidoId });
        }

        // Implementa estos métodos aquí o usa métodos de PedidosController
        private async Task<int> CrearPedidoDesdeSeleccionMultiple(List<ProductoSeleccionadoInput> seleccionados, int sucursalId)
        {
            var pedido = new Pedido
            {
                Fecha = DateTime.Now,
                SucursalId = sucursalId,
                PedidoProductos = new List<PedidoProducto>(),
                Estado = "Preparándose"
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            decimal total = 0;

            foreach (var item in seleccionados)
            {
                var producto = await _context.Productos.FindAsync(item.ProductoId);
                if (producto != null)
                {
                    decimal subtotal = producto.Precio * item.Cantidad;
                    total += subtotal;

                    var pedidoProducto = new PedidoProducto
                    {
                        PedidoId = pedido.Id,
                        ProductoId = producto.Id,
                        Cantidad = item.Cantidad,
                        Precio = producto.Precio
                    };

                    _context.PedidoProductos.Add(pedidoProducto);
                }
            }

            pedido.Total = total;
            _context.Update(pedido);
            await _context.SaveChangesAsync();

            return pedido.Id;
        }

        private async Task<int> CrearPedidoDesdeCarrito(string pedidoJson, int sucursalId)
        {
            var itemsCarrito = System.Text.Json.JsonSerializer.Deserialize<List<CarritoItem>>(pedidoJson);

            var pedido = new Pedido
            {
                Fecha = DateTime.Now,
                SucursalId = sucursalId,
                PedidoProductos = new List<PedidoProducto>(),
                Estado = "Preparándose"
            };

            _context.Pedidos.Add(pedido);
            await _context.SaveChangesAsync();

            decimal total = 0;

            foreach (var item in itemsCarrito)
            {
                if (int.TryParse(item.Id, out int productoId))
                {
                    var producto = await _context.Productos.FindAsync(productoId);
                    if (producto != null)
                    {
                        decimal subtotal = item.Precio * item.Cantidad;
                        total += subtotal;

                        var pedidoProducto = new PedidoProducto
                        {
                            PedidoId = pedido.Id,
                            ProductoId = productoId,
                            Cantidad = item.Cantidad,
                            Precio = item.Precio
                        };

                        _context.PedidoProductos.Add(pedidoProducto);
                    }
                }
            }

            pedido.Total = total;
            _context.Update(pedido);
            await _context.SaveChangesAsync();

            // Indicar que se debe limpiar el carrito
            TempData["LimpiarCarrito"] = true;

            return pedido.Id;
        }
    }
}