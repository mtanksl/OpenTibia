# What is this?

It is an open Tibia server written in C# from scratch. Base protocol is 8.60. Experimental support for other protocols, ranging from 7.72 to 8.70.

# Why another one?

I believe one can only learn by doing. 

# Notes before proceding

This project is really old, I did it as a tribute to the open Tibia community. 
From time to time, I take this project to have some fun and recall the good old days.

![Server and Client](/server.png)

![Debugging .lua scripts](/debugging-lua-scripts.png)

# Running the server

### Option 1 - Running on Windows (GUI)

- Download the last GUI release asset [mtanksl.OpenTibia.Host.GUI_win-x64](https://github.com/mtanksl/OpenTibia/releases)
- Extract and run server

### Option 2 - Running on Windows (console)

- Download the last release asset [mtanksl.OpenTibia.Host_win-x64](https://github.com/mtanksl/OpenTibia/releases)
- Extract and run server

### Option 3 - Running on Linux (console)

- Download the last release asset [mtanksl.OpenTibia.Host_linux-x64](https://github.com/mtanksl/OpenTibia/releases)
- Extract 
- Allow executing file as program permission
```
chmod +x ./mtanksl.OpenTibia.Host
```
- Run server
```
./mtanksl.OpenTibia.Host
```
- Sqlite database location: `\mtanksl.OpenTibia.GameData\data\database.db`.
- Server configuration file location: `\mtanksl.OpenTibia.GameData\data\server\config.lua`.

# Connecting to the server

- Use Tibia 8.6 client (download below) with an IP changer (download also below) to connect to localhost (IP address 127.0.0.1) on port 7171.
- Use account number `1` and password `1` to enter game.

### Tibia 8.6 client

You can download the official release direct from here [4Shared](https://www.4shared.com/s/fVTbjUnjCiq).

### IP changer 

You can use [jo3bingham's IP changer](https://github.com/jo3bingham/tibia-ip-changer), [OtLand IP changer](https://otland.net/threads/otland-ip-changer.134369/) or download direct from here [4Shared](https://www.4shared.com/s/f2VQahgxIiq).

# Troubleshoot

> MSB3821 Couldn't process file MainForm.resx due to its being in the Internet or Restricted zone or having the mark of the web on the file. Remove the mark of the web if you want to process these files.

Go to `\mtanksl.OpenTibia.Host.GUI\MainForm.resx` file in Windows File Explorer. Right-click and select properties. At the bottom of the the dialog is an "unblock" option.

> How do I enable lua debugger?

Lua debugger is disabled by default. On Windows, go to `\mtanksl.OpenTibia.GameData\data\lualibs\` and rename the file `_mobdebug.lua` to `mobdebug.lua`. See [How to debug scripts](https://github.com/mtanksl/OpenTibia/wiki/lua-how-to-debug) and [How to add autocomplete](https://github.com/mtanksl/OpenTibia/wiki/lua-how-to-add-autocomplete) for additional info.

# Other resources

### Map editor (.otbm)

You can use [Remere's map editor](https://github.com/hampusborgos/rme).

### SQLite database manager (.db)

You can use [SQLiteStudio](https://github.com/pawelsalawa/sqlitestudio/releases).

### Item editor (.dat and .spr)

You can use Blackdemon's Tibia editor, download direct from here [4Shared](https://www.4shared.com/s/fYbs_yvrrge).

### Item editor (.otb)

You can use [OTItemEditor](https://github.com/opentibia/item-editor).

### Lua IDE (.lua)

You can use [ZeroBrane Studio IDE](https://studio.zerobrane.com).

### Tibia 7.72 client

You can download the official release direct from here [4Shared](https://www.4shared.com/s/fFuN0IXb6ge).

### Tibia 8.74 client

You can download the official release direct from here [4Shared](https://www.4shared.com/s/fDNUFuISgku).

# Project's wiki

If you want to know what is implemented and how it works, please, visit the [Project's wiki](https://github.com/mtanksl/OpenTibia/wiki).

# Support Us

If you enjoy using open source projects and would like to support our work, consider making a donation! Your contributions help us maintain and improve the project. You can support us by sending directly to the following address:

__Bitcoin (BTC) Address:__ bc1qc2p79gtjhnpff78su86u8vkynukt8pmfnr43lf

__Monero (XMR) Address:__ 87KefRhqaf72bYBUF3EsUjY89iVRH72GsRsEYZmKou9ZPFhGaGzc1E4URbCV9oxtdTYNcGXkhi9XsRhd2ywtt1bq7PoBfd4

Thank you for your support! Every contribution, no matter the size, makes a difference.