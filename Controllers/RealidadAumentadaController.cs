using Microsoft.AspNetCore.Mvc;
using Proyecto1_MZ_MJ.Data;
using Proyecto1_MZ_MJ.Models;
using System.Threading.Tasks;

namespace Proyecto1_MZ_MJ.Controllers
{
    public class RealidadAumentadaController : Controller
    {
        private readonly Proyecto1_MZ_MJContext _context;

        public RealidadAumentadaController(Proyecto1_MZ_MJContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> VistaAR(int id)
        {
            // Obtener el producto para pasar sus datos a la vista
            var producto = await _context.Productos.FindAsync(id);
            if (producto == null)
            {
                return NotFound();
            }

            return View(producto);
        }
    }
}