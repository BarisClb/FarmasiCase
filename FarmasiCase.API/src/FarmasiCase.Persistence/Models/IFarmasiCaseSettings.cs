namespace FarmasiCase.Persistence.Models
{
    public interface IFarmasiCaseSettings
    {
        string DatabaseName { get; set; }
        string ConnectionString { get; set; }
        string OrdersCollectionName { get; set; }
        string ProductsCollectionName { get; set; }
        string UsersCollectionName { get; set; }
    }
}
