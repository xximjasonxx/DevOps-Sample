provider "azurerm" {
  version = "=1.36.0"
}

terraform {
  backend "azurerm" {
  }
}

resource "azurerm_storage_account" "storage" {
  name                     = "userapiartifactstorage"
  resource_group_name      = "${data.azurerm_resource_group.rg.name}"
  location                 = "${data.azurerm_resource_group.rg.location}"
  account_tier             = "Standard"
  account_replication_type = "LRS"
}

resource "azurerm_storage_container" "storage" {
  name                  = "userapi-${var.env_name}-artifacts"
  storage_account_name  = "${azurerm_storage_account.storage.name}"
  container_access_type = "private"
}

# next need to create the blob