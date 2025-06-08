// Author: Israel OrtizRodriguez  |  GitHub: MrDigifox
// Logs a single thing that happened in the match – kind of like a tweet.
// but only soccer nerds like me care. Could be used for other sports for those respective nerds.

namespace PitchVision360
{
    public class MatchEvent
    {
        public int        Minute  { get; }
        public Player     Actor   { get; }
        public ActionType Action  { get; }
        public string     Team    { get; }

        public MatchEvent(int minute, Player actor, ActionType action, string team)
        {
            Minute = minute;
            Actor  = actor;
            Action = action;
            Team   = team;
        }

        // Example: 65’ Smith – Goal (Home)
        public override string ToString() =>
            $"{Minute:00}’ {Actor.Name} – {Action} ({Team})";
    }
}
