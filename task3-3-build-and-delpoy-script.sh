
# Variable block
rootFolder=$(pwd)
centralUsLoc="centralus"
eastUsLoc="eastus"
rg="CloudX"
fileName="site.zip"
sitePrefix="e-shop-on-web"

cd src/Web
dotnet publish -o pub
cd pub
zip -r $fileName *
webPubPath=$(pwd)/$fileName
echo "Web zip path is "$webPubPath""

echo "---------------------------------------------------------------------------"

cd ../../PublicApi
dotnet publish -o pub
cd pub
zip -r $fileName *
apiPubPath=$(pwd)/$fileName
echo "API zip path is "$apiPubPath""

echo "---------------------------------------------------------------------------"
cd $rootFolder
echo "Starting template deployment from $rootFolder"
read
az deployment group create -n myTemplate -g $rg -f template.json

echo "---------------------------------------------------------------------------"
az webapp deploy -g $rg -n $sitePrefix-front --src-path $webPubPath
az webapp deploy -g $rg -n $sitePrefix-eastus --src-path $webPubPath
az webapp deploy -g $rg -n $sitePrefix-api --src-path $apiPubPath

read