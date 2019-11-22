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

data "azurerm_resource_group" "rg" {
  name     = "${var.app_name}-rg"
}

resource "azurerm_storage_account" "storage" {
  name                     = "userapi${var.env_name}storage"
  resource_group_name      = "${data.azurerm_resource_group.rg.name}"
  location                 = "${data.azurerm_resource_group.rg.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

data "azurerm_app_service_plan" "plan" {
  name                = "${var.app_name}-PreProd-ConsumptionPlan"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
}

data "azurerm_container_registry" "registry" {
  name = "${var.app_name}registry"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
}

resource "azurerm_function_app" "funcApp" {
    name                       = "userapi${var.env_name}${var.app_name}fa"
    location                   = "WestUS"
    resource_group_name        = "${data.azurerm_resource_group.rg.name}"
    app_service_plan_id        = "${data.azurerm_app_service_plan.plan.id}"
    storage_connection_string  = "${azurerm_storage_account.storage.primary_connection_string}"
    version                    = "~2"

    app_settings = {
        FUNCTIONS_WORKER_RUNTIME                  = "dotnet"
        WEBSITE_CONTENTAZUREFILECONNECTIONSTRING  = "${azurerm_storage_account.storage.primary_connection_string}"
        WEBSITE_CONTENTSHARE                      = "${azurerm_storage_account.storage.name}"
        FUNCTION_APP_EDIT_MODE                    = "readOnly"
    }
}