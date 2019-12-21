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

resource "azurerm_sql_server" "sql" {
  name                          = "${var.app_name}-${var.env_name}-sqlserver"
  resource_group_name           = "${data.azurerm_resource_group.rg.name}"
  location                      = "${data.azurerm_resource_group.rg.location}"  
  version                       = "12.0"
  administrator_login           = "${var.env_name}-admin"
  administrator_login_password  = "Password01!"

  tags = {
    environment = "${var.env_name}"
  }
}

resource "azurerm_sql_firewall_rule" "firewall" {
  name                = "${var.app_name}-${var.env_name}-sqlserver-azure-fw-rule"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
  server_name         = "${azurerm_sql_server.sql.name}"
  start_ip_address    = "0.0.0.0"
  end_ip_address      = "0.0.0.0"
}

resource "azurerm_cosmosdb_account" "db" {
  name                      = "${var.app_name}-${var.env_name}-cosmos-account"
  location                  = "${data.azurerm_resource_group.rg.location}"
  resource_group_name       = "${data.azurerm_resource_group.rg.name}"
  offer_type                = "Standard"
  kind                      = "MongoDB"
  
  enable_automatic_failover = false
  consistency_policy {
    consistency_level       = "BoundedStaleness"
    max_interval_in_seconds = 10
    max_staleness_prefix    = 200
  }

  geo_location {
    location                = "EastUS"
    failover_priority       = 0
  }
}

resource "azurerm_application_insights" "insights" {
  name                          = "${var.app_name}-${var.env_name}-insights"
  resource_group_name           = "${data.azurerm_resource_group.rg.name}"
  location                      = "${data.azurerm_resource_group.rg.location}"
  application_type              = "web"
}

resource "azurerm_app_service_plan" "plan" {
  name                = "${var.app_name}-${var.env_name}-ConsumptionPlan"
  location            = "${data.azurerm_resource_group.rg.location}"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
  kind                = "Linux"
  reserved            = true
  sku {
    tier = "${var.env_name == "prod" || var.env_name == "metric" ? "Standard" : "Basic"}"
    size = "${var.env_name == "prod" || var.env_name == "metric" ? "S1" : "B1"}"
  }
}

output "topic_access_key" {
  value = "${azurerm_eventgrid_topic.topic.primary_access_key}"
}

output "topic_endpoint" {
  value = "${azurerm_eventgrid_topic.topic.endpoint}"
}

output "topic_id" {
  value = "${azurerm_eventgrid_topic.topic.id}"
}
