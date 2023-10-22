import { dotnet } from './dotnet.js'

const is_browser = typeof window != "undefined";
if (!is_browser) throw new Error(`Expected to be running in a browser`);

const dotnetRuntime = await dotnet
    .withDiagnosticTracing(true)
    .withApplicationArgumentsFromQuery()
    .create();


const config = dotnetRuntime.getConfig();

const exports = await dotnetRuntime.getAssemblyExports(config.mainAssemblyName);

window.addEventListener('beforeunload', (event) => {
    exports.HolyClient.Browser.Interop.SaveSettings();
});

const terminationEvent = 'onpagehide' in self ? 'pagehide' : 'unload';

window.addEventListener(terminationEvent, (event) => {
    exports.HolyClient.Browser.Interop.SaveSettings();
});

await dotnetRuntime.runMainAndExit(config.mainAssemblyName, [window.location.search]);