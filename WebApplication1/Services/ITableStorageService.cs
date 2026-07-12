using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ITableStorageService
    {

        Task<AttendeeEntity> GetAttendeee(string industry, string id);
        Task<List<AttendeeEntity>> GetAttendeees();
        Task UpsertAttendeee(AttendeeEntity attendeeEntity);
        Task DeleteAttendeee(string industry, string id);
    }
}
