provider "azurerm" {
  version = "=1.36.0"
}

variable "app_name" {
  type = "string"
}


terraform {
  backend "azurerm" {
    resource_group_name  = "movieappwmp-rg"
    storage_account_name = "movieappwmpstate"
    container_name       = "tfstate"
    key                  = "global-state"
  }
}

data "azurerm_resource_group" "rg" {
    name = "${var.app_name}-rg"
}

resource "azurerm_container_registry" "acr" {
  name                     = "${var.app_name}registry"
  resource_group_name      = "${data.azurerm_resource_group.rg.name}"
  location                 = "${data.azurerm_resource_group.rg.location}"
  sku                      = "Premium"
  admin_enabled            = false
}