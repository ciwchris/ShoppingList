param name string
param location string = resourceGroup().location
param tags object = {}
param apimName string

param sku object = {
  name: 'Standard'
  tier: 'Standard'
}

resource web 'Microsoft.Web/staticSites@2022-03-01' = {
  name: name
  location: location
  tags: union(tags, { 'azd-service-name': 'web' })
  sku: sku
  properties: {
    provider: 'Custom'
  }
}

resource staticWebAppBackend 'Microsoft.Web/staticSites/linkedBackends@2022-03-01' = {
  parent: web
  name: 'backend'
  kind: 'apim'
  properties: {
    backendResourceId: apimService.id
    region: location
  }
}

resource apimService 'Microsoft.ApiManagement/service@2021-08-01' existing = {
  name: apimName
}

output SERVICE_WEB_NAME string = web.name
output SERVICE_WEB_URI string = 'https://${web.properties.defaultHostname}'
output APIM_PRODUCT_NAME string = substring(web.properties.defaultHostname, 0, indexOf(web.properties.defaultHostname, '.'))
