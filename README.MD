# Project Pokémon Pokédex
A work-in-progress Pokédex for various games.

## Supported Games

* Pokémon Sun/Moon
* Pokémon Mystery Dungeon: Explorers of Sky
* Pokémon Super Mystery Dungeon

## Dependencies

* DotNet3dsToolkit 2
* IPS-Pages-Publisher.Interfaces
* Newtonsoft.Json
* pk3DS
* SkyEditor.ROMEditor

## Requirements

* .Net Framework 4.6.2

## Building

Build using Visual Studio 2017. 2015 might work but is unsupported. The following NuGet packages need to be included:

Project Pokémon MyGet:
```
https://www.myget.org/F/projectpokemon/api/v3/index.json
```

Sky Editor MyGet:
```
https://www.myget.org/F/skyeditor/api/v3/index.json
```

## Usage
Current usage is as follows:

```
ProjectPokemonPokedex.exe <eosPath> <psmdPath> <smPath> <ultraSmPath> <OutputFilename>
```

The output of this program is a JSON text file that will be used by a closed-source program to publish to Project Pokémon.