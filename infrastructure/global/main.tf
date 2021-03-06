provider "azurerm" {
  version = "=1.36.0"
}

variable "app_name" {
  type = "string"
}

terraform {
  backend "azurerm" {
  }
}

data "azurerm_resource_group" "rg" {
    name = "${var.app_name}-rg"
}

data "azurerm_client_config" "current" { }

resource "azurerm_container_registry" "acr" {
  name                     = "${var.app_name}registry"
  resource_group_name      = "${data.azurerm_resource_group.rg.name}"
  location                 = "${data.azurerm_resource_group.rg.location}"
  sku                      = "Premium"
  admin_enabled            = true
}

data "azuread_service_principal" "admin_user" {
  display_name = "wmp-DevOps-MovieAppTest-a94e41d4-5686-46fc-8390-e18bbbbb27cc"
}

resource "azurerm_key_vault" "kv" {
  name                        = "${var.app_name}-vault"
  location                    = "${data.azurerm_resource_group.rg.location}"
  resource_group_name         = "${data.azurerm_resource_group.rg.name}"
  enabled_for_disk_encryption = true
  tenant_id                   = "${data.azurerm_client_config.current.tenant_id}"
  sku_name                    = "standard"

  network_acls {
    default_action = "Allow"
    bypass         = "AzureServices"
  }

  access_policy {
    tenant_id       = "${data.azurerm_client_config.current.tenant_id}"
    object_id       = "${data.azuread_service_principal.admin_user.id}"

    key_permissions = [
      "create",
      "get",
    ]

    secret_permissions = [
      "set",
      "get",
      "delete",
    ]
  }

  tags = {
    environment = "global"
  }
}

// key vault secrets
resource "azurerm_key_vault_secret" "dev_key" {
  name            = "jwt-dev-key"
  value           = "${base64encode(uuid())}"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
}

resource "azurerm_key_vault_secret" "test_key" {
  name            = "jwt-test-key"
  value           = "${base64encode(uuid())}"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
}

resource "azurerm_key_vault_secret" "metric_key" {
  name            = "jwt-metric-key"
  value           = "${base64encode(uuid())}"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
}

resource "azurerm_key_vault_secret" "dev_db_pass" {
  name            = "db-dev-pass"
  value           = "${base64encode(uuid())}"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
}

resource "azurerm_key_vault_secret" "test_db_pass" {
  name            = "db-test-pass"
  value           = "${base64encode(uuid())}"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
}

resource "azurerm_key_vault_secret" "metric_db_pass" {
  name            = "db-metric-pass"
  value           = "${base64encode(uuid())}"
  key_vault_id    = "${azurerm_key_vault.kv.id}"
}