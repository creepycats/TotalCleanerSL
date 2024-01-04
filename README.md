[![Github All Releases](https://img.shields.io/github/downloads/creepycats/TotalCleanerSL/total.svg)](https://github.com/creepycats/TotalCleanerSL/releases) [![Maintenance](https://img.shields.io/badge/Maintained%3F-yes-green.svg)](https://github.com/creepycats/TotalCleanerSL/graphs/commit-activity) [![GitHub license](https://img.shields.io/github/license/Naereen/StrapDown.js.svg)](https://github.com/creepycats/TotalCleanerSL/blob/main/LICENSE)
<a href="https://github.com/creepycats/TotalCleanerSL/releases"><img src="https://img.shields.io/github/v/release/creepycats/TotalCleanerSL?include_prereleases&label=Release" alt="Releases"></a>
<a href="https://discord.gg/PyUkWTg"><img src="https://img.shields.io/discord/656673194693885975?color=%23aa0000&label=EXILED" alt="Support"></a>

# TotalCleaner
An SCP:SL Exiled Plugin that adds a few utilities to manually and automatically clean up Items and Ragdolls.

Made for `v13.3.1+` of SCP:SL and `v8.6.0` of Exiled and onward by creepycats.

## Features
- Distance-Based Loading and Unloading.
  - Items far away from players are not needed, and are instead discarded and respawned when a player enters their range
  - Ragdolls too far from players automatically despawn
- Ammo Clumping : Ammo Boxes dropped on top of each other combine into a single box with the collective amount of Ammo. This is done to save on items spawned at a given moment
- Decontamination and Warhead Auto-Zone Cleanups
- Configurable Broadcasts for cleaning
- Admin Commands to Clean specific Items from a certain Zone, as well as ragdolls
- Optional Debug UI (`.tcui` Command)

## Admin Commands
- `tclean items <zone/all> <optional list of case-specific items/ids>`
- `tclean ragdolls <zone/all>`

## Installation
Nothing special, install the DLL like a normal plugin in `Exiled/Plugins`

## Permissions
`totalcleaner.ui` : Optional, Allows the usage of `.tcui` to view information on Item and Ragdoll Loading and deletions
`totalcleaner.commands` : Gives admins access to the TotalCleaner manual commands for cleaning items and ragdolls in certain zones
