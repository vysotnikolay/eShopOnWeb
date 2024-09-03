
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


az webapp deploy -g $rg -n $sitePrefix-front --src-path $webPubPath
az webapp deploy -g $rg -n $sitePrefix-eastus --src-path $webPubPath

read