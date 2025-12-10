using ferreterbackend.Data;
using ferreterbackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ferreterbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetodosPagoController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public MetodosPagoController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var metodos = await _db.MetodosPago.ToListAsync();
            return Ok(metodos);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(MetodoPago metodo)
        {
            _db.MetodosPago.Add(metodo);
            await _db.SaveChangesAsync();
            return Ok(metodo);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, MetodoPago dto)
        {
            var mp = await _db.MetodosPago.FindAsync(id);
            if (mp == null) return NotFound();

            mp.Nombre = dto.Nombre;
            mp.Descripcion = dto.Descripcion;

            await _db.SaveChangesAsync();
            return Ok(mp);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var mp = await _db.MetodosPago.FindAsync(id);
            if (mp == null) return NotFound();

            _db.MetodosPago.Remove(mp);
            await _db.SaveChangesAsync();

            return Ok("Método de pago eliminado");
        }
    }
}