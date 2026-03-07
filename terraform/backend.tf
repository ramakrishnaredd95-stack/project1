terraform {
  backend "azurerm" {
    resource_group_name  = "rsg-storage-account"
    storage_account_name = "storageaccount9802"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }
}
