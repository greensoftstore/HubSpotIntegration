using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

public class HubSpotService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiKey;

    public HubSpotService(HttpClient httpClient, string apiKey)
    {
        _httpClient = httpClient;
        _apiKey = apiKey;
    }

    public async Task<string> SendEmailAsync(string emailId, string to, string from, string subject, string content)
    {
        var sendEmailUrl = "https://api.hubapi.com/email/public/v1/singleEmail/send";
        var emailData = new
        {
            emailId,
            message = new
            {
                to = new[] { to },
                from,
                subject,
                content
            }
        };

        var jsonEmail = JsonConvert.SerializeObject(emailData);
        var dataEmail = new StringContent(jsonEmail, Encoding.UTF8, "application/json");

        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        var response = await _httpClient.PostAsync(sendEmailUrl, dataEmail);

        return response.IsSuccessStatusCode ? "Email sent successfully." : await response.Content.ReadAsStringAsync();
    }

    public async Task<string> EnrollContactInSequenceAsync(string email, string sequenceId)
    {
        var enrollSequenceUrl = "https://api.hubapi.com/automation/v3/actions/enrollments";
        var sequenceData = new
        {
            email,
            sequenceId
        };

        var jsonSequence = JsonConvert.SerializeObject(sequenceData);
        var dataSequence = new StringContent(jsonSequence, Encoding.UTF8, "application/json");
        _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", _apiKey);
        var response = await _httpClient.PostAsync(enrollSequenceUrl, dataSequence);

        return response.IsSuccessStatusCode ? "Contact enrolled in sequence successfully." : await response.Content.ReadAsStringAsync();
    }
}