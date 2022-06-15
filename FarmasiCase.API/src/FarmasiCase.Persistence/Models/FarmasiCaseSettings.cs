namespace FarmasiCase.Persistence.Models
{
    public class FarmasiCaseSettings : IFarmasiCaseSettings
    {
        public string DatabaseName { get; set; }
        public string ConnectionString { get; set; }
        public string OrdersCollectionName { get; set; }
        public string ProductsCollectionName { get; set; }
        public string UsersCollectionName { get; set; }
    }
}
