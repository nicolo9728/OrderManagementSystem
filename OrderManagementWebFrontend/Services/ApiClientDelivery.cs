namespace OrderManagementWebFrontend.Services;

public class ApiClientDelivery(IConfiguration configuration) : ApiClient(configuration["DeliveryService:Url"]!) {}