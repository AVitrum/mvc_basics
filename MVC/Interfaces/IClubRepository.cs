using System.Collections;
using MVC.Models;

namespace MVC.Interfaces;

public interface IClubRepository
{
    Task<IEnumerable<Club>> GetAll();
    Task<Club> GetByIdAsync(int id);
    Task<IEnumerable<Club>> GetClubsByCity(string city);
    bool Add(Club club);
    Task<bool> AddAsync(Club club);
    bool Update(Club club);
    Task<bool> UpdateAsync(Club club);
    bool Delete(Club club);
    Task<bool> DeleteAsync(Club club);
    bool Save();
    Task<bool> SaveAsync();
}