namespace OrderManagementWebFrontend.Services;

public class ApiClientProdotti(IConfiguration configuration) : ApiClient(configuration["ProductService:Url"]!) { }