namespace SignalRAssignment.Models
{
    public class CartProcessParam
    {
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public string? ReturnUrl { get; set; }
        public string Action { get; set; } = null!;
    }
}
