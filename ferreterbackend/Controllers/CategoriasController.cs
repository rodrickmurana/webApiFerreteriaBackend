using ferreterbackend.Data;
using ferreterbackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ferreterbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriasController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public CategoriasController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categorias = await _db.Categorias.ToListAsync();
            return Ok(categorias);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            _db.Categorias.Add(categoria);
            await _db.SaveChangesAsync();
            return Ok(categoria);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Categoria dto)
        {
            var cat = await _db.Categorias.FindAsync(id);
            if (cat == null) return NotFound();

            cat.Nombre = dto.Nombre;
            await _db.SaveChangesAsync();
            return Ok(cat);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var cat = await _db.Categorias.FindAsync(id);
            if (cat == null) return NotFound();

            _db.Categorias.Remove(cat);
            await _db.SaveChangesAsync();
            return Ok("Categoría eliminada");
        }
    }
}