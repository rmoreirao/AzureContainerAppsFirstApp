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


Upload image to ACR:
    az acr build --registry $ACR_NAME --image "tasksmanager/$BACKEND_API_NAME" --file 'TasksTracker.TasksManager.Backend.Api/Dockerfile' . 
    az acr build --registry $ACR_NAME --image "tasksmanager/$FRONTEND_WEBAPP_NAME" --file 'TasksTracker.TasksManager.Backend.Api/Dockerfile' . 
    az acr build --registry $ACR_NAME --image "tasksmanager/$BACKEND_SVC_NAME" --file 'FirstACAAppBackendSvc/Dockerfile' . 


Deploy to env:
    az containerapp update ` 
        --name $BACKEND_API_NAME ` 
        --resource-group $RESOURCE_GROUP ` 
        --revision-suffix v20220829-1 ` 
        --cpu 0.25 --memory 0.5Gi ` 
        --min-replicas 1 ` 
        --max-replicas 2