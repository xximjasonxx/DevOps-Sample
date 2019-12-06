provider "azurerm" {
  version = "=1.36.0"
}

terraform {
  backend "azurerm" {
  }
}

variable "app_name" {
  type = "string"
}


variable "container_id" {
  type = "string"
}

variable "env_name" {
  type = "string"
}

variable "jwt_key" {
  type = "string"
}

variable "eg_topic_endpoint" {
  type = "string"
}

variable "eg_access_key" {
  type = "string" 
}


data "azurerm_resource_group" "rg" {
  name = "${var.app_name}-rg"
}

data "azurerm_container_registry" "registry" {
  name = "${var.app_name}registry"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
}

data "azurerm_sql_server" "sql" {
  name                          = "${var.app_name}-${var.env_name}-sqlserver"
  resource_group_name           = "${data.azurerm_resource_group.rg.name}"
}

resource "azurerm_sql_database" "database" {
  name                = "${var.app_name}-auth-db"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
  location            = "${data.azurerm_resource_group.rg.location}"
  server_name         = "${data.azurerm_sql_server.sql.name}"

  tags = {
    environment = "${var.env_name}"
  }
}

data "azurerm_app_service_plan" "plan" {
  name                = "${var.app_name}-${env_name}-ConsumptionPlan"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
}

data "azurerm_application_insights" "insights" {
  name                          = "${var.app_name}-${var.env_name}-insights"
  resource_group_name           = "${data.azurerm_resource_group.rg.name}"
}

resource "azurerm_app_service" "authapi" {
  name                = "${var.app_name}-authapi-${var.env_name}"
  location            = "${data.azurerm_resource_group.rg.location}"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
  app_service_plan_id = "${data.azurerm_app_service_plan.plan.id}"

  app_settings = {
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = false
    DOCKER_REGISTRY_SERVER_URL      = "${data.azurerm_container_registry.registry.login_server}"
    DOCKER_REGISTRY_SERVER_USERNAME = "${data.azurerm_container_registry.registry.admin_username}"
    DOCKER_REGISTRY_SERVER_PASSWORD = "${data.azurerm_container_registry.registry.admin_password}"
    ConnectionString                = "Server=${data.azurerm_sql_server.sql.fully_qualified_domain_name};Database=${data.azurerm_sql_database.database.name};User Id=${data.azurerm_sql_server.sql.administrator_login};Password=${data.azurerm_sql_server.sql.administrator_login_password};MultipleActiveResultSets=True;Connection Timeout=60",
    JwtKey                          = "${var.jwt_key}"
    JwtIssuer                       = "${var.app_name}"
    JwtAudience                     = "${var.app_name}-${var.env_name}"
    APPINSIGHTS_INSTRUMENTATIONKEY  = "${azurerm_application_insights.insights.instrumentation_key}"
    ASPNETCORE_ENVIRONMENT          = "${var.env_name}"
    EventTopicEndpoint              = "${var.eg_topic_endpoint}"
    EventTopicAccessKey             = "${var.eg_access_key}"
  }

  site_config {
    linux_fx_version = "DOCKER|${data.azurerm_container_registry.registry.login_server}/authapi:${var.container_id}"
    always_on = "true"
  }

  identity {
    type = "SystemAssigned"
  }

  tags = {
    environment = "${var.env_name}"
  }
}

output "appservice_url" {
  value = "${azurerm_app_service.authapi.default_site_hostname}"
}