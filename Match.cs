//Author: Israel OrtizRodriguez  |  GitHub: MrDigifox
//The big cheese: holds rosters, events, and can spit out CSVs so coaches don’t have to decode my handwriting.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace PitchVision360
{
    public class Match
    {
        public DateTime MatchDate { get; }
        public string   HomeTeam  { get; }
        public string   AwayTeam  { get; }

        private readonly List<Player>     _homeRoster = new();
        private readonly List<Player>     _awayRoster = new();
        private readonly List<MatchEvent> _events     = new();

        public Match(DateTime date, string home, string away)
        {
            MatchDate = date;
            HomeTeam  = home;
            AwayTeam  = away;
        }
        public void AddPlayer(Player p, bool isHome) =>                                                  //roster and event helpers
            (isHome ? _homeRoster : _awayRoster).Add(p);

        public void AddEvent(MatchEvent e) => _events.Add(e);

        public bool PlayerExists(string name) =>
            _homeRoster.Concat(_awayRoster)
                       .Any(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public Player FindPlayer(string name) =>
            _homeRoster.Concat(_awayRoster)
                       .First(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase));

        public string GetLiveSummary()                                                                  //live summary.
        {
            int homeGoals = _events.Count(e => e.Team == HomeTeam && e.Action == ActionType.Goal);
            int awayGoals = _events.Count(e => e.Team == AwayTeam && e.Action == ActionType.Goal);

            var sb = new StringBuilder();
            sb.AppendLine($"{HomeTeam} {homeGoals} – {awayGoals} {AwayTeam}");
            sb.AppendLine(new string('-', 30));
            sb.AppendLine("Min  Player           Action");
            foreach (var ev in _events.OrderBy(e => e.Minute))
                sb.AppendLine($"{ev.Minute,3}  {ev.Actor.Name,-15} {ev.Action}");
            return sb.ToString();
        }

        public void SaveFiles(string folder = "Data")                                                   //save to /Data as CSV 
        {
            Directory.CreateDirectory(folder);

            var rosterPath  = Path.Combine(folder, "Roster.csv");                                       //Roster.csv
            var rosterLines = _homeRoster.Concat(_awayRoster).Select(p =>
                $"{p.PlayerId},{p.Name},{p.JerseyNumber},{p.Position},{p.SchoolYear},{( _homeRoster.Contains(p) ? HomeTeam : AwayTeam )}");
            File.WriteAllLines(rosterPath, rosterLines);

            var eventsPath = Path.Combine(folder, "Events.csv");                                        //Events.csv
            var eventLines = _events.Select((e, i) =>
                $"{i + 1},{e.Minute},{e.Actor.PlayerId},{e.Action},{e.Team},{MatchDate:d}");
            File.WriteAllLines(eventsPath, eventLines);
        }
    }
}
