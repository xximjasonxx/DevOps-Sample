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

variable "app_url" {
  type = "string"
}

variable "masterKey" {
  type = "string"
}

data "azurerm_resource_group" "rg" {
  name     = "${var.app_name}-rg"
}

data "azurerm_key_vault" "kv" {
  name                = "${var.app_name}-vault"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
}

data "azurerm_key_vault_secret" "topic_id" {
  name            = "${var.env_name}-topic-id"
  key_vault_id    = "${data.azurerm_key_vault.kv.id}"
}

resource "azurerm_eventgrid_event_subscription" "default" {
  name                  = "userapi-userCreated-${var.env_name}-subscription"
  scope                 = "${data.azurerm_key_vault_secret.topic_id.value}"
  event_delivery_schema = "EventGridSchema"
  included_event_types  = [ "UserCreatedEvent" ]

  webhook_endpoint {
    url = "${var.app_url}/runtime/webhooks/EventGrid?functionName=UserCreatedFunction&code=${var.masterKey}"
  }
}