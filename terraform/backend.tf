terraform {
  backend "azurerm" {
    resource_group_name  = "flipkart-tfstate-rg"
    storage_account_name = "flipkarttfstate124"
    container_name       = "tfstate"
    key                  = "terraform.tfstate"
  }
}
