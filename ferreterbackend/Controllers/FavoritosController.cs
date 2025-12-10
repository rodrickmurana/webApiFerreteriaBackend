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
    public class FavoritosController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public FavoritosController(ApplicationDbContext db)
        {
            _db = db;
        }

        private string GetUserId() =>
            User.FindFirstValue(ClaimTypes.NameIdentifier);

        [HttpGet]
        public async Task<IActionResult> GetMyFavorites()
        {
            var userId = GetUserId();
            var favoritos = await _db.Favoritos
                .Include(f => f.Producto)
                .Where(f => f.UsuarioId == userId)
                .ToListAsync();

            return Ok(favoritos);
        }

        [HttpPost("add")]
        public async Task<IActionResult> Add(AddFavoritoDto dto)
        {
            var userId = GetUserId();

            var exists = await _db.Favoritos
                .AnyAsync(f => f.UsuarioId == userId && f.ProductoId == dto.ProductoId);

            if (exists)
                return BadRequest("El producto ya está en favoritos");

            var fav = new Favorito
            {
                UsuarioId = userId,
                ProductoId = dto.ProductoId
            };

            _db.Favoritos.Add(fav);
            await _db.SaveChangesAsync();

            return Ok("Producto agregado a favoritos");
        }

        [HttpDelete("remove/{productoId:int}")]
        public async Task<IActionResult> Remove(int productoId)
        {
            var userId = GetUserId();

            var fav = await _db.Favoritos
                .FirstOrDefaultAsync(f => f.UsuarioId == userId && f.ProductoId == productoId);

            if (fav == null) return NotFound();

            _db.Favoritos.Remove(fav);
            await _db.SaveChangesAsync();

            return Ok("Producto eliminado de favoritos");
        }
    }
}
