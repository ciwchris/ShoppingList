@description('Resouce name to uniquely dentify the API Management service instance')
@minLength(1)
param apimName string

@description('Resouce name to uniquely dentify the API within the API Management service instance')
@minLength(1)
param apimApiName string

@description('Absolute URL of the web frontend')
param webFrontendUrl string

var apiPolicyContent = replace(loadTextContent('./apim-api-policy.xml'), '{origin}', webFrontendUrl)

resource apiPolicy 'Microsoft.ApiManagement/service/apis/policies@2021-12-01-preview' = {
  name: 'policy'
  parent: apimApi
  properties: {
    format: 'rawxml'
    value: apiPolicyContent
  }
}

resource apimApi 'Microsoft.ApiManagement/service/apis@2021-12-01-preview' existing = {
  name: apimApiName
  parent: apimService
}

resource apimService 'Microsoft.ApiManagement/service@2021-08-01' existing = {
  name: apimName
}  
