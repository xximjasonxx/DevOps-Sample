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

variable "env_name" {
  type = "string"
}

variable "image_name" {
  type = "string"
}

variable "tag" {
  type = "string"
}

variable "connectionString" {
  type = "string"
}

variable "jwt_key" {
  type = "string"
}

data "azurerm_resource_group" "rg" {
  name     = "${var.app_name}-rg"
}

data "azurerm_container_registry" "registry" {
  name                  = "${var.app_name}registry"
  resource_group_name   = "${data.azurerm_resource_group.rg.name}"
}

data "azurerm_cosmosdb_account" "db" {
  name                  = "${var.app_name}-${var.env_name}-cosmos-account"
  resource_group_name   = "${data.azurerm_resource_group.rg.name}"
}

resource "azurerm_storage_account" "storage" {
  name                     = "userapi${var.env_name}storage"
  resource_group_name      = "${data.azurerm_resource_group.rg.name}"
  location                 = "${data.azurerm_resource_group.rg.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_app_service_plan" "plan" {
  name                    = "${var.app_name}-premiumPlan"
  resource_group_name     = "${data.azurerm_resource_group.rg.name}"
  location                = "${data.azurerm_resource_group.rg.location}"
  kind                    = "Linux"
  reserved                = true

  sku {
    tier = "Premium"
    size = "P1V2"
  }
}

resource "azurerm_function_app" "funcApp" {
    name                       = "userapi-${var.app_name}fa-${var.env_name}"
    location                   = "${data.azurerm_resource_group.rg.location}"
    resource_group_name        = "${data.azurerm_resource_group.rg.name}"
    app_service_plan_id        = "${azurerm_app_service_plan.plan.id}"
    storage_connection_string  = "${azurerm_storage_account.storage.primary_connection_string}"
    version                    = "~2"

    app_settings = {
        FUNCTION_APP_EDIT_MODE                    = "readOnly"
        https_only                                = true
        DOCKER_REGISTRY_SERVER_URL                = "${data.azurerm_container_registry.registry.login_server}"
        DOCKER_REGISTRY_SERVER_USERNAME           = "${data.azurerm_container_registry.registry.admin_username}"
        DOCKER_REGISTRY_SERVER_PASSWORD           = "${data.azurerm_container_registry.registry.admin_password}"
        WEBSITES_ENABLE_APP_SERVICE_STORAGE       = false
        ConnectionString                          = "${var.connectionString}"
        DatabaseName                              = "${var.app_name}-userdb"
        JwtKey                                    = "${var.jwt_key}"
        JwtIssuer                                 = "${var.app_name}"
        JwtAudience                               = "${var.app_name}-${var.env_name}"
    }

    site_config {
      always_on                 = true
      linux_fx_version          = "DOCKER|${data.azurerm_container_registry.registry.login_server}/${var.image_name}:${var.tag}"
    }
}

output "funcapp_url" {
  value = "https://${azurerm_function_app.funcApp.default_hostname}"
}