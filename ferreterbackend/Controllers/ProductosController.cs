using ferreterbackend.Data;
using ferreterbackend.DTOs;
using ferreterbackend.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ferreterbackend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductosController : ControllerBase
    {
        private readonly ApplicationDbContext _db;

        public ProductosController(ApplicationDbContext db)
        {
            _db = db;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var productos = await _db.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .ToListAsync();

            return Ok(productos);
        }

        [HttpGet("{id:int}")]
        public async Task<IActionResult> GetById(int id)
        {
            var producto = await _db.Productos
                .Include(p => p.Categoria)
                .Include(p => p.Marca)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null) return NotFound();
            return Ok(producto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> Create(ProductoDto dto)
        {
            var producto = new Producto
            {
                Nombre = dto.Nombre,
                Descripcion = dto.Descripcion,
                Precio = dto.Precio,
                Stock = dto.Stock,
                CategoriaId = dto.CategoriaId,
                MarcaId = dto.MarcaId
            };

            _db.Productos.Add(producto);
            await _db.SaveChangesAsync();

            return Ok(producto);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id:int}")]
        public async Task<IActionResult> Update(int id, ProductoDto dto)
        {
            var producto = await _db.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            producto.Nombre = dto.Nombre;
            producto.Descripcion = dto.Descripcion;
            producto.Precio = dto.Precio;
            producto.Stock = dto.Stock;
            producto.CategoriaId = dto.CategoriaId;
            producto.MarcaId = dto.MarcaId;

            await _db.SaveChangesAsync();

            return Ok(producto);
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id:int}")]
        public async Task<IActionResult> Delete(int id)
        {
            var producto = await _db.Productos.FindAsync(id);
            if (producto == null) return NotFound();

            _db.Productos.Remove(producto);
            await _db.SaveChangesAsync();

            return Ok("Producto eliminado");
        }
    }
}
