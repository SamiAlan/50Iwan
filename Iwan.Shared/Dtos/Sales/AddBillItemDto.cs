namespace Iwan.Shared.Dtos.Sales
{
    public class AddBillItemDto
    {
        public string BillId { get; set; }
        public int Quantity { get; set; }
        public string ProductId { get; set; }
        public decimal Price { get; set; }
    }
}
