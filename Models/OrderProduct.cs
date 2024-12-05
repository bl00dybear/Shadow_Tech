namespace Shadow_Tech.Models
{
    public class OrderProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public ApplicationUser User { get; set; }
        public Order Order { get; set; }
    }
}
