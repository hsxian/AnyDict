rm -rf publish/
dotnet publish -c release -r ubuntu-x64 /p:PublishSingleFile=true -o publish/any-dict-ubuntu-x64/
dotnet publish -c release -r centos-x64 /p:PublishSingleFile=true -o publish/any-dict-centos-x64/
dotnet publish -c release -r win7-x64 /p:PublishSingleFile=true -o publish/any-dict-win7-x64/
dotnet publish -c release -r win10-x64 /p:PublishSingleFile=true -o publish/any-dict-win10-x64/