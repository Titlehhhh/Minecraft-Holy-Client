> 🇷🇺 [Читать на русском](./ru.md)

# Project Status — Holy Client

**Last update:** February 6, 2024  
**Current status:** Not maintained

## Why development has stopped

The **Minecraft Holy Client** has not received updates for over a year.  
Originally written in C#, the project's architecture became increasingly unmaintainable, and adding new features turned into a technical nightmare due to accumulated legacy code and poor scalability.

In December 2024, I decided to fully switch focus to developing the library **[McProtoNet](https://github.com/Titlehhhh/McProtoNet)**.  
By around February 2025, I implemented full multi-version support and simplified the architecture. The library is now in a stable, usable state.

## What's next?

Once McProtoNet became solid, I reevaluated the future of Holy Client and concluded that rewriting it from scratch would be the best path forward.  
Instead of the original monolithic design, I'm building a **new client as a client-server application with a web interface**. This also helps me deepen my experience with modern web technologies.

At the moment, I’m fully focused on this **new version of the client**.  
Until it is ready and released as open-source, the original Holy Client will remain **frozen and will not be updated**.

## FAQ

**Will there be an open-source version?**  
Yes, once it's ready for general use.

**Why is development taking so long?**  
This is a side project I work on in my free time, alongside studies and personal commitments.

**How can development be accelerated?**  
By contributing code, reporting issues, writing documentation, or supporting the project financially (see [Support](../README.md#-support-the-project)). Every contribution helps move things forward faster.

**What features will the new version have?**
- Multi-version support (1.12.2 up to the latest version)
- Attack modes (BotJoiner, Memory)
- Better feedback from stress tests

These are the core features I plan to include. I can't promise anything, but my goal is to release something during the summer.

**Will there be desktop GUI support?**  
I’m still considering this. My new architecture allows for it, but it could complicate development since I’d have to maintain two GUIs in parallel.
Maybe someone else will take on this task in the future — perhaps even create a Minecraft mod.
That would be a great learning experience.
I’ll do my best to provide proper API documentation.

---

## Screenshots (Work in progress)

*Some data shown may be fake (e.g., proxy IPs)*

![img_1.png](img_1.png)

![img_2.png](img_2.png)

![img_3.png](img_3.png)

![img_4.png](img_4.png)

