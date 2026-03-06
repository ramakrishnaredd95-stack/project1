# # ---------------------------------------------------------------
# # Variables
# # ---------------------------------------------------------------
# variable "resource_group_name" { type = string }
# variable "location" { type = string }
# variable "keyvault_name" { type = string }
# variable "sp_object_id" { type = string }
# variable "sql_admin_login" {
#   type      = string
#   sensitive = true
# }
# variable "sql_admin_password" {
#   type      = string
#   sensitive = true
# }
# variable "tags" { type = map(string) }


# # ---------------------------------------------------------------
# # Data — current tenant for Key Vault
# # ---------------------------------------------------------------
# data "azurerm_client_config" "current" {}

# # ---------------------------------------------------------------
# # Azure Key Vault
# # ---------------------------------------------------------------
# resource "azurerm_key_vault" "main" {
#   name                       = var.keyvault_name
#   location                   = var.location
#   resource_group_name        = var.resource_group_name
#   tenant_id                  = data.azurerm_client_config.current.tenant_id
#   sku_name                   = "standard"
#   soft_delete_retention_days = 7
#   purge_protection_enabled   = false
#   enable_rbac_authorization  = true
#   tags                       = var.tags
# }

# # ---------------------------------------------------------------
# # Grant the GitHub Actions Service Principal access to KV secrets
# # Role: Key Vault Secrets Officer (can set + get secrets)
# # ---------------------------------------------------------------
# resource "azurerm_role_assignment" "sp_kv_secrets_officer" {
#   scope                = azurerm_key_vault.main.id
#   role_definition_name = "Key Vault Secrets Officer"
#   principal_id         = var.sp_object_id
# }

# # ---------------------------------------------------------------
# # Store SQL credentials as Key Vault secrets
# # ---------------------------------------------------------------
# resource "azurerm_key_vault_secret" "sql_admin_login" {
#   name         = "sql-admin-login"
#   value        = var.sql_admin_login
#   key_vault_id = azurerm_key_vault.main.id

#   depends_on = [azurerm_role_assignment.sp_kv_secrets_officer]
# }

# resource "azurerm_key_vault_secret" "sql_admin_password" {
#   name         = "sql-admin-password"
#   value        = var.sql_admin_password
#   key_vault_id = azurerm_key_vault.main.id

#   depends_on = [azurerm_role_assignment.sp_kv_secrets_officer]
# }

# # ---------------------------------------------------------------
# # Outputs
# # ---------------------------------------------------------------
# output "keyvault_id" {
#   value = azurerm_key_vault.main.id
# }

# output "keyvault_uri" {
#   value = azurerm_key_vault.main.vault_uri
# }

# output "sql_admin_login" {
#   value     = azurerm_key_vault_secret.sql_admin_login.value
#   sensitive = true
# }

# output "sql_admin_password" {
#   value     = azurerm_key_vault_secret.sql_admin_password.value
#   sensitive = true
# }
