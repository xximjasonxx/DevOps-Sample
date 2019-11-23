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

variable "artifact_path" {
  type = "string"
}


data "azurerm_resource_group" "rg" {
  name     = "${var.app_name}-rg"
}

resource "azurerm_storage_account" "storage" {
  name                     = "userapiartifactstorage"
  resource_group_name      = "${data.azurerm_resource_group.rg.name}"
  location                 = "${data.azurerm_resource_group.rg.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "storage" {
  name                  = "userapi-artifacts"
  storage_account_name  = "${azurerm_storage_account.storage.name}"
  container_access_type = "private"
}

resource "azurerm_storage_blob" "blob" {
  name                    = "${basename(var.artifact_path)}"
  storage_container_name  = "${azurerm_storage_container.storage.name}"
  type                    = "Block"
  source                  = "${var.artifact_path}"
}