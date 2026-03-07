terraform {
  backend "azurerm" {
    resource_group_name  = "rsg-storage-account"
    storage_account_name = "flipkarttfstate356"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }
}
