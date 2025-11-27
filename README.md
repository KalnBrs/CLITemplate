# CLIProjectTool

Small CLI to manage and scaffold project templates.

**Prerequisites**

- .NET SDK 9.x installed (verify with `dotnet --version`).
- macOS with `zsh` (commands below use zsh syntax).

**Files of interest**

- `CLIProjectTool/CLIProjectTool.csproj` — project file and package metadata (Version, PackageId, ToolCommandName)
- `Templates/` — template files included in the packaged tool
- `scripts/rebuild-tool.sh` — helper script to build/pack and (re)install the global tool

**Add dotnet tools to PATH (one-time, zsh)**

Run to add to current session:

```bash
export PATH="$PATH:$HOME/.dotnet/tools"
```

Persist for new shells:

```bash
cat << 'EOF' >> ~/.zprofile
# Add .NET Core SDK tools
export PATH="$PATH:$HOME/.dotnet/tools"
EOF
# then open a new shell or run `zsh -l`
```

**Quick install (recommended)**
This repository includes a helper script that builds, packs, and installs the CLI as a global tool.

From the repository root run:

```bash
./scripts/rebuild-tool.sh
```

This will:

- Build the project in `Release` configuration
- Pack the project into `./nupkgs` (a local feed)
- Update or install the global tool from that local feed

**Manual install / update**

1. Pack the project (create `.nupkg` in `./nupkgs`):

```bash
dotnet pack ./CLIProjectTool/CLIProjectTool.csproj -c Release -o ./nupkgs
# if you already built, use --no-build to avoid rebuilding
# dotnet pack ./CLIProjectTool/CLIProjectTool.csproj -c Release -o ./nupkgs --no-build
```

2. Install globally (from the local feed):

```bash
dotnet tool install --global --add-source "$(pwd)/nupkgs" CLIProjectTool --version 1.0.3
```

3. To update later (preferred: bump the `<Version>` in the csproj and repack):

```bash
# After bumping <Version> and repacking
dotnet tool update --global --add-source "$(pwd)/nupkgs" CLIProjectTool --version X.Y.Z
# Or uninstall + install (works when reusing same version)
# dotnet tool uninstall --global CLIProjectTool
# dotnet tool install --global --add-source "$(pwd)/nupkgs" CLIProjectTool --version 1.0.3
```

**Quick local testing without installing**

To run the CLI directly from source (fast iteration):

```bash
dotnet run --project ./CLIProjectTool/CLIProjectTool.csproj -- list
dotnet run --project ./CLIProjectTool/CLIProjectTool.csproj -- version
```

**Common commands (after global install)**

```bash
cli-project-tool --help
cli-project-tool version
cli-project-tool list
cli-project-tool new C#GitIgnore.gitignore MyProject
cli-project-tool templates add MyTemplate /path/to/file.txt
cli-project-tool templates remove MyTemplate
```

**Troubleshooting**

- If the tool command is not found, ensure `~/.dotnet/tools` is on your `PATH` (see above).
- If `dotnet tool install` reports package/version not found, confirm the `.nupkg` exists in `./nupkgs` and the package `Version` matches the `--version` you supplied.
- Hidden files (like `.gitignore`) are excluded by default from NuGet packages and will generate warnings during `dotnet pack`. If you need to include dotfiles, repack with `-NoDefaultExcludes` but review whether they should be packaged.
- If your tool looks for `Templates` in the wrong location, remember global tools run from the tools install path; this project packages templates under `tools/net9.0/any/Templates/` so they will be available at runtime under the tool installation folder.

**Developer notes**

- The `VersionCommand` reads the assembly informational / file / assembly version at runtime so the printed `Version` reflects the package version in most builds.
- To release a new version, bump the `<Version>` in `CLIProjectTool/CLIProjectTool.csproj`, run `./scripts/rebuild-tool.sh`, and test with `cli-project-tool version`.

**License & attribution**

- (Add a license here if you want one.)

---

If you want, I can also add GitHub Actions to automatically pack and publish a release when you push tags or merge to `main`.
