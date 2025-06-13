//Israel OrtizRodriguez  |  GitHub: DigiFox
//Handles the SQLite workload so the app can try and keep clean.

using Microsoft.Data.Sqlite;
using System;
using System.IO;

namespace PitchVision360
{
    internal static class DatabaseHelper
    {
        private const string DbFile = "pitchvision.db";

        // call once at program start
        public static void EnsureDatabase()
        {
            if (!File.Exists(DbFile)) Console.WriteLine("Creating new database…");
            using var conn = Open();
            var cmd = conn.CreateCommand();

            cmd.CommandText = @"
            CREATE TABLE IF NOT EXISTS Roster (
                PlayerId      INTEGER PRIMARY KEY,
                Name          TEXT    NOT NULL,
                JerseyNumber  INT     NOT NULL,
                Position      TEXT,
                SchoolYear    TEXT,
                Team          TEXT
            );

            CREATE TABLE IF NOT EXISTS Events (
                EventId     INTEGER PRIMARY KEY AUTOINCREMENT,
                Minute      INT     NOT NULL,
                PlayerId    INT     NOT NULL,
                ActionType  TEXT    NOT NULL,
                Team        TEXT,
                MatchDate   TEXT,
                FOREIGN KEY (PlayerId) REFERENCES Roster(PlayerId)
            );";
            cmd.ExecuteNonQuery();
        }

        public static void InsertPlayer(Player p, string team)
        {
            using var conn = Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
              INSERT OR IGNORE INTO Roster
              (PlayerId, Name, JerseyNumber, Position, SchoolYear, Team)
              VALUES ($id, $name, $jersey, $pos, $year, $team);";
            cmd.Parameters.AddWithValue("$id",     p.PlayerId);
            cmd.Parameters.AddWithValue("$name",   p.Name);
            cmd.Parameters.AddWithValue("$jersey", p.JerseyNumber);
            cmd.Parameters.AddWithValue("$pos",    p.Position);
            cmd.Parameters.AddWithValue("$year",   p.SchoolYear);
            cmd.Parameters.AddWithValue("$team",   team);
            cmd.ExecuteNonQuery();
        }

        public static void InsertEvent(MatchEvent e, DateTime matchDate)
        {
            using var conn = Open();
            var cmd = conn.CreateCommand();
            cmd.CommandText = @"
              INSERT INTO Events
              (Minute, PlayerId, ActionType, Team, MatchDate)
              VALUES ($min, $pid, $act, $team, $date);";
            cmd.Parameters.AddWithValue("$min",  e.Minute);
            cmd.Parameters.AddWithValue("$pid",  e.Actor.PlayerId);
            cmd.Parameters.AddWithValue("$act",  e.Action.ToString());
            cmd.Parameters.AddWithValue("$team", e.Team);
            cmd.Parameters.AddWithValue("$date", matchDate.ToString("yyyy-MM-dd"));
            cmd.ExecuteNonQuery();
        }

        private static SqliteConnection Open()
        {
            var conn = new SqliteConnection($"Data Source={DbFile}");
            conn.Open();
            return conn;
        }
    }
}
