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

data "azurerm_key_vault" "kv" {
  name                = "${var.app_name}-vault"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
}

data "azurerm_key_vault_secret" "secret_key" {
  name            = "jwt-${var.env_name}-key"
  key_vault_id    = "${data.azurerm_key_vault.kv.id}"
}

data "azurerm_key_vault_secret" "topic_key" {
  name            = "${var.env_name}-topic-access-key"
  key_vault_id    = "${data.azurerm_key_vault.kv.id}"
}

data "azurerm_key_vault_secret" "topic_endpoint" {
  name            = "${var.env_name}-topic-endpoint"
  key_vault_id    = "${data.azurerm_key_vault.kv.id}"
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
  name                = "${var.app_name}-${var.env_name}-ConsumptionPlan"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
}

data "azurerm_application_insights" "insights" {
  name                          = "${var.app_name}-${var.env_name}-insights"
  resource_group_name           = "${data.azurerm_resource_group.rg.name}"
}

data "azurerm_key_vault_secret" "db_pass" {
  name            = "db-${var.env_name}-pass"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
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
    ConnectionString                = "Server=${data.azurerm_sql_server.sql.fqdn};Database=${azurerm_sql_database.database.name};User Id=${data.azurerm_sql_server.sql.administrator_login};Password=${data.azurerm_key_vault_secret.db_pass.value};MultipleActiveResultSets=True;Connection Timeout=60",
    JwtKey                          = "${data.azurerm_key_vault_secret.secret_key.value}"
    JwtIssuer                       = "${var.app_name}"
    JwtAudience                     = "${var.app_name}-${var.env_name}"
    APPINSIGHTS_INSTRUMENTATIONKEY  = "${data.azurerm_application_insights.insights.instrumentation_key}"
    ASPNETCORE_ENVIRONMENT          = "${var.env_name}"
    EventTopicEndpoint              = "${data.azurerm_key_vault_secret.topic_endpoint.value}"
    EventTopicAccessKey             = "${data.azurerm_key_vault_secret.topic_key.value}"
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