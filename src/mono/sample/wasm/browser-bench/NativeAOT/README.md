## NativeAOT wasm browser-bench variation

### Prerequisite

NativeAOT requires setting `EMSDK` environment variable to the matching (the version the runtime got built with) Emscripten SDK version in order to properly build/deploy the app.

```
$env:EMSDK='C:\Users\ivanpovazan\repos\emsdk\'
```

### Build

From repo root:

```
.\dotnet.cmd publish -c Release -r browser-wasm .\src\mono\sample\wasm\browser-bench\NativeAOT\Wasm.Console.Bench.Sample.csproj
```

### Run

#### Browser

From repo root:

```
cp src\mono\sample\wasm\browser-bench\NativeAOT\bin\Release\net8.0\browser-wasm\publish\Wasm.Console.Bench.Sample.html src\mono\sample\wasm\browser-bench\NativeAOT\bin\Release\net8.0\browser-wasm\publish\index.html ; dotnet-serve.exe -o -d src\mono\sample\wasm\browser-bench\NativeAOT\bin\Release\net8.0\browser-wasm\publish
```

#### Node

From repo root:

```
node.exe .\src\mono\sample\wasm\browser-bench\NativeAOT\bin\Release\net8.0\browser-wasm\publish\Wasm.Console.Bench.Sample.js
```

### Clean

From repo root:

```
rm -r .\src\mono\sample\wasm\browser-bench\NativeAOT\bin ; rm -r .\src\mono\sample\wasm\browser-bench\NativeAOT\obj
```

### One-shot command

From repo root:

```
rm -r .\src\mono\sample\wasm\browser-bench\NativeAOT\bin ; rm -r .\src\mono\sample\wasm\browser-bench\NativeAOT\obj ; .\dotnet.cmd publish -c Release -r browser-wasm .\src\mono\sample\wasm\browser-bench\NativeAOT\Wasm.Console.Bench.Sample.csproj ; cp src\mono\sample\wasm\browser-bench\NativeAOT\bin\Release\net8.0\browser-wasm\publish\Wasm.Console.Bench.Sample.html src\mono\sample\wasm\browser-bench\NativeAOT\bin\Release\net8.0\browser-wasm\publish\index.html ; dotnet-serve.exe -o -d src\mono\sample\wasm\browser-bench\NativeAOT\bin\Release\net8.0\browser-wasm\publish
```

### Limitations

- This setup works only on windows-x64
- The setup only runs `Exceptions` benchmarks


## Running the benchmark with Mono for comparison

To run the NativeAOT supported set of benchmarks with Mono pass `-p:BenchmarkNativeAOT=true` when building/running the benchmark sample.
This can be achieved by passing the additional flag via `BuildAdditionalArgs` MSBuild property e.g.,:

From repo root:

```
rm -r .\src\mono\sample\wasm\browser-bench\bin ; rm -r .\artifacts\obj\mono\Wasm.Browser.Bench.Sample ; .\dotnet.cmd publish -t:RunSampleWithBrowser -c Release src\mono\sample\wasm\browser-bench\Wasm.Browser.Bench.Sample.csproj -p:RunAOTCompilation=true -p:BuildAdditionalArgs=/p:BenchmarkNativeAOT=true
```
