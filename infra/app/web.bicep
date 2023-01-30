param name string
param location string = resourceGroup().location
param tags object = {}
param funcAppName string

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
  name: '${name}/backend'
  properties: {
    backendResourceId: api.id
    region: location
  }
}

resource api 'Microsoft.Web/sites@2021-03-01' existing = {
  name: funcAppName
}

output SERVICE_WEB_NAME string = web.name
output SERVICE_WEB_URI string = 'https://${web.properties.defaultHostname}'
