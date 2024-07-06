using System.Collections.Generic;

namespace Iwan.Shared.Dtos.Sales
{
    public class AddBillDto
    {
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        public IList<AddBillItemDto> BillItems { get; set; }
    }
}
