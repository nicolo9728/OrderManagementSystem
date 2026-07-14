using OrderManagementCommon.Models;

namespace OrderManagementCommon.Events;

public record DeliveryGuyCreatoEvent(IdDeliveryGuy Id) : DomainEvent;