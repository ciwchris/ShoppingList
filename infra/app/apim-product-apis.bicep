param apimName string
param apimProductName string
param apimProductApiName string


resource apimService 'Microsoft.ApiManagement/service@2021-08-01' existing = {
  name: apimName
}

resource apimProduct 'Microsoft.ApiManagement/service/products@2022-04-01-preview' existing = {
  name: apimProductName
  parent: apimService
}

resource productApi 'Microsoft.ApiManagement/service/products/apis@2022-04-01-preview' = {
  name: apimProductApiName
  parent: apimProduct
}
