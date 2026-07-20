# 1. Caricare ingress
kubectl apply -f https://raw.githubusercontent.com/kubernetes/ingress-nginx/main/deploy/static/provider/cloud/deploy.yaml

# 2. Creare il database yb-demo

Se non gia fatto: helm repo add yugabytedb https://charts.yugabyte.com

kubectl create namespace yb-demo

helm install yugabyte-db yugabytedb/yugabyte --namespace yb-demo --set storage.master.storageClass=standard --set storage.tserver.storageClass=standard --set storage.master.size=10Gi  --set storage.tserver.size=10Gi --set replicas.master=1 --set replicas.tserver=1  --set resource.master.requests.cpu=1 --set resource.master.requests.memory=1Gi --set resource.tserver.requests.cpu=1 --set resource.tserver.requests.memory=2Gi

# 3. Creare la coda rabbitMQ

kubectl apply -f https://github.com/cert-manager/cert-manager/releases/latest/download/cert-manager.yaml

(Importante prima di eseguire il comando sottostante verificare che tutti i pod generati dal comando sopra siano attivi altrimenti il comando sotto fallira)

kubectl apply -f https://github.com/rabbitmq/cluster-operator/releases/latest/download/cluster-operator.yml

# 4. Build delle immagini

Per semplicità ho messo un docker compose quindi:

docker compose build

# 5. Deploy delle immagini su kubernates

cd kubernetes

kubectl apply -f .

# 6. Inserire account Admin

kubectl port-forward yb-master-0 -n yb-demo 7000:7000

Usare qualunque software compatibile con postgres per collegarsi al database (Username: yugabyte, Password: yugabyte).

Nello UserOrderManagement ce una tabella Utenti

INSERT Into "Utenti" ("Id", "Ruolo", "Username", "Password", "Cognome", "Nome")
    VALUES ('ec978f28-39b1-4cb1-8220-0bf06d559fe6', 'Admin', 'admin', '$2a$12$Y5HSIqdV5E1racEW64zp1OE4uG.WZd0dtfrbddtV2YMva.am0OBG6', 'Prova', 'Prova')

(La password è 1234)


# 7. (Opzionale) Altri comandi utili
kubectl rollout restart deployment
kubectl logs svc/delivery-service

kubectl port-forward <tipo-risorsa>/<nome-risorsa> <porta-locale>:<porta-remota>