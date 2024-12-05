namespace Shadow_Tech.Models
{
    public class Order
    {
        public int Id { get; set; }

        public DateTime Date { get; set; }
        public decimal Total {  get; set; }

        public virtual ApplicationUser User { get; set; }
    }
}
