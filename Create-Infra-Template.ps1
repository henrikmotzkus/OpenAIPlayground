# Getting an token for manual API request
$TenantID = "<YOUR Tenant ID>"
Import-Module -Name Az
Import-Module -Name AzureAD
Connect-AzAccount -TenantId $TenantID
$ctx = get-azcontext
$t = Get-AzAccessToken -ResourceUrl "https://search.azure.com" -TenantId $TenantID
$t.Token
$upn = "<User UPN>"
$me = Get-AzADUser -Filter "userPrincipalName eq '$upn'"
$objectid = $me.Id
$subscriptionid = "<SubID>"
$rgname = "OpenAI"
$searchservice = "henrik-test-1"
$indexname = "hotels-sample-index"
$scopedef = "Search Index Data Reader"
$scope = "/subscriptions/$subscriptionid/resourceGroups/$rgname/providers/Microsoft.Search/searchServices/$searchservice/indexes/$indexname"

# Assign a user to an specific index
New-AzRoleAssignment -ObjectId $objectid -RoleDefinitionName $scopedef -Scope $scope
get-azroleassignment -Scope $scope
remove-azroleassignment -Scope $scope -ObjectId $objectid -RoleDefinitionName $scopedef

# Create a dotnet app 
dotnet new blazorwasm -au SingleOrg --client-id "<Client of your App in AAD>" -o "OpenAIClient" --tenant-id $TenantID