namespace ferreterbackend.Models
{
    public class Carrito
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public int Cantidad { get; set; }
    }
}