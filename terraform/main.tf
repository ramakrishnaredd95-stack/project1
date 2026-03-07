terraform {
  required_version = ">= 1.5.0"

  required_providers {
    azurerm = {
      source  = "hashicorp/azurerm"
      version = "~> 3.80"
    }
  }

}

provider "azurerm" {
  features {}
}

# Fetch current authenticated SP / user details
data "azurerm_client_config" "current" {}

# Resource Group
resource "azurerm_resource_group" "main" {
  name     = var.resource_group_name
  location = var.location
  tags     = var.tags
}

# ---------------------------------------------------------------
# Key Vault Module
# Provisions the vault and stores SQL credentials as secrets
# ---------------------------------------------------------------
module "keyvault" {
  source              = "./modules/keyvault"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  keyvault_name       = var.keyvault_name
  sp_object_id        = data.azurerm_client_config.current.object_id
  sql_admin_login     = var.sql_admin_login
  sql_admin_password  = var.sql_admin_password
  tags                = var.tags
}

# ---------------------------------------------------------------
# Azure SQL Module
# Reads credentials from Key Vault outputs (not raw vars)
# ---------------------------------------------------------------
module "sql" {
  source              = "./modules/sql"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  sql_server_name     = var.sql_server_name
  sql_db_name         = var.sql_db_name
  sql_admin_login     = module.keyvault.sql_admin_login
  sql_admin_password  = module.keyvault.sql_admin_password
  tags                = var.tags

  depends_on = [module.keyvault]
}

# Azure Container Registry Module
module "acr" {
  source              = "./modules/acr"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  acr_name            = var.acr_name
  tags                = var.tags
}

# Azure Kubernetes Service Module
module "aks" {
  source              = "./modules/aks"
  resource_group_name = azurerm_resource_group.main.name
  location            = azurerm_resource_group.main.location
  cluster_name        = var.aks_cluster_name
  dns_prefix          = var.aks_dns_prefix
  node_count          = var.aks_node_count
  vm_size             = var.aks_vm_size
  acr_id              = module.acr.acr_id
  tags                = var.tags
}
