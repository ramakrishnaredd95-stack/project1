variable "resource_group_name" {
  type        = string
  description = "Name of the resource group"
}

variable "location" {
  type        = string
  description = "Azure location"
}

variable "keyvault_name" {
  type        = string
  description = "Name of the Key Vault"
}

variable "sp_object_id" {
  type        = string
  description = "Service Principal Object ID for RBAC"
}

variable "sql_admin_login" {
  type      = string
  sensitive = true
}

variable "sql_admin_password" {
  type      = string
  sensitive = true
}

variable "tags" {
  type = map(string)
}
