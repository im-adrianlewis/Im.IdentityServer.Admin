az group create --name ImAccessGraphGroup --location westeurope
az aks create -g ImAccessGraphGroup -n ImAccessGraphCluster --location westeurope --disable-rbac --generate-ssh-keys
az aks use-dev-spaces -g ImAccessGraphGroup -n ImAccessGraphCluster --space default

#az ad sp create-for-rbac --sdk-auth --skip-assignment
#az aks show -g ImAccessGraphGroup -n ImAccessGraphCluster --query id
#az acr show --name imadrianlewis --query id
#az role assignment create --assignee <ClientId> --scope <AKSId> --role Contributor
#az role assignment create --assignee <ClientId>  --scope <ACRId> --role AcrPush