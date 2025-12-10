namespace ferreterbackend.Models
{
    public class Orden
    {
        public int Id { get; set; }
        public string UsuarioId { get; set; }

        public int MetodoPagoId { get; set; }
        public MetodoPago MetodoPago { get; set; }

        public decimal Total { get; set; }
        public DateTime FechaCreacion { get; set; } = DateTime.Now;

        public ICollection<DetalleOrden> Detalles { get; set; }
    }
}