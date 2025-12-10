namespace ferreterbackend.Models
{
    public class Favorito
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public DateTime FechaAgregado { get; set; } = DateTime.Now;
    }
}