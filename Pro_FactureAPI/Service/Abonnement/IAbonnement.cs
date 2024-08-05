namespace Pro_FactureAPI.Service.Abonnement;
     using Pro_FactureAPI.Models;

    public interface IAbonnement
    {
    IEnumerable<Abonnement> GetAll();
    Abonnement Get(Guid id);
    Abonnement Add(Abonnement abonnement);
    bool Update(Abonnement abonnement);
    void Remove(Guid id);
    bool SetInactive(Guid id);
}

