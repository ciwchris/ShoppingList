param name string = 'add'

param webName string = ''
param keyVaultName string = ''
param permissions object = { secrets: [ 'get', 'list' ] }
param principalId string

resource keyVaultAccessPolicies 'Microsoft.KeyVault/vaults/accessPolicies@2022-07-01' = {
  parent: keyVault
  name: name
  properties: {
    accessPolicies: [ {
        objectId: principalId
        tenantId: subscription().tenantId
        permissions: permissions
      } ]
  }
}


resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}

resource web 'Microsoft.Web/staticSites@2022-03-01' existing = {
  name: webName
}
