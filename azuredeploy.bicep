param location string = resourceGroup().location
param logAnalyticsWorkspaceName string
param applicationInsightsName string
param containerRegistryName string
param containerAppsEnvironmentName string
param containerAppName string
param sqlServerName string
param sqlDatabaseName string
param sqlServerAdministratorsGroupName string
param sqlServerAdministratorsGroupObjectId string
param sqlDatabaseSkuName string

resource logAnalyticsWorkspace 'Microsoft.OperationalInsights/workspaces@2022-10-01' = {
  name: logAnalyticsWorkspaceName
  location: location
}

resource applicationInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: applicationInsightsName
  location: location
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalyticsWorkspace.id
  }
}

resource containerRegistry 'Microsoft.ContainerRegistry/registries@2021-09-01' = {
  name: containerRegistryName
  location: location
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
    networkRuleBypassOptions: 'AzureServices'
  }
}

resource containerAppsEnvironment 'Microsoft.App/managedEnvironments@2022-03-01' = {
  name: containerAppsEnvironmentName
  location: location
  properties: {
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalyticsWorkspace.properties.customerId
        sharedKey: logAnalyticsWorkspace.listKeys().primarySharedKey
      }
    }
    daprAIConnectionString: applicationInsights.properties.ConnectionString
  }
}

// resource containerApp 'Microsoft.App/containerApps@2022-03-01' = {
//   name: containerAppName
//   location: location
//   identity: {
//     type: 'SystemAssigned'
//   }
//   properties: {
//     managedEnvironmentId: containerAppsEnvironment.id
//     configuration: {
//       ingress: {
//         external: true
//       }
//     }
//     template: {
//       containers: [
//         {
//           name: 'api'
//           image: 'api:v1'

//         }
//       ]
//     }
//   }
// }

resource sqlServer 'Microsoft.Sql/servers@2021-11-01' = {
  name: sqlServerName
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    minimalTlsVersion: '1.2'
    publicNetworkAccess: 'Enabled'
    administrators: {
      administratorType: 'ActiveDirectory'
      azureADOnlyAuthentication: true
      login: sqlServerAdministratorsGroupName
      principalType: 'Group'
      sid: sqlServerAdministratorsGroupObjectId
      tenantId: tenant().tenantId
    }
  }

  resource firewallRule 'firewallRules' = {
    name: 'AllowAllWindowsAzureIps'
    properties: {
      startIpAddress: '0.0.0.0'
      endIpAddress: '0.0.0.0'
    }
  }

  resource sqlDatabase 'databases' = {
    name: sqlDatabaseName
    location: location
    sku: {
      name: sqlDatabaseSkuName
    }
    properties: {}
  }
}

// output containerAppManagedIdentityId string = containerApp.identity.principalId