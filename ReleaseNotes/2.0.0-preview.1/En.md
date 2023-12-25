# What's New in Minecraft Holy Client *2.0.0-preview.1*?

## Major Fixes

This section includes the following subsections:

- [Bug Fixes](#bug-fixes)
- [Performance](#performance)
- [Stress Test](#stress-test)
- [Installer](#installer)

### Bug Fixes

#### Bot Counter

There was a minor issue where, during stress testing, the bot count would spike inaccurately, showing more bots than actually present. The bot online counter now accurately reflects the correct value.

#### Bot Restart

In previous versions, bots might fail to restart in situations like unsuccessful proxy connection attempts, bots exiting the server, or encountering internal errors during operation. Currently, bots reliably initiate the restart process in such scenarios.

#### Socks5 Proxy

Previously, bots couldn't connect to Socks5 proxies due to a protocol implementation error. Now, bots can successfully connect to Socks5 proxies.

### Performance

Performance is a crucial aspect of Minecraft Holy Client, and each feature is designed with maximum efficiency in mind. Additionally, the following enhancements have been incorporated into Minecraft Holy Client to specifically boost performance:

- Significantly reduced RAM usage. Previously, new instances of MinecraftPrimitiveReader and MinecraftPrimitiveWriter were created for each test bot, and memory was allocated for each new packet received from the server. Now, we actively utilize object pooling to reuse MinecraftPrimitiveReader and MinecraftPrimitiveWriter.

- Increased throughput. Play mode bots now separate the processes of reading data from the network and syntactic analysis of packets into two threads using the System.IO.Pipelines library. This has led to a 10% increase in throughput.

- Optimized proxy servers. QuickProxyNet now efficiently manages memory when connecting to Socks4, Socks4a, and Socks5 proxy servers.

### Stress Test

#### Default Proxies

Stress tests now use default proxies if the user has not added custom ones. This feature was added based on [Discord community voting](https://discord.com/channels/1166051202367246396/1166055869566419085/1172042445928812574).

#### Default Plugins

Previously, users had the option to create behavior for stress tests using plugins, which might have seemed complex for regular users. Now, a new default plugin has been introduced to simplify the process of running stress tests and make it accessible to everyone. It's worth noting that the default plugin currently only launches simple bots that register on the server using /reg \<pass\> \<pass\> and immediately start spamming. Additional functionality will be added to the plugin in the future.

#### Loading Proxies from Clipboard and URL

Previously, proxies could only be loaded from a file. Now, proxies can be imported in three ways:
- File
- URL
- Manual input of the proxy list

#### Proxy Disable Option

In the main proxy settings, an option has been added to disable the use of proxies; bots will use the user's IP.

#### Stress Test Window Design

The stress test window no longer includes a graph of the number of online bots and connections per second. Instead, a logger has been added. Online bot and connections per second indicators have been moved to the top, and a peak connections per second indicator has been added.

### Installer

The program is now fully cross-platform and portable, so the ClickOnce installer on Windows is no longer supported.
