# Changelog
All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Releases]
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