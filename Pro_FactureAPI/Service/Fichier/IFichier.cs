namespace Pro_FactureAPI.Service.Fichier;
 using Pro_FactureAPI.Models;

    public interface IFichier
{ 
    IEnumerable<Fichier> GetAll();
    Fichier Get(Guid id);
    Fichier Add(Fichier fichier);
    bool Update(Fichier item);
    void Remove(Guid id);
}
