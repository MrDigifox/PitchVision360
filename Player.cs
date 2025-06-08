// Author: Israel OrtizRodriguez  |  GitHub: MrDigifox
// Super-simple class that holds player info.
// Think of it like a contact card but for soccer stats.

namespace PitchVision360
{
    public class Player
    {
        // ID is just an auto counter so we can link events fast.
        public int    PlayerId      { get; init; }
        public string Name          { get; init; }
        public int    JerseyNumber  { get; init; }
        public string Position      { get; init; }
        public string SchoolYear    { get; init; }  // freshman, sophomore, etc.

        public Player(int id, string name, int jersey, string position, string schoolYear)
        {
            PlayerId     = id;
            Name         = name;
            JerseyNumber = jersey;
            Position     = position;
            SchoolYear   = schoolYear;
        }

        // Prints like #10 Messi (FWD) so it looks nice in the console. (FWD) is the postion abbreviation which the area will be reflected in the different types of positions.
        public override string ToString() => $"#{JerseyNumber} {Name} ({Position})";
    }
}
