
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

echo "--------------------Web app zip is ready for publishing. Press any key ---------------------------------------------"
cd $rootFolder
read

echo "Creating the resource group $rg"
az group create -n $rg -l $centralUsLoc

echo "Starting template deployment from $rootFolder"
az deployment group create -n myTemplate -g $rg -f template.json

echo "---------------------------------------------------------------------------"


az webapp deploy -g $rg -n $sitePrefix-front --src-path $webPubPath
az webapp deploy -g $rg -n $sitePrefix-eastus --src-path $webPubPath

read