namespace OrderManagementWebFrontend.Services;

public class ApiClientUser(IConfiguration configuration) : ApiClient(configuration["UserService:Url"]!)
{
}