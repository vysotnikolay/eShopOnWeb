
# Variable block
rootFolder=$(pwd)
centralUsLoc="centralus"
rg="CloudX"
fileName="site.zip"
sitePrefix="e-shop-on-web"

cd src/PublicApi
dotnet publish -o pub
cd pub
zip -r $fileName *
apiPubPath=$(pwd)/$fileName
echo "API zip path is "$apiPubPath""

echo "---------------------------------------------------------------------------"
cd $rootFolder
read

echo "Creating the resource group $rg"
az group create -n $rg -l $centralUsLoc

echo "Starting template deployment from $rootFolder"
az deployment group create -n myTemplate -g $rg -f template-web-api-and-sqls.json

echo "---------------------------------------------------------------------------"

az webapp deploy -g $rg -n $sitePrefix-api --src-path $apiPubPath

read