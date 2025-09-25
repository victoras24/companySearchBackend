using Microsoft.Azure.Functions.Worker;

namespace CompanySearchBackend.Functions
{
    public class AlwaysOnTimerTrigger(ILoggerFactory loggerFactory)
    {
        private readonly ILogger _logger = loggerFactory.CreateLogger<AlwaysOnTimerTrigger>();
        private static readonly HttpClient HttpClient = new HttpClient();

        private const string BackendUrl = "https://companysearchcyprus-cshzasdadrgdcjf4.westeurope-01.azurewebsites.net/api/company/open";

        [Function("AlwaysOnTimerTrigger")]
        public async Task Run([TimerTrigger("0 */18 * * * *")] TimerInfo myTimer)
        {
            _logger.LogInformation("Keep-alive ping executed at: {executionTime}", DateTime.Now);

            if (myTimer.ScheduleStatus is not null)
            {
                _logger.LogInformation("Next schedule at: {nextSchedule}", myTimer.ScheduleStatus.Next);
            }

            try
            {
                var response = await HttpClient.GetAsync(BackendUrl);
                _logger.LogInformation("Pinged {url} - Status: {status}", BackendUrl, response.StatusCode);
            }
            catch (Exception ex)
            {
                _logger.LogError("Error pinging {url}: {message}", BackendUrl, ex.Message);
            }
        }
    }
}