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
    name = "${var.app_name}-rg"
}

resource "azurerm_eventgrid_topic" "topic" {
    name                = "${var.app_name}-${var.env_name}-topic"
    location            = "${data.azurerm_resource_group.rg.location}"
    resource_group_name = "${data.azurerm_resource_group.rg.name}"

    tags = {
        environment = "${var.env_name}"
    }
}

output "topic_access_key" {
  value = "${azurerm_eventgrid_topic.topic.primary_access_key}"
}

output "topic_endpoint" {
  value = "${azurerm_eventgrid_topic.topic.endpoint}"
}