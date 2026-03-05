# Dev environment Terraform variable overrides
resource_group_name = "flipkartapp-dev-rg"
location            = "canda center"
sql_server_name     = "flipkartapp-dev-121"
sql_db_name         = "FlipkartAppDevDb121"
acr_name            = "flipkartdevacr"
aks_cluster_name    = "flipkartapp-dev-aks132"
aks_dns_prefix      = "flipkartdev132"
aks_node_count      = 2
aks_vm_size         = "Standard_B2s"

tags = {
  Project     = "FlipkartApp"
  Environment = "Dev"
  ManagedBy   = "Terraform"
}
