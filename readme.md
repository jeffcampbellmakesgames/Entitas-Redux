<a href="https://openupm.com/packages/com.jeffcampbellmakesgames.entitasredux/"><img src="https://img.shields.io/npm/v/com.jeffcampbellmakesgames.entitasredux?label=openupm&amp;registry_uri=https://package.openupm.com" /></a>
<img alt="GitHub issues" src="https://img.shields.io/github/issues/jeffcampbellmakesgames/EntitasRedux?style=flat-square">[![License: MIT](https://img.shields.io/badge/License-MIT-blue.svg)](https://opensource.org/licenses/MIT)
[![Twitter Follow](https://img.shields.io/badge/twitter-%40stampyturtle-blue.svg?style=flat&label=Follow)](https://twitter.com/stampyturtle)

<a href='https://ko-fi.com/I3I2W7GX' target='_blank'><img height='36' style='border:0px;height:36px;' src='https://cdn.ko-fi.com/cdn/kofi3.png?v=2' border='0' alt='Buy Me a Coffee at ko-fi.com' /></a>

# Entitas Redux

## About
Entitas Redux is a reworked version of [Entitas](https://github.com/sschmid/Entitas-CSharp) with a sole focus on Unity. For more background on this project, see [here](BACKGROUND.md) for more information.

## Requirements
* **Min Unity Version**: 2019.1
* **Scripting Runtime**: .Net 4.X

## Installing Entitas Redux
Using this library in your project can be done in three ways:

### Install via OpenUPM
The package is available on the [openupm registry](https://openupm.com/). It's recommended to install it via [openupm-cli](https://github.com/openupm/openupm-cli).

```
openupm add com.jeffcampbellmakesgames.entitasredux
```

### Install via GIT URL
Using the native Unity Package Manager introduced in 2017.2, you can add this library as a package by modifying your `manifest.json` file found at `/ProjectName/Packages/manifest.json` to include it as a dependency. See the example below on how to reference it.

```
{
	"dependencies": {
		...
		"com.jeffcampbellmakesgames.genesis" : "https://github.com/jeffcampbellmakesgames/genesis.git#release/stable",
		"com.jeffcampbellmakesgames.entitasredux" : "https://github.com/jeffcampbellmakesgames/entitasredux.git#release/stable",
		...
	}
}
```


You will need to have Git installed and available in your system's PATH.

### Install via classic `.UnityPackage`
The latest release can be found [here](https://github.com/jeffcampbellmakesgames/entitasredux/releases) as a UnityPackage file that can be downloaded and imported directly into your project's Assets folder.

**Note**: If you install Entitas using a `.UnityPackage` you must also grab the latest release of `Genesis` as well from [here](https://github.com/jeffcampbellmakesgames/genesis.git) as it is not included. For better dependency handling, I would recommend using OpenUPM instead.

## Usage

Entitas Redux at the moment is largely representative of the features present in `v1.13.0` of Entitas. As a temporary solution for now, I would point you to the Entitas Wiki [here](https://github.com/sschmid/Entitas-CSharp/wiki/Tutorials) for a breakdown on features and documentation on usage. In the future, EntitasRedux documentation and code examples will be hosted on this repository.

## Support
If this is useful to you and/or you’d like to see future development and more tools in the future, please consider supporting it either by contributing to the Github projects (submitting bug reports or features and/or creating pull requests) or by buying me coffee using any of the links below. Every little bit helps!

[![ko-fi](https://www.ko-fi.com/img/githubbutton_sm.svg)](https://ko-fi.com/I3I2W7GX)

## Contributing

For information on how to contribute and code style guidelines, please visit [here](CONTRIBUTING.md).
