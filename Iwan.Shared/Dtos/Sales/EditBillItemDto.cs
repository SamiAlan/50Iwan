namespace Iwan.Shared.Dtos.Sales
{
    public class EditBillItemDto
    {
        public string Id { get; set; }
        public int NewQuantity { get; set; }
        public decimal NewPrice { get; set; }
    }
}
