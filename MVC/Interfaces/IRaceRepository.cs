using MVC.Models;

namespace MVC.Interfaces;

public interface IRaceRepository
{
    Task<IEnumerable<Race>> GetAll();
    Task<Race> GetByIdAsync(int id);
    Task<IEnumerable<Race>> GetAllRacesByCity(string city);
    
    bool Add(Race race);
    Task<bool> AddAsync(Race race);
    bool Update(Race race);
    Task<bool> UpdateAsync(Race userRace);
    bool Delete(Race race);
    bool Save();
    Task<bool> SaveAsync();
}