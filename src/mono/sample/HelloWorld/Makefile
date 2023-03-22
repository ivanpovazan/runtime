TOP=../../../../
TOOLS_DIR=../../../tasks
DOTNET:=$(TOP)dotnet.sh
DOTNET_Q_ARGS=--nologo -v:q -consoleloggerparameters:NoSummary

MONO_CONFIG?=Debug
MONO_ARCH?=$(shell . $(TOP)eng/native/init-os-and-arch.sh && echo $${arch})
TARGET_OS?=$(shell . $(TOP)eng/native/init-os-and-arch.sh && echo $${os})
AOT?=false
ILC_DA?=
LOG?=false

LOG_LEVEL=
LOG_MASK=
ifeq ($(LOG), true)
	LOG_LEVEL=debug
	LOG_MASK=aot
endif

MONO_ENV_OPTIONS ?=
ifeq ($(AOT), true)
	MONO_ENV_OPTIONS=--full-aot
endif

monoaottask:
	$(DOTNET) build -c Debug $(TOOLS_DIR)/AotCompilerTask/MonoAOTCompiler.csproj

publish: clean monoaottask
	$(DOTNET) publish \
	-c $(MONO_CONFIG) \
	-r $(TARGET_OS)-$(MONO_ARCH) \
	/p:RunAOTCompilation=$(AOT) \
	/p:RunIlcDependencyAnalysis=$(ILC_DA) \
	-bl

run: publish
	DOTNET_DebugWriteToStdErr=1 \
	MONO_ENV_OPTIONS="$(MONO_ENV_OPTIONS)" \
	MONO_LOG_LEVEL=$(LOG_LEVEL) \
	MONO_LOG_MASK=$(LOG_MASK) \
	$(TOP)artifacts/bin/HelloWorld/$(MONO_ARCH)/$(MONO_CONFIG)/$(TARGET_OS)-$(MONO_ARCH)/publish/HelloWorld

clean:
	rm -rf $(TOP)artifacts/bin/HelloWorld/
	rm -rf $(TOP)artifacts/obj/mono/HelloWorld/