# What is this?

It is an open Tibia server, a simple one, written in C# from scratch. 

# Why another one?

I believe one can only learn by doing. 

# Notes before proceding

This project is really old, I did it as a tribute to the open Tibia community. 
From time to time, I take this project to have some fun and recall the good old days.

# First run

- You need Visual Studio 2022 and .NET 7.
- Download and extract the files.
- Open `mtanksl.OpenTibia.sln`.
- Set `mtanksl.OpenTibia.Host` or `mtanksl.OpenTibia.Host.GUI` as startup project.
- Press F5 to run the server. 
- Use Tibia 8.6 client (download below) with an IP Changer (download also below) to connect to localhost (IP address 127.0.0.1) on port 7171.
- Use account number `1` and password `1` to enter game.
- Sqlite database location: `\bin\Debug\...\data\database.db`.

![Server and Client](/server.png)

# Troubleshoot

> MSB3821 Couldn't process file MainForm.resx due to its being in the Internet or Restricted zone or having the mark of the web on the file. Remove the mark of the web if you want to process these files.

Go to MainForm.resx file in Windows File Explorer. Right-click and select properties. At the bottom of the the dialog is an "unblock" option.

> Account name or password is not correct.

Use account number `1` and password `1` to enter game.

# Resources

### Tibia 8.6 client

You can download the official release direct from here [4Shared](https://www.4shared.com/s/fVTbjUnjCiq).

### IP changer 

You can use [jo3bingham's IP changer](https://github.com/jo3bingham/tibia-ip-changer), [OtLand IP changer](https://otland.net/threads/otland-ip-changer.134369/) or download direct from here [4Shared](https://www.4shared.com/s/f2VQahgxIiq).

### Map editor (.otbm)

You can use [Remere's map editor](https://github.com/hampusborgos/rme).

### SQLite database manager (.db)

You can use [SQLiteStudio](https://github.com/pawelsalawa/sqlitestudio/releases).

### Item editor (.dat and .spr)

You can use Blackdemon's Tibia editor, download direct from here [4Shared](https://www.4shared.com/s/fYbs_yvrrge).

### Item editor (.otb)

You can use [OTItemEditor](https://github.com/opentibia/item-editor).

# Project's wiki

If you want to know what is implemented and how it works, please, visit the [Project's wiki](https://github.com/mtanksl/OpenTibia/wiki).