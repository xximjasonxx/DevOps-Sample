provider "azurerm" {
  version = "=1.36.0"
}

terraform {
  backend "azurerm" {
  }
}

variable "env_name" {
  type = "string"
}

variable "topic_id" {
  type = "string"
}

variable "app_url" {
  type = "string"
}

variable "masterKey" {
  type = "string"
}


resource "azurerm_eventgrid_event_subscription" "default" {
  name                  = "userCreated-${var.env_name}-subscription"
  scope                 = "${var.topic_id}"
  event_delivery_schema = "EventGridSchema"
  included_event_types  = [ "UserCreatedEvent" ]

  webhook_endpoint {
    url = "https://${var.app_url}/runtime/webhooks/EventGrid?functionName=UserCreatedFunction&code=${var.masterKey}"
  }
}