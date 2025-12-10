namespace ferreterbackend.Models
{
    public class Inventario
    {
        public int Id { get; set; }

        public int ProductoId { get; set; }
        public Producto Producto { get; set; }

        public int Cantidad { get; set; }
        public string TipoMovimiento { get; set; }  // Entrada | Salida | Ajuste

        public DateTime Fecha { get; set; } = DateTime.Now;
    }
}