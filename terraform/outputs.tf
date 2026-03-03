output "resource_group_name" {
  value = azurerm_resource_group.main.name
}

output "sql_server_fqdn" {
  value = module.sql.sql_server_fqdn
}

output "sql_connection_string" {
  value     = module.sql.connection_string
  sensitive = true
}

output "acr_login_server" {
  value = module.acr.login_server
}

output "aks_cluster_name" {
  value = module.aks.cluster_name
}

output "aks_kube_config" {
  value     = module.aks.kube_config
  sensitive = true
}
