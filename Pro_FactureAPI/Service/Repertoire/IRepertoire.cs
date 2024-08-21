namespace Pro_FactureAPI.Service.Repertoire;

using Pro_FactureAPI.Models;


public interface IRepertoire
{
    IEnumerable<Repertoire> GetAll();
    Repertoire Get(Guid id);
    Repertoire Add(Repertoire repertoire);
    bool Update(Guid id, string nouveauNom);
    void Remove(Guid id);
    IEnumerable<Repertoire> GetRepertoiresByUserId(Guid userId);
}
