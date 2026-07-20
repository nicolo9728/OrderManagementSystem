#Script per creare il database DeliveryOrderManagment 
CREATE DATABASE "DeliveryOrderManagment";

CREATE TABLE "Eventi" (
    "Id" UUID PRIMARY KEY,
    "Tipo" TEXT NOT NULL,
    "Contenuto" TEXT NOT NULL,
    "IsCompletato" BOOLEAN NOT NULL,
    "MomentoCreazione" TIMESTAMP WITH TIME ZONE NOT NULL
);

CREATE TABLE "DeliveryGuys" (
    "Id" UUID PRIMARY KEY,
    "NumeroConsegneAttive" INTEGER NOT NULL
);

CREATE TABLE "Ordini" (
    "Id" UUID PRIMARY KEY,
    "IdProdotto" UUID NOT NULL,
    "IdUtente" UUID NOT NULL,
    "Quantita" INTEGER NOT NULL,
    "IdDeliveryGuyAssegnato" UUID,
    "MomentoCancellazione" TIMESTAMP WITH TIME ZONE,
    "MomentoConsegna" TIMESTAMP WITH TIME ZONE,
    "Tipo" TEXT NOT NULL,
    "MomentoAcquisto" TIMESTAMP WITH TIME ZONE NOT NULL,
    "IsDeliveryGuyNotified" BOOLEAN NOT NULL,
    "Indirizzo" TEXT NOT NULL
);

#Script per creare il database ProductOrderManagment 

CREATE DATABASE "ProductOrderManagment";

CREATE TABLE "Prodotti" (
    "Codice" UUID PRIMARY KEY,
    "Nome" TEXT NOT NULL,
    "QuantitaDisponibile" INTEGER NOT NULL
);

CREATE TABLE "Acquisto" (
    "Id" UUID PRIMARY KEY,
    "Momento" TIMESTAMP WITH TIME ZONE NOT NULL,
    "IdUtente" UUID NOT NULL,
    "IdProdotto" UUID NOT NULL,
    "QuantitaAcquistata" INTEGER NOT NULL,
    
    CONSTRAINT fk_prodotto FOREIGN KEY ("IdProdotto") REFERENCES "Prodotti"("Codice")
);

CREATE TABLE "Eventi" (
    "Id" UUID PRIMARY KEY,
    "Tipo" TEXT NOT NULL,
    "Contenuto" TEXT NOT NULL,
    "IsCompletato" BOOLEAN NOT NULL,
    "MomentoCreazione" TIMESTAMP WITH TIME ZONE NOT NULL
);

#Script per creare il database UserOrderManagment 
CREATE DATABASE "UserOrderManagment"

CREATE TABLE "Eventi" (
    "Id" UUID PRIMARY KEY,
    "Tipo" TEXT NOT NULL,
    "Contenuto" TEXT NOT NULL,
    "IsCompletato" BOOLEAN NOT NULL,
    "MomentoCreazione" TIMESTAMP WITH TIME ZONE NOT NULL
);

CREATE TABLE "Utenti" (
    "Id" UUID PRIMARY KEY,
    "Ruolo" VARCHAR(13) NOT NULL,
    "Password" TEXT NOT NULL,
    "Username" TEXT NOT NULL,
    "Cognome" TEXT NOT NULL,
    "Nome" TEXT NOT NULL,
    "IndirizzoCorrente" TEXT
);