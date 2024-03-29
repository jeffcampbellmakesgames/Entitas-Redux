# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Releases]
### [2.1.3] - 2022-04-20
#### Fixes
* Updated Genesis min package version to include bug fix for CLI installation paths with spaces in folder names when importing config contents
* Fixed bug with custom entity index generation where parts of code-gen were not being output.

#### Changes
* Added optional ENTITAS_REDUX_NO_SHARED_CONTEXT define to enable compiling out usage of `Contexts.SharedInstance`.
* Added `IContext.EntityIndices` to enable returning a read-only collection of all entity indexes for that context.

## [Releases]
### [2.1.2] - 2022-02-27
#### Fixes
* Updated Genesis min package version to include bug fix for CLI installation paths with spaces in folder names when generating code.

#### Changes
* Added author details to ER package manifest.

### [2.1.0] - 2021-07-13
#### Fixes
* Fixed several bugs with Entity Index code generation where index-related methods and code was not generated.

#### Changes
* Sealed attribute types EntityIndex, PrimaryEntityIndex, and CustomEntityIndex to ensure stable behavior for the EntityIndex code-generator.

### [2.0.1] - 2021-07-05
#### Fixes
* Updated Genesis package dependency min version to v2.3.2 to include several fixes for path issues

### [2.0.0] - 2021-06-16
#### Changed
* All code-generation plugins have been extracted from Unity as v2 Genesis .Net Core plugin assemblies (use Roslyn over Reflection).

## CL Template
This template should be used for any new releases to document any changes.

### [0.0.0] - 1900-01-01
#### Added
For new features.

#### Changed
For changes in existing functionality.

#### Deprecated
For soon-to-be removed features.

#### Removed
for now removed features.

#### Fixed
For any bug fixes.

#### Security
In case of vulnerabilities.