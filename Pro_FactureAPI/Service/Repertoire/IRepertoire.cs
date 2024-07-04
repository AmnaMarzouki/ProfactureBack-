namespace Pro_FactureAPI.Service.Repertoire;

using Pro_FactureAPI.Models;


public interface IRepertoire
{
    IEnumerable<Repertoire> GetAll();
    Repertoire Get(int id);
    Repertoire Add(Repertoire repertoire);
    bool Update(Repertoire item);
    void Remove(int id);
}
