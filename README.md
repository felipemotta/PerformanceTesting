# Performance Testing

Performance testing of .net Standard Assemblies using Docker and BenchmarkDotNet framework.

## 1. Prepare it!

 - Mono JIT compiler version 5.14.0 (Visual Studio built mono) [Mono](https://www.mono-project.com/download/stable/)
 - Powershell 5.1 [Powershell](https://docs.microsoft.com/en-us/powershell/scripting/setup/installing-windows-powershell?view=powershell-6)
 - Docker 18.06 [Docker](https://docs.docker.com/)
 
## 2. Run it!

Open command line console, go to Build folder and run the following command:

### Windows

```powershell
# Execute the bootstrapper script.
./build.ps1- Powershell 5.1.17134.228
```

### Linux / OS X

```console
# Adjust the permissions for the bootstrapper script.
chmod +x build.sh

# Execute the bootstrapper script.
./build.sh
```
