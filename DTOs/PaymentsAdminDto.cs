namespace Crypto_Store.DTOs
{
    public class PaymentsAdminDto
    {
        public Guid Id {  get; set; }
        public string UserEmail { get; set; }
        public string ProductTitle { get; set; }
        public decimal Amount { get; set; }
        public string TxHash { get; set; }
        public DateTime Created { get; set; }
    }
}
