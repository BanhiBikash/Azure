using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface ITableStorageService
    {

        Task<AttendeeEntity> GetAttendeee(string id);
        Task<List<AttendeeEntity>> GetAttendeees(string id);
        Task UpsertAttendeee(AttendeeEntity attendeeEntity);
        Task DeleteAttendeee(AttendeeEntity attendeeEntity);
    }
}
