using Microsoft.Extensions.Configuration;

namespace CustomerApi.Services
{
    public class FileSettings : ISettings
    {
        private readonly int searchCustomers_MaxResults;

        public FileSettings(IConfiguration configuration)
        {
            this.searchCustomers_MaxResults = int.Parse(configuration["SearchCustomers_MaxResults"]);
        }

        public int SearchCustomers_MaxResults => searchCustomers_MaxResults;
    }
}
