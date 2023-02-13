param name string = 'add'

param keyVaultName string = ''
param permissions object = { secrets: [ 'get', 'list' ] }
param principalId string
@secure()
param azureClientId string
@secure()
param azureClientSecret string

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

resource azureClientIdSecret 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: keyVault
  name: 'AZURECLIENTID'
  properties: {
    value: azureClientId
  }
}

resource azureClientSecretSecret 'Microsoft.KeyVault/vaults/secrets@2021-11-01-preview' = {
  parent: keyVault
  name: 'AZURECLIENTSECRET'
  properties: {
    value: azureClientSecret
  }
}

resource keyVault 'Microsoft.KeyVault/vaults@2022-07-01' existing = {
  name: keyVaultName
}


