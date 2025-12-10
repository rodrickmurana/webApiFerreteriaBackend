using ferreterbackend.Data;
using ferreterbackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ferreterbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MarcasController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public MarcasController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var marcas = await _db.Marcas.ToListAsync();
            return Ok(marcas);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(Marca marca)
        {
            _db.Marcas.Add(marca);
            await _db.SaveChangesAsync();
            return Ok(marca);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, Marca dto)
        {
            var marca = await _db.Marcas.FindAsync(id);
            if (marca == null) return NotFound();

            marca.Nombre = dto.Nombre;
            await _db.SaveChangesAsync();
            return Ok(marca);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var marca = await _db.Marcas.FindAsync(id);
            if (marca == null) return NotFound();

            _db.Marcas.Remove(marca);
            await _db.SaveChangesAsync();
            return Ok("Marca eliminada");
        }
    }
}