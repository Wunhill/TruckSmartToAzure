##Values that map to the deployment
$rg = "4480CH03"
$uniquePrefix = "tsw4480c2"
$location = "EastUS"
$sqlAdmin = "student"
$sqlPassword = "Pa55w.rd1234"
$dbFilePath = "$($PSScriptRoot)\$($dbFileName)"
# If you are running this piecemeal, set the $dbFilePath manually
# $dbFilePath = "<local path to file>\trucksmart.bacpac"

#Calculated values
$saName = "$($uniquePrefix)sa"
$container = "uploads"
$dbName = "TruckSmart"
$dbServerName = "$($uniquePrefix)sql"
$dbFileName = "trucksmart.bacpac"
$sqlPasswordSecure = ConvertTo-SecureString -String $sqlPassword -AsPlainText -Force

#Upload the bacpac file
New-AzureRmStorageAccount -ResourceGroupName $rg -Name $saName -SkuName Standard_LRS `
    -Location $location -Kind StorageV2 -AccessTier Hot

$key = (Get-AzureRmStorageAccountKey -ResourceGroupName $rg -Name $saName)[0].Value
$context = New-AzureStorageContext -StorageAccountName $saName -StorageAccountKey $key

New-AzureStorageContainer -Name $container -Permission Off -Context $context
Set-AzureStorageBlobContent -File $dbFilePath -Container $container -Blob $dbFileName `
    -BlobType Block -Context $context

$bacpacUri = "https://$($saName).blob.core.windows.net/$($container)/$($dbFileName)"

##Create SQL Server
$Credential = New-Object -TypeName "System.Management.Automation.PSCredential" -ArgumentList $sqlAdmin, $sqlPasswordSecure
New-AzureRmSqlServer -ServerName $dbServerName -SqlAdministratorCredentials $Credential -Location $location 

#Import the bacpac file
New-AzureRmSqlDatabaseImport -ResourceGroupName $rg -ServerName $dbServerName -DatabaseName $dbName `
    -StorageKeyType "StorageAccessKey" -StorageKey $key -StorageUri $bacpacUri `
     -AdministratorLogin $sqlAdmin  -AdministratorLoginPassword $sqlPasswordSecure `
     -Edition Basic -ServiceObjectiveName Basic -DatabaseMaxSizeBytes 5000000



#Output connection information
$connectionString = "Server=tcp:$($dbServerName).database.windows.net,1433;Initial Catalog=$($dbName);" + `
    "Persist Security Info=False;User ID=$($sqlAdmin);Password=$($sqlPassword);MultipleActiveResultSets=False;" + `
    "Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"

Write-Output "SQL Connection String:"
Write-Output $connectionString
