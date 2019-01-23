..\sonar-scanner\SonarScanner.MSBuild.exe begin /k:"linqfilter" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="5c61980badf55fe39f8ba3cb1ec1fc76697ab662" /d:sonar.cs.opencover.reportsPaths="%CD%\opencover.xml"
MsBuild.exe src\Swords.LinqFilter.sln /t:Rebuild
"%LOCALAPPDATA%\Apps\OpenCover\OpenCover.Console.exe" -output:"%CD%\opencover.xml" -register:user -target:"c:\Program Files\dotnet\dotnet.exe" -oldstyle -targetargs:"test src\"
..\sonar-scanner\SonarScanner.MSBuild.exe end /d:sonar.login="5c61980badf55fe39f8ba3cb1ec1fc76697ab662"


