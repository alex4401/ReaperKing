# Reaper King
A static site generator built entirely in C# and integrated with Razor.

## Usage
To build a site, a static site configuration and a project configuration are required.

Once you have access to one, run the builder program (`ReaperKing.Builder`) with appropriate
command line parameters.

| Parameter Name | Description                                                 | Required? |
| -------------- | ----------------------------------------------------------- | --------- |
| *positional*   | Name of the project's static configuration assembly.        | Yes       |
| environment    | Project configuration environment name.                     | Yes       |
| assembly-path  | Path to the directory which contains project assemblies.    | Yes       |
| skip-pre-build | Disables execution of the pre-build tasks.                  | No        |

### Static Site Configuration
The static site configuration assembly contains a *build recipe*, a class which inherits from
`ReaperKing.Core.Site` and manages the build process through `PreBuild`, `Build` and `PostBuild`
methods.

Content may be emitted by calling `EmitDocument(IDocumentGenerator)` or
`EmitDocumentsFrom(ISiteContentProvider)`. Content Providers may emit documents through similar
methods found on the `SiteContext` they receive for their routines.

You may choose to split the recipe from generators by moving them to a separate assembly and
using its types in the primary assembly.

### Engine Modules
*Build recipes* may also attach special modules to the engine, which get notified of certain
events happening during the build process and sometimes may modify their state.

Several modules are bundled with the engine and may be attached at will:

* **Document Collection:** Collects information about generated documents for later use (e.g.
                           for sitemap generation).
* **Sitemap Exclusion:**   Allows exclusion of documents generated in a specific block from
                           being included in sitemaps.
* **Uglify HTML:**         Self-explanatory.
* **Image Optimization:**  Automatically optimizes JPEGs and PNGs.

## Licensing
Reaper King is free software: you can redistribute and/or modify it under the terms of the
[GNU General Public License](LICENSE) as published by the Free Software Foundation, either
version 3 of the License, or (at your option) any later version.

Projects `ReaperKing.Generation.ARK`, `ReaperKing.Generation.Tools` and
`ReaperKing.StaticConfig` are covered by the [GNU Affero General Public License](LICENSE.PERSO),
either version 3 of the License, or (at your option) any later version.

Additional non-free modules may eventually be freed at later date.