namespace GiftOfTheGivers.Models.ViewModels
{
    public class DonationVM
    {
        public Donation Donation { get; set; } = new Donation();
        public decimal ConvertedAmount { get; set; }
        public string TargetCurrency { get; set; } = "ZAR";
        public decimal ExchangeRate { get; set; } = 1.0m;
    }
}