cd $PSScriptRoot

if (Test-Path 'install.lock') { return }
ni install.lock -Type file -Force | Out-Null

# Install dotnet cli
$dotnetPath = "$pwd\.dotnet"
$scriptsPath = "$pwd\.scripts"
$dotnetCliVersion = if ($env:DOTNET_CLI_VERSION) { $env:DOTNET_CLI_VERSION } else { 'Latest' }
$dotnetInstallScriptUrl = 'https://raw.githubusercontent.com/dotnet/cli/rel/1.0.0-preview2/scripts/obtain/dotnet-install.ps1'
$dotnetInstallScriptPath = "$scriptsPath\dotnet-install.ps1"
md -Force $scriptsPath | Out-Null
iwr $dotnetInstallScriptUrl -OutFile $dotnetInstallScriptPath
& $dotnetInstallScriptPath -Channel "preview" -version $dotnetCliVersion -InstallDir $dotnetPath -NoPath
$env:Path = if ($env:Path -like "*$dotnetPath*") { $env:Path } else { "$dotnetPath;$env:Path" }

# Install chocolatey
iwr https://chocolatey.org/install.ps1 -UseBasicParsing | iex

# Install nuget command line
cinst nuget.commandline -y

# Install npm
cinst nodejs.install -y

# Install bower
npm install -g bower

# Install FAKE
nuget install FAKE -OutputDirectory packages -ExcludeVersion -ConfigFile nuget.config -Verbosity quiet
