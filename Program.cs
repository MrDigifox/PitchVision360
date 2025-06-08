//Israel OrtizRodriguez  |  GitHub: MrDigifox
//CLI driver (command line interface) where we actually type stuff. No fancy UI yet, sorry.

//HOW TO USE THIS PROGRAM (for instructor and soccer nerds)
//(1.) Build & Run
//> dotnet build
//> dotnet run
//
//(2.) At launch you’ll be asked for the home and away team names.
//Example: "Home team?"  Fox FC and then another rival team pops up "Away team?"  ECPI
//
//(3.) Available commands during the match:
//  r <Name> <Jersey> <Pos> <Home/Away> ----- add player to a roster
//  g <Minute> <Name> ------------------------log a Goal
//  a <Minute> <Name> ------------------------log an Assist
//  y <Minute> <Name> ------------------------Yellow card
//  r <Minute> <Name> ------------------------Red card  (note: the letter is the same as add-roster, but inside event mode “r” is only valid after a minute)
//  s <Minute> <Name> ------------------------Substitution
//  summary ----------------------------------print live score + running event table
//  end --------------------------------------finalize match, print summary again, save CSV files
//
//(4.) Minimal example sequence of what to type for a run through:
//      > r Messi 10 FWD home
//      > r Neuer 1 GK away
//      > g 12 Messi
//      > y 60 Neuer
//      > summary
//      > end
//      After “end”, the program writes two files in a new /Data folder:
//      *Roster.csv (all players).
//      *Events.csv (each logged event with minute + action type).
//
//(5.) If a command is typed incorrectly the program prints:
//    “Uh-oh, that command makes zero sense.” and waits for the next input.
//
//(6.) No external data or database is required; everything runs in-memory
//    until ‘end’, then CSV files are generated for later review.
// HAVE FUN SOCCER NERDS....AND RESPECTFULLY THE INSTRUCTOR :}


using System;
using PitchVision360;

class Program
{
    static void Main()
    {
        Console.WriteLine("Pitch Vision 360 – Console Logger");
        Console.Write("Home team? "); string home = Console.ReadLine();
        Console.Write("Away team? "); string away = Console.ReadLine();

        var match = new Match(DateTime.Now, home, away);
        int nextId = 1;                                                                             //auto-increment for new players

        Console.WriteLine("\nCommands:");
        Console.WriteLine("  r <Name> <Jersey> <Pos> <Home/Away>");
        Console.WriteLine("  g|a|y|r|s <Minute> <Name>");
        Console.WriteLine("  summary");
        Console.WriteLine("  end\n");

        while (true)
        {
            Console.Write("> ");
            var raw = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(raw)) continue;

            var parts = raw.Split(' ', StringSplitOptions.RemoveEmptyEntries);
            string cmd = parts[0].ToLower();
         
            if (cmd == "r" && parts.Length == 5)                                                    //add roster entry
            {
                var p = new Player(nextId++, parts[1],
                                   int.Parse(parts[2]), parts[3], "N/A");
                bool isHome = parts[4].StartsWith("h", StringComparison.OrdinalIgnoreCase);
                match.AddPlayer(p, isHome);
                Console.WriteLine($"Added {p}");
            }
            
            else if ("g a y r s".Contains(cmd) && parts.Length == 3)                                //log events
            {
                if (!match.PlayerExists(parts[2]))
                {
                    Console.WriteLine("Player not found – add them first.");
                    continue;
                }
                ActionType action = cmd switch                                                      
                {
                    "g" => ActionType.Goal,
                    "a" => ActionType.Assist,
                    "y" => ActionType.YellowCard,
                    "r" => ActionType.RedCard,
                    _   => ActionType.Substitution
                };
                var ev = new MatchEvent(
                    int.Parse(parts[1]),
                    match.FindPlayer(parts[2]),
                    action,
                    cmd == "s" ? away : home
                );
                match.AddEvent(ev);
                Console.WriteLine(ev);
            }
            else if (cmd == "summary")                                                              //live summary
            {
                Console.WriteLine(match.GetLiveSummary());
            }
            else if (cmd == "end")                                                                  //end match
            {
                Console.WriteLine("\nFinal Summary");
                Console.WriteLine(match.GetLiveSummary());
                match.SaveFiles();
                Console.WriteLine("Saved roster and events to /Data. Peace!");
                break;
            }
            else
            {
                Console.WriteLine("Uh-oh, that command makes zero sense.");
            }
        }
    }
}
