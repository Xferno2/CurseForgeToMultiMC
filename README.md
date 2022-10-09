# CurseForgeToMultiMC
A tool to convert CurseForge instances to MultiMC instances and make them easy to use with Steam ROM Manager

### MultiMCToSteamRomManager
This separate project just copies the MultiMC instance icon to a separate folder and make it ready to be used by Steam ROM Manager at least for now. I plan to make a way to automatically add a Controller support for instances that have a modloader, in case you use Steam Big Picture as I do. Please use this [config](https://github.com/Xferno2/CurseForgeToMultiMC/blob/master/SteamROMManagerConfig.txt) inside Steam ROM Manager to get the instances imported. Just reaplce ${yourUserAccount} with your Steam username and every [MULTIMC LOCATION HERE] with your MultiMC install path.

## Why does this project exist?
Recently I centralized all of my games and ROMs into Steam using Steam ROM Manager. All of my modded Minecraft instances were on CurseForge which I soon realized has no way to launch an instance with arguments or create a shortcut for that instance. Therefore I wanted to migrate everything to MultiMC which supports all of that.
This project automates that entire procedure, also using junctions to link Curse and MultiMC instances so if you edit something in Curse or MultiMC it reflects in both of them.

### Links to all mentions
[SteamROMManager](https://github.com/SteamGridDB/steam-rom-manager)

[CurseForge](https://www.curseforge.com/)

[MultiMC](https://multimc.org/)

[My Steam ROM Manager parser config](https://github.com/Xferno2/CurseForgeToMultiMC/blob/master/SteamROMManagerConfig.txt)
