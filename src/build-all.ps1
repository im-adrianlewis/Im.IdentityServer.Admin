Param(
	[Parameter(Mandatory=$true)]
	[ValidatePattern("^[0-9]+\.[0-9]+\.[0-9]+$")]
	[string]
	$Version,
	[Parameter(Mandatory=$true)]
	[ValidateSet("Dev", "Alpha", "Beta", "Stable")]
	[string]
	$ReleaseKind,
	[int]
	$BuildNumber
)

enum ReleaseType
{
	Dev
	Alpha
	Beta
	Stable
}

$Release = [ReleaseType]$ReleaseKind

function PublishToDocker
{
	[CmdletBinding()]
	Param(
		[Parameter(Mandatory=$true)]
		[string]
		$PathToContext,
		[Parameter(Mandatory=$true)]
		[string]
		$PathToDockerfile,
		[Parameter(Mandatory=$true)]
		[string]
		$Repo,
		[Parameter(Mandatory=$true)]
		[ValidatePattern("^[0-9]+\.[0-9]+\.[0-9]+$")]
		[string]
		$Version,
		[Parameter(Mandatory=$true)]
		[ReleaseType]
		$Release,
		[int]
		$BuildNumber
	)

	switch ($Release)
	{
		ReleaseType.Dev { $SemVer = "{0}-dev-{1:yyyyMMddHHmmss}" -f $Version, [DateTime]::UtcNow }
		ReleaseType.Alpha { $SemVer = "{0}-alpha{1}" -f $Version, $BuildNumber }
		ReleaseType.Beta { $SemVer = "{0}-beta{1}" -f $Version, $BuildNumber }
		default { $SemVer = $Version }
	}

	Write-Host ("Building docker image imadrianlewis/{0}:{1}" -f $Repo, $SemVer)
	docker build -t imadrianlewis.azurecr.io/${Repo}:${SemVer} -f $PathToDockerfile $PathToContext
	docker push imadrianlewis.azurecr.io/${Repo}:${SemVer}
}

PublishToDocker -Repo "imaccessgraphportal" -Version $Version -Release $Release -BuildNumber $BuildNumber -PathToContext ".\Backend" -PathToDockerfile ".\Backend\Im.Access.GraphPortal\Dockerfile"
