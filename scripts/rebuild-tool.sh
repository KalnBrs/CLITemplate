#!/usr/bin/env bash
# rebuild-tool.sh — Pack and reinstall the CLI tool globally
# Usage: ./scripts/rebuild-tool.sh

set -e

SCRIPT_DIR="$(cd "$(dirname "$0")" && pwd)"
PROJECT_ROOT="$(dirname "$SCRIPT_DIR")"
CSPROJ="$PROJECT_ROOT/CLIProjectTool/CLIProjectTool.csproj"
NUPKGS="$PROJECT_ROOT/nupkgs"

cd "$PROJECT_ROOT"

# Extract version from csproj
VERSION=$(grep -oP '(?<=<Version>)[^<]+' "$CSPROJ" 2>/dev/null || \
          sed -n 's/.*<Version>\([^<]*\)<\/Version>.*/\1/p' "$CSPROJ")

if [ -z "$VERSION" ]; then
  echo "❌ Could not extract version from csproj"
  exit 1
fi

echo "🔨 Building CLIProjectTool v$VERSION..."
dotnet build "$CSPROJ" -c Release

echo ""
echo "📦 Packing CLIProjectTool v$VERSION..."
mkdir -p "$NUPKGS"
dotnet pack "$CSPROJ" -c Release -o "$NUPKGS" --no-build

echo ""
echo "🔄 Updating global tool to v$VERSION..."

# Try update first, fall back to uninstall+install
dotnet tool update --global --add-source "$NUPKGS" CLIProjectTool --version "$VERSION" 2>/dev/null || {
  echo "   (update failed, trying uninstall+install)"
  dotnet tool uninstall --global CLIProjectTool 2>/dev/null || true
  dotnet tool install --global --add-source "$NUPKGS" CLIProjectTool --version "$VERSION"
}

echo ""
echo "✅ Done! Installed version:"
dotnet tool list --global | grep -i cliprojecttool || true

echo ""
echo "Run: cli-project-tool --help"
