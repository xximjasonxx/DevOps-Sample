provider "azurerm" {
  version = "=1.36.0"
}

variable "app_name" {
  type = "string"
}


resource "azurerm_resource_group" "rg" {
  name     = "${var.app_name}-rg"
  location = "Central US"
}

resource "azurerm_storage_account" "stg" {
  name                     = "${var.app_name}state"
  resource_group_name      = "${azurerm_resource_group.rg.name}"
  location                 = "${azurerm_resource_group.rg.location}"
  account_tier             = "Standard"
  account_replication_type = "GRS"
}

resource "azurerm_storage_container" "con" {
  name                  = "tfstate"
  storage_account_name  = "${azurerm_storage_account.stg.name}"
  container_access_type = "private"
}