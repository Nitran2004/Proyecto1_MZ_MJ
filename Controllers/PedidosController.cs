using Microsoft.AspNetCore.Mvc;
using Proyecto1_MZ_MJ.Data;
using Proyecto1_MZ_MJ.Models;
using System.Security.Claims;
using Microsoft.EntityFrameworkCore; // ← necesario para Include y ToListAsync


public class PedidosController : Controller
{
    private readonly Proyecto1_MZ_MJContext _context;

    public PedidosController(Proyecto1_MZ_MJContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Crear(int productoId, int cantidad)
    {
        var producto = await _context.Productos.FindAsync(productoId);
        if (producto == null) return NotFound();

        var pedido = new Pedido
        {
            UsuarioId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null,
            PedidoProductos = new List<PedidoProducto>
            {
                new PedidoProducto
                {
                    ProductoId = producto.Id,
                    Cantidad = cantidad,
                    Precio = producto.Precio
                }
            }
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return RedirectToAction("Resumen", new { id = pedido.Id });
    }

    public async Task<IActionResult> Resumen(int id)
    {
        var pedido = await _context.Pedidos
            .Include(p => p.PedidoProductos!)
                .ThenInclude(pp => pp.Producto)
            .FirstOrDefaultAsync(p => p.Id == id);

        if (pedido == null) return NotFound();

        return View(pedido);
    }

    public async Task<IActionResult> Admin()
    {
        var pedidos = await _context.Pedidos.ToListAsync();
        return View(pedidos);
    }

    [HttpGet]
    public async Task<IActionResult> SeleccionarProductos()
    {
        var productos = await _context.Productos.ToListAsync();

        var viewModel = new PedidoViewModel
        {
            ProductosSeleccionados = productos.Select(p => new ProductoSeleccionado
            {
                ProductoId = p.Id,
                Nombre = p.Nombre,
                Precio = p.Precio,
                Cantidad = 1
            }).ToList()
        };

        return View(viewModel);
    }

    [HttpPost]
    public async Task<IActionResult> CrearPedidoMultiple(PedidoViewModel model)
    {
        var productosElegidos = model.ProductosSeleccionados
            .Where(p => p.Cantidad > 0)
            .ToList();

        if (!productosElegidos.Any()) return RedirectToAction("SeleccionarProductos");

        var pedido = new Pedido
        {
            UsuarioId = User.Identity.IsAuthenticated ? User.FindFirstValue(ClaimTypes.NameIdentifier) : null,
            PedidoProductos = productosElegidos.Select(p => new PedidoProducto
            {
                ProductoId = p.ProductoId,
                Cantidad = p.Cantidad,
                Precio = p.Precio
            }).ToList()
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return RedirectToAction("Resumen", new { id = pedido.Id });
    }

    [HttpPost]
    public async Task<IActionResult> CrearPedido([FromBody] List<PedidoDetalle> detalles)
    {
        if (detalles == null || !detalles.Any())
            return BadRequest("Carrito vacío");

        var pedido = new Pedido
        {
            Fecha = DateTime.Now,
            Total = detalles.Sum(d => d.Cantidad * d.PrecioUnitario),
            Detalles = detalles
        };

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        return Ok(new { mensaje = "Pedido creado exitosamente", pedido.Id });
    }
    [HttpPost]
    public async Task<IActionResult> AgregarSeleccionados(List<ProductoSeleccionadoInput> seleccionados)
    {
        var seleccionadosValidos = seleccionados
            .Where(p => p.Seleccionado && p.Cantidad > 0)
            .ToList();

        if (!seleccionadosValidos.Any())
        {
            return BadRequest("Datos inválidos");
        }

        var pedido = new Pedido
        {
            Fecha = DateTime.Now,
            PedidoProductos = new List<PedidoProducto>()
        };

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

        pedido.Total = total; // ✅ Ahora sí se asigna el total correcto

        _context.Pedidos.Add(pedido);
        await _context.SaveChangesAsync();

        // Guardar ID del pedido en una cookie por 30 minutos
        CookieOptions options = new CookieOptions
        {
            Expires = DateTimeOffset.Now.AddMinutes(30)
        };
        Response.Cookies.Append("PedidoTemporalId", pedido.Id.ToString(), options);

        return RedirectToAction("Resumen", new { id = pedido.Id });
    }

    public async Task<IActionResult> ResumenAdmin()
    {
        var pedidos = await _context.Pedidos
            .Include(p => p.PedidoProductos!)
                .ThenInclude(pp => pp.Producto)
            .OrderByDescending(p => p.Fecha)
            .ToListAsync();

        return View(pedidos);
    }

    public async Task<IActionResult> VerPedidoTemporal()
    {
        if (Request.Cookies.TryGetValue("PedidoTemporalId", out string pedidoIdStr) && int.TryParse(pedidoIdStr, out int pedidoId))
        {
            var pedido = await _context.Pedidos
                .Include(p => p.PedidoProductos)
                    .ThenInclude(pp => pp.Producto)
                .FirstOrDefaultAsync(p => p.Id == pedidoId);

            if (pedido != null)
            {
                return View("Resumen", pedido); // o "ResumenAdmin" si es esa la vista
            }
        }

        return RedirectToAction("Index", "Home"); // O una vista de error
    }


    [HttpPost]
    public async Task<IActionResult> ActualizarEstado(int id, string estado)
    {
        var pedido = await _context.Pedidos.FindAsync(id);
        if (pedido == null)
        {
            return NotFound();
        }

        pedido.Estado = estado;
        await _context.SaveChangesAsync();

        return RedirectToAction("ResumenAdmin", new { id = pedido.Id });
    }



}
