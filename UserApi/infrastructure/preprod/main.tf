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

variable "artifact_name" {
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

data "azurerm_storage_account" "storage" {
  name = "userapiartifactstorage"
}

data "azurerm_storage_account_sas" "storage_sas" {
 connection_string = "${data.azurerm_storage_account.storage.primary_connection_string}"
 https_only = false

 resource_types {
  service = false
  container = false
  object = true
 }
 
 services {
  blob = true
  queue = false
  table = false
  file = false
 }
 
 start = "2019–11–21"
 expiry = "2028–03–21"
 
 permissions {
  read = true
  write = false
  delete = false
  list = false
  add = false
  create = false
  update = false
  process = false
 }
}

resource "azurerm_function_app" "funcApp" {
    name                       = "userapi-${var.app_name}fa-${var.env_name}"
    location                   = "${data.azurerm_resource_group.rg.location}"
    resource_group_name        = "${data.azurerm_resource_group.rg.name}"
    app_service_plan_id        = "${data.azurerm_app_service_plan.plan.id}"
    storage_connection_string  = "${azurerm_storage_account.storage.primary_connection_string}"
    version                    = "~2"

    app_settings = {
        FUNCTIONS_WORKER_RUNTIME                  = "dotnet"
        WEBSITE_CONTENTAZUREFILECONNECTIONSTRING  = "${azurerm_storage_account.storage.primary_connection_string}"
        WEBSITE_CONTENTSHARE                      = "${azurerm_storage_account.storage.name}"
        FUNCTION_APP_EDIT_MODE                    = "readOnly"
        https_only                                = true

        WEBSITE_RUN_FROM_PACKAGE                  = "${data.azurerm_storage_account.storage.primary_blob_endpoint}userapi-artifacts/${var.artifact_name}${data.azurerm_storage_account_sas.storage_sas.sas}"
    }
}