using System.Collections;
using MVC.Models;

namespace MVC.Interfaces;

public interface IClubRepository
{
    Task<IEnumerable<Club>> GetAll();
    Task<Club> GetById(int id);
    Task<IEnumerable<Club>> GetClubsByCity(string city);
    bool Add(Club club);
    bool Update(Club club);
    bool Delete(Club club);
    bool Save();
}