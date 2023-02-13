# Enabling interprocedural LLVM optimizations with HelloiOS sample

DISCLAIMER: This README.md should be removed in the future. Its purpose is to note down the changes added during the experiment of enabling interprocedural optimizations with LLVM.

## 

## Plan

- [x] Create a custom managed library `MyLib` with private members which can be optimized with LLVM
- [x] Reference the library from `HelloiOS` application
- [x] Remove `MyLib's` references of private members:
    - [x] mark private methods in `mono_aot_can_specialize` in `aot-compiler.c` 
    - [x] remove their entries from `method_addresses` table (used by aot-runtime) 
    - [x] remove their entries from `llvm.used` table (enabling LLVM optimizer) https://llvm.org/docs/LangRef.html#id1828
    - [x] explicitly set internal linkage for private members (enabling LLVM optimizer)
    - [ ] add logging if a method look-up fails at runtime
- [x] Use `llvm-dis` to verify that all from the above has been successfully applied and private members removed, by inspecting the llvm bitcode files
- [ ] Enable the optimization not just for the custom managed library but for the whole program compilation ()
- [ ] Test and special-case methods needed at runtime by adding/excluding them in the above steps
- [ ] Document findings

Follow-up work:

- [ ] Extend the optimization to `internal` members as well
- [ ] Enable further optimizations with LLVM (removal of null checks, unused argument removal, constant propagation, etc)
    - `--attributor-enable=all` will be required to be passed to the optimizer `llvm opt` to enable eliminating null checks
- ...

## Tips

- To inspect the assembler files produced by the compiler look for `*.s` in the intermediate/output directories.
- For disassembling the generated object files and executables use: `otool -vt <file_name>`
- To inspect the llvm bitcode files look for `*.bc` in the intermediate/output directories.
- For disassembling the generated bitcode files use: `llvm-dis bin/ios-arm64/publish/MyLib.dll.s.opt.bc ` it will generate a `*.ll` file with llvm IR
- `method_addresses` table maps `method_id -> method` address, it could be understood as (but not entirely accurate):
```
-- metadata tokens:
MyType::.ctor : 0x06000004
MyType::SomeMethod: 0x06000005
-- respective method_idx:
MyType::.ctor : 0x00000000
MyType::SomeMethod: 0x00000001
-- respective offsets from method_addresses
MyType::.ctor : +0
MyType::SomeMethod: +4
```
- For listing symbols of the generated object files and executables use: `nm <file_name>`

## Known limitations

- Private members can still be accessed indirectly (e.g, via reflection) with proposed optimizations this would crash at runtime. (`DisablePrivateReflectionAttribute` might help)
- Internal members could be accessed from other assemblies via `InternalsVisibleToAttribute` which would also require externally visible entries
- ...

## Reference

The work has been inspired by Zoltan's prototype for WASM.
The patch is available here: https://gist.github.com/vargaz/e3567e7e26e71e9956d779e5bed054b3