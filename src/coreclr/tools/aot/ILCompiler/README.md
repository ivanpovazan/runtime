## Overview

This is an experiment of using NativeAOT dependency analysis scanner to work with Mono system module - `System.Private.CoreLib`.
To goal of the experiment is to use NativeAOT to output all the methods that need to be AOT compiled with Mono,
into a file (in .mibc format). This file would be used as an input to the MonoAOT compiler when performing fullAOT compilation
to compile only what is required for the app to run (similar to the PGO optimization).

The experiment should consist of two stages:
1. Optimize generics - The first stage of this experiment is to assist MonoAOT compilation with compilation of generics i.e. providing a list of generic instances needed during runtime.
2. Optimize full program - The later stage would be to expand this feature for all types.

## How to build

1. Build the repo with:
``` bash
./build.sh mono+libs+clr.tools+clr.jit -c debug /p:MonoEnableLlvm=true /p:MonoLLVMUseCxx11Abi=true
```
- `mono+libs` builds Mono runtime, MonoAOT compiler and libraries with llvm backend
- `clr.tools+clr.jit` builds ILCompiler
2. Build the HelloWorld app:
``` bash
pushd src/mono/sample/HelloWorld
make publish
popd
```
3. Create a response file: `repro.rsp` with the following content
```
artifacts/bin/HelloWorld/arm64/Debug/osx-arm64/publish/HelloWorld.dll
-r:artifacts/bin/HelloWorld/arm64/Debug/osx-arm64/publish/System.Private.CoreLib.dll
-r:artifacts/bin/HelloWorld/arm64/Debug/osx-arm64/publish/System.Collections.dll
-r:artifacts/bin/HelloWorld/arm64/Debug/osx-arm64/publish/System.Console.dll

-o:output.obj
--root:HelloWorld

--scan
--scanmibclog:HelloiOS.mibc
--scanmibcforassembly:HelloWorld
#--scanmibcforallgenerics
#--scanmibclogdump
```
4. Run the ILCompiler with the response file (3) via:
```
artifacts/bin/coreclr/osx.arm64.Debug/ilc/ilc @repro.rsp
```
5. TODO: Pass the generated `HelloiOS.mibc` to MonoAOT compilation