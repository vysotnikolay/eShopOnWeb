
# Variable block
rootFolder=$(pwd)
centralUsLoc="centralus"
rg="CloudX"
fileName="site.zip"
sitePrefix="e-shop-on-web"

cd src/PublicApi
rm -rf pub
echo "Press any key..."
read
dotnet publish -o pub
cd pub
echo "Press any key..."
read
zip -r $fileName *
apiPubPath=$(pwd)/$fileName
echo "API zip path is "$apiPubPath""

echo "---------------------------------------------------------------------------"
cd $rootFolder
# echo "Press any key..."
# read

# echo "Creating the resource group $rg"
# az group create -n $rg -l $centralUsLoc

# echo "Starting template deployment from $rootFolder"
# az deployment group create -n myTemplate -g $rg -f template-web-api-and-sqls.json

# echo "---------------------------------------------------------------------------"
echo "Press any key..."
read
az webapp deploy -g $rg -n $sitePrefix-api --src-path $apiPubPath

echo "Press any key..."
read