variable "resource_group_name" {
  description = "Name of the Azure Resource Group"
  type        = string
  default     = "flipkartapp-rg"
}

variable "location" {
  description = "Azure region for all resources"
  type        = string
  default     = "East US"
}

variable "sql_server_name" {
  description = "Name of the Azure SQL Server"
  type        = string
  default     = "flipkartapp-sqlserver"
}

variable "sql_db_name" {
  description = "Name of the Azure SQL Database"
  type        = string
  default     = "FlipkartAppDb"
}

variable "sql_admin_login" {
  description = "SQL Server admin login"
  type        = string
  default     = "flipkartadmin"
  sensitive   = true
}

variable "sql_admin_password" {
  description = "SQL Server admin password"
  type        = string
  sensitive   = true
}

variable "acr_name" {
  description = "Name of the Azure Container Registry"
  type        = string
  default     = "flipkartacr"
}

variable "aks_cluster_name" {
  description = "Name of the AKS cluster"
  type        = string
  default     = "flipkartapp-aks"
}

variable "aks_dns_prefix" {
  description = "DNS prefix for AKS cluster"
  type        = string
  default     = "flipkartapp"
}

variable "aks_node_count" {
  description = "Number of AKS nodes"
  type        = number
  default     = 3
}

variable "aks_vm_size" {
  description = "VM size for AKS nodes"
  type        = string
  default     = "Standard_B2s"
}

# variable "keyvault_name" {
#   description = "Name of the Azure Key Vault (must be globally unique)"
#   type        = string
#   default     = "flipkart-kv"
# }

variable "tags" {
  description = "Tags for all resources"
  type        = map(string)
  default = {
    Project     = "FlipkartApp"
    Environment = "Production"
    ManagedBy   = "Terraform"
  }
}
