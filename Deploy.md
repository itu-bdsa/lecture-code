# Deploy

```bash
SQL_GROUP_DISPLAY_NAME=BDSA2022-Admins
SQL_GROUP_NICKNAME=bdsa2022-admins
LOCATION=westeurope
RESOURCE_GROUP_NAME=BDSA2022
CONTAINER_REGISTRY_NAME=bdsa2022registry

group=$(az ad group create --display-name $SQL_GROUP_DISPLAY_NAME --mail-nickname $SQL_GROUP_NICKNAME --query id --output tsv)

az account set --subscription ondfisk-dev

az group create --name $RESOURCE_GROUP_NAME --location $LOCATION

az deployment group create \
--resource-group $RESOURCE_GROUP_NAME \
--template-file azuredeploy.bicep \
--parameters @azuredeploy.parameters.json \
--parameters sqlServerAdministratorsGroupObjectId=$group

az acr login --name $CONTAINER_REGISTRY_NAME

az acr build --image $CONTAINER_REGISTRY_NAME/api:{{.Run.ID}} --registry $CONTAINER_REGISTRY_NAME .
```
