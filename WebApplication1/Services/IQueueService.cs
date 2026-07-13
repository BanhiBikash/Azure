using WebApplication1.Models;

namespace WebApplication1.Services
{
    public interface IQueueService
    {
        Task SendMessage(Email email);
    }
}
