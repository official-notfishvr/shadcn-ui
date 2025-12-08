param(
    [Parameter(Mandatory=$true)]
    [string]$Version,
    
    [Parameter(Mandatory=$false)]
    [ValidateSet('alpha', 'beta', 'rc', 'stable')]
    [string]$ReleaseType = 'stable',
    
    [Parameter(Mandatory=$false)]
    [switch]$DryRun
)

function Write-Success { Write-Host $args -ForegroundColor Green }
function Write-Info { Write-Host $args -ForegroundColor Cyan }
function Write-Warning { Write-Host $args -ForegroundColor Yellow }
function Write-Error { Write-Host $args -ForegroundColor Red }

Write-Host ""
Write-Host "╔═══════════════════════════════════════╗" -ForegroundColor Magenta
Write-Host "║   shadcnui Release Helper Script     ║" -ForegroundColor Magenta
Write-Host "╚═══════════════════════════════════════╝" -ForegroundColor Magenta
Write-Host ""

if ($Version -notmatch '^v?\d+\.\d+\.\d+$') {
    Write-Error "Invalid version format. Use: X.Y.Z or vX.Y.Z (e.g., 0.0.1 or v0.0.1)"
    exit 1
}

if (-not $Version.StartsWith('v')) {
    $Version = "v$Version"
}

if ($ReleaseType -ne 'stable') {
    $fullVersion = "$ReleaseType-$Version"
} else {
    $fullVersion = $Version
}

Write-Info "Preparing release: $fullVersion"
Write-Info "Release type: $ReleaseType"
Write-Host ""

if (-not (Test-Path "shadcnui.sln")) {
    Write-Error "Error: shadcnui.sln not found. Are you in the project root?"
    exit 1
}

Write-Success "Found project solution"

try {
    $gitVersion = git --version
    Write-Success "Git is available: $gitVersion"
} catch {
    Write-Error "Git is not available. Please install Git."
    exit 1
}

$gitStatus = git status --porcelain
if ($gitStatus) {
    Write-Warning "You have uncommitted changes:"
    Write-Host $gitStatus
    Write-Host ""
    $continue = Read-Host "Continue anyway? (y/N)"
    if ($continue -ne 'y') {
        Write-Info "Release cancelled."
        exit 0
    }
}

$existingTag = git tag -l $fullVersion
if ($existingTag) {
    Write-Error "Tag $fullVersion already exists!"
    Write-Info "To delete it, run:"
    Write-Info "  git tag -d $fullVersion"
    Write-Info "  git push origin :refs/tags/$fullVersion"
    exit 1
}

Write-Success "Tag $fullVersion is available"

$currentBranch = git branch --show-current
Write-Info "Current branch: $currentBranch"

if ($currentBranch -ne 'main') {
    Write-Warning "You're not on the main branch!"
    $continue = Read-Host "Continue anyway? (y/N)"
    if ($continue -ne 'y') {
        Write-Info "Release cancelled."
        exit 0
    }
}

Write-Host ""
Write-Info "Recent commits:"
git log --oneline -5
Write-Host ""

$lastTag = git describe --tags --abbrev=0 2>$null
if ($lastTag) {
    Write-Info "Changes since $lastTag"
    $commitCount = (git rev-list $lastTag..HEAD --count)
    Write-Info "   Commits: $commitCount"
    
    $contributors = git log $lastTag..HEAD --format='%aN' --no-merges | Sort-Object -Unique
    Write-Info "   Contributors: $($contributors.Count)"
    
    Write-Host ""
    Write-Info "Commit summary:"
    git log $lastTag..HEAD --pretty=format:"   - %s (%h)" --no-merges | Select-Object -First 10
    Write-Host ""
} else {
    Write-Info "This will be the first release"
}

Write-Host ""

if (-not $DryRun) {
    Write-Host "═══════════════════════════════════════" -ForegroundColor Yellow
    Write-Host "Ready to create release: $fullVersion" -ForegroundColor Yellow
    Write-Host "═══════════════════════════════════════" -ForegroundColor Yellow
    Write-Host ""
    Write-Host "This will:"
    Write-Host "  1. Create tag: $fullVersion"
    Write-Host "  2. Push tag to origin"
    Write-Host "  3. Trigger GitHub Actions release workflow"
    Write-Host "  4. Build all 4 configurations"
    Write-Host "  5. Create GitHub release with DLLs"
    Write-Host ""
    
    $confirm = Read-Host "Proceed? (y/N)"
    if ($confirm -ne 'y') {
        Write-Info "Release cancelled."
        exit 0
    }
}

Write-Host ""
Write-Info "Creating tag..."

if ($DryRun) {
    Write-Warning "[DRY RUN] Would execute: git tag $fullVersion"
    Write-Warning "[DRY RUN] Would execute: git push origin $fullVersion"
} else {
    try {
        git tag $fullVersion
        Write-Success "Tag created locally"
        
        Write-Info "Pushing tag to origin..."
        git push origin $fullVersion
        Write-Success "Tag pushed to origin"
        
        Write-Host ""
        Write-Success "═══════════════════════════════════════"
        Write-Success "Release $fullVersion initiated!"
        Write-Success "═══════════════════════════════════════"
        Write-Host ""
        Write-Info "Monitor the release build:"
        Write-Info "   https://github.com/official-notfishvr/shadcn-ui/actions"
        Write-Host ""
        Write-Info "View releases when ready:"
        Write-Info "   https://github.com/official-notfishvr/shadcn-ui/releases"
        Write-Host ""
        Write-Success "The release workflow is now running!"
        Write-Info "   It will take a few minutes to build all configurations."
        Write-Host ""
        
    } catch {
        Write-Error "Error creating release: $_"
        exit 1
    }
}

Write-Host ""
Write-Info "Next steps:"
Write-Info "   1. Wait for GitHub Actions to complete (~5-10 minutes)"
Write-Info "   2. Review the auto-generated release notes"
Write-Info "   3. Edit the release description if needed"
Write-Info "   4. Test the released DLLs"
Write-Info "   5. Announce the release!"
Write-Host ""
