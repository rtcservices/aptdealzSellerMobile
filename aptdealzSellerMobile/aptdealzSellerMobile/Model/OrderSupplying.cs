namespace aptdealzSellerMobile.Model
{
    public class OrderSupplying
    {
        public string InvoiceId { get; set; }
        public double InvAmount { get; set; }
        public string DeliveryStatus { get; set; }
        public string DeliveryDate { get; set; }
        public string ExpDeliveryDate { get; set; }
        public bool IsVisible { get; set; }

    }
}
