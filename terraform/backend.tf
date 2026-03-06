terraform {
  backend "azurerm" {
    resource_group_name  = "rg-storage-account"
    storage_account_name = "flipkarttfstate126"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }
}
