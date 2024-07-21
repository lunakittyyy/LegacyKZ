# Only works in very old Gorilla Tag versions, on or around the Body Tag update.


# Credits
- most code and maps by Graic and SneakyEvil

# Map specifications
- All map objects must be under an Empty named "Map", and its prefab needs to be named "Map" also
- An Empty named "Spawn" must be placed where the player needs to be spawned
- Map info component can have map names, speed multipliers, gamemodes, etc.
  - Checkpoints and time trials will only be enabled if gamemode is "gkz". Make a map any other gamemode to make it act like a generic map without any time trial features
- Start point must be a collider named "Start" surrounding the start area
  - Timer starts when *exiting* the start area and resets when entering it
- End points must be a collider named "End" surrounding the end area
