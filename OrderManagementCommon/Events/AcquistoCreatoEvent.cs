using OrderManagementCommon.Models;

namespace OrderManagementCommon.Events;

public record AcquistoCreatoEvent(IdProdotto IdProdotto, IdCustomer IdCustomer, Quantita QuantitaAcquista, DateTime MomentoAcquisto) : DomainEvent;