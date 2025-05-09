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
}
