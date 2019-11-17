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
  admin_enabled            = true
}

resource "azurerm_app_service_plan" "plan" {
  name                = "${var.app_name}-PreProd-ConsumptionPlan"
  location            = "${data.azurerm_resource_group.rg.location}"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
  kind                = "Linux"
  reserved            = true
  sku {
    tier = "Basic"
    size = "B1"
  }
}

resource "azurerm_application_insights" "metric_insights" {
  name                          = "${var.app_name}-metric-insights"
  resource_group_name           = "${data.azurerm_resource_group.rg.name}"
  location                      = "${data.azurerm_resource_group.rg.location}"
  application_type              = "web"
}