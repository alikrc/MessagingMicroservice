dotnet test --collect:"XPlat Code Coverage" --no-restore


#need to replace guid here to generate real report
reportgenerator "-reports:\TestResults\{guid}\coverage.cobertura.xml" "-targetdir:coveragereport" -reporttypes:Html


# run this command for first time to install reporting tool
#dotnet tool install -g dotnet-reportgenerator-globaltool


# form more info 
# https://docs.microsoft.com/en-us/dotnet/core/testing/unit-testing-code-coverage?tabs=windows