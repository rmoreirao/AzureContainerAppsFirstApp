# AzureContainerAppsFirstApp
AzureContainerAppsFirstApp

https://bitoftech.net/2022/08/29/dapr-integration-with-azure-container-apps/


dapr:
    Client> dapr run --app-id tasksmanager-frontend-webapp --app-port 7219 --dapr-http-port 3501 --app-ssl dotnet run 

    Api> dapr run --app-id tasksmanager-backend-api --app-port 7218 --dapr-http-port 3500 --app-ssl --resources-path ../../../components dotnet run

    Pub Sub Backend > dapr run --app-id tasksmanager-backend-processor --app-port 7220 --dapr-http-port 3502 --app-ssl --resources-path ../../../components dotnet run

Azure CLI Env Properties:

$RESOURCE_GROUP="tasks-tracker-rg"
$LOCATION="eastus"
$ENVIRONMENT="tasks-tracker-containerapps-env"
$BACKEND_API_NAME="tasksmanager-backend-api"
$FRONTEND_WEBAPP_NAME="tasksmanager-frontend-webapp"
$BACKEND_SVC_NAME="tasksmanager-backend-processor"
$ACR_NAME="taskstrackeracrrmoreirao"
$NamespaceName="taskstrackerrmoreirao"
$TopicName="tasksavedtopic"
$STORAGE_ACCOUNT_NAME = "firstacastgrmoreirao"
$workspace = "/subscriptions/afb8f550-216d-4848-b6f1-73b1bbf58f1e/resourcegroups/tasks-tracker-rg/providers/microsoft.operationalinsights/workspaces/workspace-taskstrackerrgLM5w"


Upload image to ACR:
    az acr build --registry $ACR_NAME --image "tasksmanager/$BACKEND_API_NAME" --file 'FirstACAAppWebAPI/Dockerfile' . 
    az acr build --registry $ACR_NAME --image "tasksmanager/$FRONTEND_WEBAPP_NAME" --file 'FirstACAAppClient/Dockerfile' . 
    az acr build --registry $ACR_NAME --image "tasksmanager/$BACKEND_SVC_NAME" --file 'FirstACAAppBackendSvc/Dockerfile' . 


Deploy to env - Increment Revision for each deploy:
    $RevisionSuffix="v011"

    az containerapp update --name $BACKEND_API_NAME --resource-group $RESOURCE_GROUP --revision-suffix $RevisionSuffix --cpu 0.25 --memory 0.5Gi --min-replicas 1 --max-replicas 3

    az containerapp update --name $FRONTEND_WEBAPP_NAME --resource-group $RESOURCE_GROUP --revision-suffix $RevisionSuffix --cpu 0.25 --memory 0.5Gi --min-replicas 1 --max-replicas 3

    az containerapp update --name $BACKEND_SVC_NAME --resource-group $RESOURCE_GROUP --revision-suffix $RevisionSuffix --cpu 0.25 --memory 0.5Gi --min-replicas 1 --max-replicas 3
        