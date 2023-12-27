> [!WARNING]  
> Disclaimer: This translation was generated using AI.

# What's New in Minecraft Holy Client *2.0.0-preview.1*?

## Major Fixes

This section includes the following subsections:

- [Bug Fixes](#bug-fixes)
- [Performance](#performance)
- [Stress Test](#stress-test)
- [Installer](#installer)

### Bug Fixes

#### Bot Counter

There was a minor issue where, during the stress test process, the bot count would spike, inaccurately portraying more bots than actually present. The bot counter now accurately reflects the current number of online bots.

#### Bot Restarts

In previous versions, bots might fail to restart in situations such as unsuccessful attempts to connect to a proxy, bots exiting the server, or internal errors during operation. Currently, bots correctly execute the restart process in such scenarios.

#### Socks5 Proxy

Previously, bots were unable to connect to socks5 proxies due to a protocol implementation error. Bots can now successfully connect to socks5 proxies.

### Performance

Performance is a crucial aspect of Minecraft Holy Client, with each feature designed for maximum efficiency. Additionally, the following enhancements have been implemented in Minecraft Holy Client, specifically geared towards improving performance:

- Drastically reduced RAM consumption. Previously, instances of MinecraftPrimitiveReader and MinecraftPrimitiveWriter were created for each test bot, and memory was allocated for each new packet received from the server. Now, we actively use an object pool for reusing MinecraftPrimitiveReader and MinecraftPrimitiveWriter.

- Increased throughput. Bots in Play mode now separate the processes of reading data from the network and parsing packets into two threads, using the System.IO.Pipelines library. This has resulted in a 10% increase in throughput.

- Optimized proxy servers. QuickProxyNet now efficiently manages memory when connecting to Socks4, Socks4a, and Socks5 proxy servers.

### Stress Test

#### Default Proxies

Now, default proxies are used in the stress test if the user hasn't added their own proxies. This feature was added based on a [Discord poll](https://discord.com/channels/1166051202367246396/1166055869566419085/1172042445928812574).

#### Default Plugins

Previously, there was an option to create behavior for stress tests using plugins, which might have been challenging for regular users. Now, a new default plugin has been introduced, simplifying the process of launching stress tests and making it accessible to everyone. It's worth noting that the default plugin currently only runs simple bots that register on the server using /reg \<pass\> \<pass\> and immediately start spamming. The plugin's functionality will be expanded in the future.

#### Proxy Loading from Clipboard and URL

Previously, proxies could only be loaded from a file. Now, proxies can be imported in three ways: file, URL, and manual entry of the proxy list.

#### Proxy Disabling

In the main proxy settings, the option to disable the use of proxies has been added. Bots will use the user's IP.

#### Stress Test Process Window Design

The stress test process window no longer displays a graph of the number of online bots and connections per second. Instead, a logger has been added. Online bot and connections per second indicators have been moved to the top, and a peak connections per second indicator has been added.

### Installer

The program is now fully cross-platform and portable, so the ClickOnce installer on Windows is no longer supported.
