namespace PharmacyApp.Common
{
    public static class MedicineUtils
    {
        public static double ConvertPriceToUSD(double price, double rate)
        {
            return price * rate;
        }
    }
}
