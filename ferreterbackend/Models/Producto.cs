namespace ferreterbackend.Models
{
    public class Producto
    {
        public int Id { get; set; }
        public string Nombre { get; set; }
        public string Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }

        public int CategoriaId { get; set; }
        public Categoria Categoria { get; set; }

        public int MarcaId { get; set; }
        public Marca Marca { get; set; }

        public ICollection<DetalleOrden> DetallesOrden { get; set; }
        public ICollection<Carrito> Carrito { get; set; }
        public ICollection<Favorito> Favoritos { get; set; }
        public ICollection<Inventario> MovimientosInventario { get; set; }
    }
}