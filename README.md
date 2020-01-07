# DevOps Automation Example Project
*This project does not represent a complete solution. It is designed to show example and concepts that can be used in your project. Copy and Paste only when appropriate and use this for reference purposes only*

## Understanding this Project
This project is comprised of two services (AuthApi and UserApi) which communicate with each other via events transmitted through an Azure Event Grid. The design of this is such that **ALMOST EVERY** aspect of this deployment is represented in Terraform scripts. This allows for correction of drift with each deployment. The pipelines are configured so that builds will automatically kickoff whenever changes to the code for those services is made. These builds will then kick off Releases which will deploy the updates using Terraform infrastructure changes.

Alternatively, for pure Infra changes, a new release can be created which will invoke the updated scripts without a build.

The methodology being used here is known as *NoOps*, which emphasizes that the application is deployed in its entirety with each commit. This means that in addition to code changes, any infra changes must also be considered as well.

## Infrastructure
**Azure Components Used in this Sample**
- Azure Event Grid
- Azure Event Subscriptions
- Azure Functions
- Azure Key Vault
- Azure Container Registry
- Cosmos DB (MongoDB)
- SQL Azure
- Azure Blob Storage
- Azure App Service

Each of these components is stood up using Terraform and each relevant script is run with each commit to ensure that our Infra always represents the latest

## Application

### Auth Api
Deployed using an App Service, serves as the authentication broker for the system issueing JWT tokens upon User creation and Login actions. Code is hosted behind the App Serivce using a Docker container that is built and validated with each commit using Anchore Container validation, Unit tested, and OSS Dependency checking using WhiteSource.

#### Build Process
When a check in occurs the **authapi-buildspec.yaml** is executed which immediately spins off two jobs for the first stage:
- Job 1 builds an "unchecked" container and pushes it to the Container Registry. It then invokes Anchore (currently disabled) to perform security checks against the built image. If it passes the Job succeeds

- Job 2 builds the code outside of a container and executes the unit test and then performs an OSS Dependency check with WhiteSource. If these all pass the job succeeds

Once these jobs complete the second stage is initiated which rebuilds the Docker image as a "checked" image and then publishes our *testing* folder so it is available to the Release pipeline.

#### Release Process
Once the build completes, the Release pipeline will kick off with two artifacts: the build itself and the repo itself. We use the repo to pull in our Terraform infrastructure scripts. Dev will update the existing environment. Test and Metric will be stood up, executed, and then torn down.

#### Events
When a User is created the UserCreatedEvent is sent to the Azure Event Grid instance for that environment. This will be collected by the UserApi service which will create our User data in that data store (MongoDB).

### User Api
Deployed as an Azure Function App with three methods, two of which are public
- UserCreatedFunction - Invoked when the UserCreatedEvent is received from the Azure Event Grid
- Ping - Health check for the Azure Function
- GetCurrentUserFunction - Externally exposed and returns information about the current user as stored in the data store (MongoDB)

The main example being shown here is the user of the Function App as a simple API and a connector for other backend components.

#### Build Process
In order to use Containers with Azure Function apps a Premium App Service Plan must be used. The rest of the configuration for the Function App parallels what we see when deploying App Service with the exception of the **WEBSITES_ENABLE_APP_SERVICE_STORAGE** being set to false.

## Testing
### Unit Testing
Both services employe Unit Tests to verify the code level requirements, and the unit tests are executed in parallel with Docker image building. These are stored in the **.Tests** projects that are associated with each main project

#### Integration Testing
We employ integration testing in the AuthApi project using Newman. These verify the application operates as we expect when running. The Test environment is ephemeral, but we maintain the App Insights instance so any problems and data can be reviewed after the release is complete

#### Metric Testing
Artillery.io is used to employ metric testing against the AuthApi project. This is part of the accountability culture in DevOps where we want to enforce this testing early on so issues do not accrue over time. This Load Report is published with each Release

## Extra Points

### Subscription Creation
For Event Grid to send data we need subscriptions between the Event Grid and the Function App. To do this we need to extract the **master key** from the Function App, to do this we use an Azure CLI task in the Release pipeline. This is then passed to the separate Terraform scripts that create the subscription.

### Azure Key Vault
Any sensitive data is stored in an Azure Key Vault and then extracted (using Terraform) and injected into resources. Further, the **JWT Key** and **Database Passwords** are generated by Terraform and stored in the Azure Key Vault so that no one without access to List in the Azure Key Vault - this also enables a central spot for sensitive data