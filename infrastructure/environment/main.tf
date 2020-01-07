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

data "azurerm_key_vault" "kv" {
  name                = "${var.app_name}-vault"
  resource_group_name = "${data.azurerm_resource_group.rg.name}"
}

data "azurerm_key_vault_secret" "db_pass" {
  name            = "db-${var.env_name}-pass"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
}

resource "azurerm_sql_server" "sql" {
  name                          = "${var.app_name}-${var.env_name}-sqlserver"
  resource_group_name           = "${data.azurerm_resource_group.rg.name}"
  location                      = "${data.azurerm_resource_group.rg.location}"  
  version                       = "12.0"
  administrator_login           = "${var.env_name}-admin"
  administrator_login_password  = "${data.azurerm_key_vault_secret.db_pass.value}"

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

resource "azurerm_key_vault_secret" "topic_key" {
  name            = "${var.env_name}-topic-access-key"
  value           = "${azurerm_eventgrid_topic.topic.primary_access_key}"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
}

resource "azurerm_key_vault_secret" "topic_endpoint" {
  name            = "${var.env_name}-topic-endpoint"
  value           = "${azurerm_eventgrid_topic.topic.endpoint}"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
}

resource "azurerm_key_vault_secret" "topic_id" {
  name            = "${var.env_name}-topic-id"
  value           = "${azurerm_eventgrid_topic.topic.id}"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
}

resource "azurerm_key_vault_secret" "mongo_connection" {
  name            = "${var.env_name}-mongo-connection"
  value           = "${azurerm_cosmosdb_account.db.connection_strings.value[0]}"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
}