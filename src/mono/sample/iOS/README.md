## Overview

This is an experiment of using NativeAOT dependency analysis scanner (`ILC_DA` mode) to work with Mono system module - `System.Private.CoreLib`.
To goal of the experiment is to use NativeAOT to output all the methods that need to be AOT compiled with Mono,
into a file (in .mibc format). This file would be used as an input to the MonoAOT compiler when performing fullAOT compilation
to compile only what is required for the app to run (similar to the PGO optimization).

The experiment should consist of two stages:
1. Optimize generics - The first stage of this experiment is to assist MonoAOT compilation with compilation of generics i.e. providing a list of generic instances needed during runtime.
2. Optimize full program - The later stage would be to expand this feature for all types.

## How to build

### Build the repo

1. Builds ILCompiler from this directory execute:
``` bash
../../../../build.sh clr.tools+clr.jit -c release
```
NOTE: This will build ILCompiler for the host platform - eg: osx-arm64

2. Builds Mono to target iOS platforms:
``` bash
 ../../../../build.sh mono+libs -c release -os ios -arch arm64  
```

### Build and run the HelloiOS app

- To build and run the sample application with MonoAOT in fullAOT mode 
    ``` bash
    make rundev MONO_CONFIG=Release DEPLOY_AND_RUN=false
    ```

- To build and run the sample application with MonoAOT in fullAOT mode without GSHAREDVTs (current sample will fail at runtime if GSHAREDVTs aren't generated)
    ``` bash
    make rundev MONO_CONFIG=Release DEPLOY_AND_RUN=false NO_GSHVT=true
    ```

- To build and run the sample application with MonoAOT in fullAOT mode without GSHAREDVTs with NativeAOT dependency analysis guided compilation
    ``` bash
    make rundev MONO_CONFIG=Release DEPLOY_AND_RUN=false NO_GSHVT=true ILC_DA=true
    ```

- To measure size do not use any provisioning profile: `export DevTeamProvisioning=-`

NOTE: It is also possible to pass `LOG=true` when invoking main to enable runtime logging

### ILC da configuration

- To filter collected methods by the DAGO adjust the `Program.csproj` file.
    - For example to include generic methods from all assemblies, the setup should look like:
        ```xml
        <ReproResponseLines Include="#--scanmibcforassembly:$(AssemblyName)" />
        <ReproResponseLines Include="--scanmibcforallgenerics" />
        <ReproResponseLines Include="#--scanmibclogdump" />
        ```
    - For more information about available switches run ilc with `--help`

## Preliminary size measurements for HelloiOS

| Mode        | Size (bytes) | Size (MB) | Saving |
|-------------|--------------|-----------|--------|
| GSHAREDVT   | 27980822     | 27,98     | N/A    |
| noGSHAREDVT | 26412238     | 26,41     | -5,61%  |
| ILC-DA      | 26734318     | 26,73     | -4,45%  |

## TODO

1. Make sure we do not generate anything extra when compiling instances that came from the mibc profile (basically avoid Mono's dependency analysis on them)
    - it is possible we still include something extra that is not required
2. Try disabling `cfg->prefer_instances` in the `ILC_DA` mode as we should have everything necessary for the runtime in the mibc profile
3. Try more complex apps - MAUI template, and measure size/perf
4. Try using generics with reflection to see if that is something `ILC_DA` would be able to detect as required
5. Figure out why creating mibc profile throws a warning about method count not being the same as in the assembly
6. Go through all the ILC changes and iron them out (understand/document why they are needed and what is the effect)
7. Test GSHAREDVT tests with `ILC_DA`

## Ideas on estimating size savings and coverage

- Experiment:
    - AOT compile only methods from the DAGO profile
    - Track down and measure the percentage of methods being AOT compiler (if MonoAOT compiled everything from the profile - or more)
    - Run the sample - for which and how many cases/methods do JIT or interpreter get triggered
    - Size of the final output compared to fullAOT output

## References

- [Mono GSHAREDVT tests](../../../mono/mono/mini/gshared.cs)