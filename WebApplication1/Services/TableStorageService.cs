using Azure.Data.Tables;
using WebApplication1.Models;

namespace WebApplication1.Services
{
    public class TableStorageService: ITableStorageService
    {
        private readonly string TableName = "Attendees";
        private readonly IConfiguration _configuration;

        public TableStorageService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task<AttendeeEntity> GetAttendeee(string industry, string id)
        {
            var tableClient = await GetTableClient();
            return await tableClient.GetEntityAsync<AttendeeEntity>(industry, id);
        }

        public async Task<List<AttendeeEntity>> GetAttendeees()
        {
            var tableClient = await GetTableClient();
            return await tableClient.QueryAsync<AttendeeEntity>().ToListAsync();
        }

        public async Task UpsertAttendeee(AttendeeEntity attendeeEntity)
        {
            var tableClient = await GetTableClient();
            await tableClient.UpsertEntityAsync(attendeeEntity);
        }

        public async Task DeleteAttendeee(string industry, string id)
        {
            var tableClient = await GetTableClient();
            await tableClient.DeleteEntityAsync(industry, id);
        }

        private async Task<TableClient> GetTableClient()
        {
            var serviceClient= new TableServiceClient(_configuration["AzureStorageConnectionString"]);

            var tableClient = serviceClient.GetTableClient(TableName);
            await tableClient.CreateIfNotExistsAsync();

            return tableClient;
        }
    }
}
