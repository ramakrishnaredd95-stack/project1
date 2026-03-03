variable "resource_group_name" { type = string }
variable "location" { type = string }
variable "acr_name" { type = string }
variable "tags" { type = map(string) }

resource "azurerm_container_registry" "main" {
  name                = var.acr_name
  resource_group_name = var.resource_group_name
  location            = var.location
  sku                 = "Standard"
  admin_enabled       = true
  tags                = var.tags
}

output "acr_id" {
  value = azurerm_container_registry.main.id
}

output "login_server" {
  value = azurerm_container_registry.main.login_server
}
