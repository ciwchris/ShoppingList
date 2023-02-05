targetScope = 'subscription'

@minLength(1)
@maxLength(64)
@description('Name of the the environment which is used to generate a short unique hash used in all resources.')
param environmentName string

@minLength(1)
@description('Primary location for all resources')
param location string

param applicationInsightsName string = ''
param apiServiceName string = ''
param resourceGroupName string = ''
param storageAccountName string = ''
param webServiceName string = ''
param apimProductApiName string = 'shopping-list-api'

var abbrs = loadJsonContent('./abbreviations.json')
var resourceToken = toLower(uniqueString(subscription().id, environmentName, location))
var tags = { 'azd-env-name': environmentName }

// Organize resources in a resource group
resource rg 'Microsoft.Resources/resourceGroups@2021-04-01' = {
  name: !empty(resourceGroupName) ? resourceGroupName : '${abbrs.resourcesResourceGroups}${environmentName}'
  location: location
  tags: tags
}

// The application frontend
module web './app/web.bicep' = {
  name: 'web'
  scope: rg
  params: {
    name: !empty(webServiceName) ? webServiceName : '${abbrs.webStaticSites}web-${resourceToken}'
    location: location
    tags: tags
    apimName: apim.outputs.apimServiceName
  }
}

// Link application frontend to APIM
module apimProductApis './app/apim-product-apis.bicep' = {
  name: 'apimProductApis'
  scope: rg
  params: {
    apimName: apim.outputs.apimServiceName
    apimProductName: web.outputs.APIM_PRODUCT_NAME
    apimProductApiName: apimProductApiName
  }
}

// The application backend
module api './app/api.bicep' = {
  name: 'api'
  scope: rg
  params: {
    name: !empty(apiServiceName) ? apiServiceName : '${abbrs.webSitesFunctions}api-${resourceToken}'
    location: location
    tags: tags
    storageAccountName: storage.outputs.name
    applicationInsightsName: applicationInsights.outputs.name
  }
}

// Backing storage for Azure functions backend API
module storage './app/storage.bicep' = {
  name: 'storage'
  scope: rg
  params: {
    name: !empty(storageAccountName) ? storageAccountName : '${abbrs.storageStorageAccounts}${resourceToken}'
    location: location
    tags: tags
  }
}

// Monitor application with Azure Monitor
module applicationInsights './app/applicationInsights.bicep' = {
  name: 'applicationInsights'
  scope: rg
  params: {
    location: location
    tags: tags
    name: !empty(applicationInsightsName) ? applicationInsightsName : '${abbrs.insightsComponents}${resourceToken}'
  }
}

// Creates Azure API Management (APIM) service to mediate the requests between the frontend and the backend API
module apim './app/apim.bicep' = {
  name: 'apim-deployment'
  scope: rg
  params: {
    name: '${abbrs.apiManagementService}${resourceToken}'
    location: location
    tags: tags
    applicationInsightsName: applicationInsights.outputs.name
  }
}

// Configures the API in the Azure API Management (APIM) service
module apimApi './app/apim-api.bicep' = {
  name: 'apim-api-deployment'
  scope: rg
  params: {
    name: apim.outputs.apimServiceName
    apiName: apimProductApiName
    apiDisplayName: 'Shopping List API'
    apiDescription: 'This is a Shopping List API'
    webFrontendUrl: web.outputs.SERVICE_WEB_URI
    apiBackendUrl: api.outputs.SERVICE_API_URI
  }
}

// App outputs
output AZURE_LOCATION string = location
output AZURE_TENANT_ID string = tenant().tenantId
output API_BASE_URL string = api.outputs.SERVICE_API_URI
output WEB_BASE_URL string = web.outputs.SERVICE_WEB_URI
output PRODUCT_NAME string = web.outputs.APIM_PRODUCT_NAME
