namespace Pro_FactureAPI.Service.Repertoire;

using Pro_FactureAPI.Models;


public interface IRepertoire
{
    IEnumerable<Repertoire> GetAll();
    Repertoire Get(Guid id);
    Repertoire Add(Repertoire repertoire);
    bool Update(Repertoire item);
    void Remove(Guid id);
}
