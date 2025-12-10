using ferreterbackend.Data;
using ferreterbackend.DTOs;
using ferreterbackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace ferreterbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [Authorize(Roles = "Cliente,Admin")]
    public class CarritoController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CarritoController(ApplicationDbContext db)
        {
            _db = db;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<IActionResult> GetMyCart()
        {
            var userId = GetUserId();

            var items = await _db.Carrito
                .Include(c => c.Producto)
                .Where(c => c.UsuarioId == userId)
                .ToListAsync();

            return Ok(items);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddCarritoDto dto)
        {
            var userId = GetUserId();

            var existing = await _db.Carrito
                .FirstOrDefaultAsync(c => c.UsuarioId == userId && c.ProductoId == dto.ProductoId);

            if (existing == null)
            {
                var item = new Carrito
                {
                    UsuarioId = userId,
                    ProductoId = dto.ProductoId,
                    Cantidad = dto.Cantidad
                };
                _db.Carrito.Add(item);
            }
            else
            {
                existing.Cantidad += dto.Cantidad;
            }

            await _db.SaveChangesAsync();
            return Ok("Producto agregado al carrito");
        }

        [HttpDelete("remove/{productoId:int}")]
        public async Task<IActionResult> Remove(int productoId)
        {
            var userId = GetUserId();

            var item = await _db.Carrito
                .FirstOrDefaultAsync(c => c.UsuarioId == userId && c.ProductoId == productoId);

            if (item == null) return NotFound();

            _db.Carrito.Remove(item);
            await _db.SaveChangesAsync();

            return Ok("Producto eliminado del carrito");
        }

        [HttpDelete("clear")]
        public async Task<IActionResult> Clear()
        {
            var userId = GetUserId();
            var items = await _db.Carrito.Where(c => c.UsuarioId == userId).ToListAsync();

            _db.Carrito.RemoveRange(items);
            await _db.SaveChangesAsync();

            return Ok("Carrito vaciado");
        }
    }
}
