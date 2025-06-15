# Pitch Vision 360 – High-School Match Logger

> **Tech:** C# • .NET 9 • SQLite  
> **Release tag:** `Final`

## Features
* Command-line roster & event logging
* Live in-game summary table
* Exports CSV files _and_ a local SQLite database (`pitchvision.db`)
* Simple class model: Player, MatchEvent, Match, DatabaseHelper

## Run locally
```bash
git clone https://github.com/MrDigifox/PitchVision360
cd PitchVision360
dotnet restore
dotnet run

Pitch Vision 360 lets student managers log goals, cards, assists and substitutions in real time.
It prints a running score-board, then saves everything to CSV and SQLite so coaches can analyze matches later.

Demo Video
📺 https://youtu.be/-YBTdxKT4bw
