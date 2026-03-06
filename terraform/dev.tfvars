# Dev environment Terraform variable overrides
resource_group_name = "flipkartapp-dev-35"
location            = "canada central"
sql_server_name     = "flipkartapp-dev-35"
sql_db_name         = "FlipkartAppDevDb35"
acr_name            = "flipkartdevacr"
aks_cluster_name    = "flipkartapp-dev-aks35"
aks_dns_prefix      = "flipkartdev35"
aks_node_count      = 2
aks_vm_size         = "Standard_B2s"

tags = {
  Project     = "FlipkartApp"
  Environment = "Dev"
  ManagedBy   = "Terraform"
}
