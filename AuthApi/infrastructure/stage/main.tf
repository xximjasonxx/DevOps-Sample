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

data "azurerm_resource_group" "rg" {
    name = "${var.app_name}-rg"
}

data "azurerm_container_registry" "registry" {
  name = "${var.app_name}registry"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
}

resource "azurerm_sql_server" "sql" {
  name                          = "${var.app_name}-${var.env_name}-sqlserver"
  resource_group_name           = "${data.azurerm_resource_group.rg.name}"
  location                      = "${data.azurerm_resource_group.rg.location}"  
  version                       = "12.0"
  administrator_login           = "${var.env_name}-admin"
  administrator_login_password  = "Password01!"

  tags = {
    environment = "${var.env_name}"
  }
}

resource "azurerm_sql_firewall_rule" "firewall" {
  name                = "${var.app_name}-${var.env_name}-sqlserver-azure-fw-rule"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
  server_name         = "${azurerm_sql_server.sql.name}"
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

resource "azurerm_sql_database" "database" {
  name                = "${var.app_name}-auth-db"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
  location            = "${data.azurerm_resource_group.rg.location}"
  server_name         = "${azurerm_sql_server.sql.name}"

  tags = {
    environment = "${var.env_name}"
  }
}

resource "azurerm_app_service_plan" "plan" {
  name                = "${var.app_name}-Stage-ConsumptionPlan"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
  location            = "${data.azurerm_resource_group.rg.location}"
  kind                = "Linux"
  reserved            = true
  sku {
    tier = "Standard"
    size = "S1"
  }
}

resource "azurerm_application_insights" "insights" {
  name                          = "${var.app_name}-${var.env_name}-insights"
  resource_group_name           = "${data.azurerm_resource_group.rg.name}"
  location                      = "${data.azurerm_resource_group.rg.location}"
  application_type              = "web"
}

resource "azurerm_app_service" "authapi" {
  name                = "${var.app_name}-authapi-${var.env_name}"
  location            = "${data.azurerm_resource_group.rg.location}"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
  app_service_plan_id = "${azurerm_app_service_plan.plan.id}"

  app_settings = {
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = false
    DOCKER_REGISTRY_SERVER_URL      = "${data.azurerm_container_registry.registry.login_server}"
    DOCKER_REGISTRY_SERVER_USERNAME = "${data.azurerm_container_registry.registry.admin_username}"
    DOCKER_REGISTRY_SERVER_PASSWORD = "${data.azurerm_container_registry.registry.admin_password}"
    ConnectionString                = "Server=${azurerm_sql_server.sql.fully_qualified_domain_name};Database=${azurerm_sql_database.database.name};User Id=${azurerm_sql_server.sql.administrator_login};Password=${azurerm_sql_server.sql.administrator_login_password};MultipleActiveResultSets=True",
    JwtKey                          = "${var.jwt_key}"
    JwtIssuer                       = "${var.app_name}"
    JwtAudience                     = "${var.app_name}-${var.env_name}"
    APPINSIGHTS_INSTRUMENTATIONKEY  = "${azurerm_application_insights.insights.instrumentation_key}"
    ASPNETCORE_ENVIRONMENT          = "${var.env_name}"
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