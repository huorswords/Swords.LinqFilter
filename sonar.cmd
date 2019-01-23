..\sonar-scanner\SonarScanner.MSBuild.exe begin /k:"linqfilter" /d:sonar.host.url="http://localhost:9000" /d:sonar.login="3775667fd808abdff8a2441b7672836a5ee51cb6" /d:sonar.cs.opencover.reportsPaths="%CD%\opencover.xml"
MsBuild.exe src\Swords.LinqFilter.sln /t:Rebuild
"%LOCALAPPDATA%\Apps\OpenCover\OpenCover.Console.exe" -output:"%CD%\opencover.xml" -register:user -target:"c:\Program Files\dotnet\dotnet.exe" -oldstyle -targetargs:"test src\"
..\sonar-scanner\SonarScanner.MSBuild.exe end /d:sonar.login="3775667fd808abdff8a2441b7672836a5ee51cb6"


