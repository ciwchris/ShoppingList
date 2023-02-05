param name string

@description('Resouce name to uniquely dentify this API within the API Management service instance')
@minLength(1)
param apiName string

@description('The Display Name of the API')
@minLength(1)
@maxLength(300)
param apiDisplayName string

@description('Description of the API. May include HTML formatting tags.')
@minLength(1)
param apiDescription string

@description('Absolute URL of the web frontend')
param webFrontendUrl string

@description('Absolute URL of the backend service implementing this API.')
param apiBackendUrl string

var apiPolicyContent = replace(loadTextContent('./apim-api-policy.xml'), '{origin}', webFrontendUrl)

resource restApi 'Microsoft.ApiManagement/service/apis@2021-12-01-preview' = {
  name: apiName
  parent: apimService
  properties: {
    description: apiDescription
    displayName: apiDisplayName
    path: 'api'
    protocols: [ 'https' ]
    subscriptionRequired: false
    type: 'http'
    format: 'openapi'
    serviceUrl: '${apiBackendUrl}/api'
    value: loadTextContent('../../Api/openapi.yaml')
  }
}

resource apiPolicy 'Microsoft.ApiManagement/service/apis/policies@2021-12-01-preview' = {
  name: 'policy'
  parent: restApi
  properties: {
    format: 'rawxml'
    value: apiPolicyContent
  }
}

resource apiDiagnostics 'Microsoft.ApiManagement/service/apis/diagnostics@2021-12-01-preview' = {
  name: 'applicationinsights'
  parent: restApi
  properties: {
    alwaysLog: 'allErrors'
    backend: {
      request: {
        body: {
          bytes: 1024
        }
      }
      response: {
        body: {
          bytes: 1024
        }
      }
    }
    frontend: {
      request: {
        body: {
          bytes: 1024
        }
      }
      response: {
        body: {
          bytes: 1024
        }
      }
    }
    httpCorrelationProtocol: 'W3C'
    logClientIp: true
    loggerId: apimLogger.id
    metrics: true
    sampling: {
      percentage: 100
      samplingType: 'fixed'
    }
    verbosity: 'verbose'
  }
}

resource apimService 'Microsoft.ApiManagement/service@2021-08-01' existing = {
  name: name
}

resource apimLogger 'Microsoft.ApiManagement/service/loggers@2021-12-01-preview' existing = {
  name: 'app-insights-logger'
  parent: apimService
}

output SERVICE_API_URI string = apimService.properties.gatewayUrl
