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

data "azurerm_resource_group" "rg" {
    name = "${var.app_name}-rg"
}

data "azurerm_container_registry" "registry" {
  name = "${var.app_name}registry"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
}

resource "azurerm_app_service_plan" "plan" {
  name = "AuthApiPreProd_ConsumptionPlan"
  location = "${data.azurerm_resource_group.rg.location}"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
  kind = "Linux"
  reserved = true
  sku {
    tier = "Basic"
    size = "B1"
  }
}

resource "azurerm_app_service" "authapi" {
  name                = "${var.app_name}-authapi-dev"
  location            = "${data.azurerm_resource_group.rg.location}"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
  app_service_plan_id = "${azurerm_app_service_plan.plan.id}"

  app_settings = {
    WEBSITES_ENABLE_APP_SERVICE_STORAGE = false
    DOCKER_REGISTRY_SERVER_URL = "${data.azurerm_container_registry.registry.login_server}"
    DOCKER_REGISTRY_SERVER_USERNAME = "${data.azurerm_container_registry.registry.admin_username}"
    DOCKER_REGISTRY_SERVER_PASSWORD = "${data.azurerm_container_registry.registry.admin_password}"
  }

  site_config {
    linux_fx_version = "DOCKER|${data.azurerm_container_registry.registry.login_server}/authapi:${var.container_id}"
    always_on = "true"
  }

  identity {
    type = "SystemAssigned"
  }
}