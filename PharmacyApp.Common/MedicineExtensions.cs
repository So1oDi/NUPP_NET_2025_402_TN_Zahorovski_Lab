namespace PharmacyApp.Common
{
    public static class MedicineExtensions
    {
        public static bool IsInStock(this Medicine medicine)
        {
            return medicine.QuantityInStock > 0;
        }
    }
}
