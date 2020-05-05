# Goals for Entitas Redux
I had several goals as I started developing this re-envisioned version of Entitas.

* _I wanted a version of Entitas I could more actively add features, bug fixes, and improvements to_.  At the time, Entitas had not received active development for the better part of a year.

* _I wanted a more straightforward development environment with all source provided_. While Entitas was largely open-source, there were core parts of the code-generation framework that did not have source code provided. I've written or maintained several code-gen tools previously and so to make this a fully-open source library (and split up responsibilities between code generator and ECS framework) I've adapted the best parts of the original Entitas code-gen and several internal solutions into a new general-purpose code generation tool for Unity named [Genesis](https://github.com/jeffcampbellmakesgames/Genesis). This can be freely used, forked, and adapted for whatever code-gen purposes in Unity you might need, and is a core dependency of Entitas Redux.

* _I wanted increased iteration times and a simpler way to extend and build upon the API_. Entitas was originally setup as a plain-old CSharp solution where the code-base was split between many assemblies, including optional Unity support and some, but not all unit. When making changes, these assemblies needed to be recompiled and published to a Unity project along with any included scripts to be tested. The abstract setup for me was increasing iteration time, making it harder to debug issues in the Unity Editor, and extending functionality in small ways required referencing many assemblies to do so. As a result, I have reworked the development environment, now consisting of a Unity project containing all of the reworked EntitasRedux framework code, unit tests, and example content. In this way, it is easy to test

* _I wanted to be able to immediately test changes using all unit tests and example content_. The original CSharp solution contained a majority of unit tests, but not all as there were Unity specific-tests in several scattered Unity projects). The majority of unit tests themselves were written using a unit test library named NSpec, but Unity primarily uses NUnit. In order to have a single Unity development project able to run all unit tests (edit-mode, play-mode), I ported all of the original NSpec unit tests to NUnit and imported and reworked all disparate Unity test content into a single development project.

* _Singular focus on Unity_. The abstractions of the original development environment make a lot of sense when there is a desire to support non-Unity C# applications as the Unity integration is optional, but I had no such aspiration as I work fully in Unity. This has enabled me to collapse many of the previously-existing related assemblies into Runtime, Editor, and Plugin versions and remove functionality not pertinent for Unity such as server or CLI code-gen, CSharp project manipulation, among others. I can also now issue updates and releases easily as Packages, making it easy for downstream applications to always be on the latest version if they choose to.

### TLDR

* Entitas Redux is an updated, worked version of Entitas that is easy to develop for and against. All unit tests and example content are available in the same repository and distribution of new versions is now easy to do with a few clicks. 
* It has a more modern, updated C# code style.
* Easily extensible with only a few core `AssemblyDefinitions` for Runtime and Editor/Plugin code.
* Over time, I want to enhance the feature set even more for Unity developers and provide a more robust unit and performance testing suite for the framework.

### Outstanding Features

As I reworked Entitas, I focused on the unit tests firsts and then used those as a guide as I reworked the development environment and cleaned up the code base to reflect a more modern C# style (for code style guidelines, please see [here](CONTRIBUTING.md)). There were two significant features that while updated require further investigation before including them into the main library and have been left orphaned onto indidividual branches from `develop` for now.

* **Blueprints**: This was a feature intended to make it possible to configure Entities in the Unity Editor. In their original form they were no longer supported or included with official releases of Entitas and have been relegated to a beta state. See [here](https://github.com/sschmid/Entitas-CSharp/wiki/Entitas-Blueprints) for more information on their original intended use.

* **Migrations**: This was a CLI/Editor tool for migrating existing code to the latest Entitas API. This is something I am uncertain if I want to commit to at this time and don't largely see using myself, but did update so that it was compatible with the rest of the code-base if there was strong enough interest.