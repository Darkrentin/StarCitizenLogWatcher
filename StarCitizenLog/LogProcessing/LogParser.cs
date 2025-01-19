using System.Text.RegularExpressions;

namespace StarCitizenLog.LogProcessing;

public class LogParser
{
    
    public static string ProgramLogFilePath = "log.txt";
    
    public static List<LogEvent> Events = new List<LogEvent>();
    public static List<Player> Players = new List<Player>();

    public static void InitLog()
    {
        File.WriteAllText(ProgramLogFilePath, $"[{DateTime.Now}] Start Log\n");
    }
    
    public static List<string> ParseLog(string log)
    {
        List<string> lines = new List<string>();
        lines = log.Split("\n").ToList();

        for (int i = 0; i < lines.Count; i++)
        {
           Console.WriteLine($"Line {i}: {lines[i]}");
        }
        return lines;
    }

    public static void SimplifyLogs(List<string> logs)
    {
        using (StreamWriter sr = new StreamWriter(ProgramLogFilePath))
        {
            List<string> lines = new List<string>();
            foreach (string log in logs)
            {
                Console.WriteLine(log);
                string line = SimplifyLog(log);
                if (line != "")
                {
                    LogEvent e = new LogEvent(line);
                    Events.Add(e);
                    sr.WriteLine(line);
                }
            }
        }
    }

    public static string SimplifyLog(string log)
    {
        string Kill_pattern = @"'(?<victim>[^']+)' .+ in zone '(?<zone>[^']+)' killed by '(?<killer>[^']+)' .+ using '(?<weapon>[^']+)' .+ with damage type '(?<damageType>[^']+)'";
        
        Regex regex = new Regex(Kill_pattern);
        Match match = regex.Match(log);

        if (match.Success)
        {
            int maxlen = 10;
            string victim = match.Groups["victim"].Value;
            string zone = match.Groups["zone"].Value;
            string killer = match.Groups["killer"].Value;
            string weapon = match.Groups["weapon"].Value;
            string damageType = match.Groups["damageType"].Value;
                
            string simplify_victim = SimplifyName(victim);
            string simplify_killer = SimplifyName(killer);
            
            zone = ReduceName(zone, maxlen);
            weapon = ReduceName(weapon, maxlen);

            if (!simplify_killer.Contains("NPC") && simplify_victim!=simplify_killer)
            {
                Player? player = Players.FirstOrDefault(p => p.PlayerName == killer);
                if (player != null)
                {
                    player.AddKill(match.Groups["victim"].Value);
                }
                else
                {
                    player = new Player(match.Groups["killer"].Value);
                    player.AddKill(match.Groups["victim"].Value);
                    Players.Add(player);
                    Console.WriteLine($"New Player {match.Groups["killer"].Value}");
                }
            }

            return $"{simplify_victim} kill by {simplify_killer} with {weapon} ({damageType}). [{zone}]";
        }

        return "";
    }
    
    private static string SimplifyName(string name)
    {
        if (name.Contains("Human-Criminal")) return "Criminal NPC";
        if (name.Contains("Pilot")) return "Pilot NPC";
        if (name.Contains("Kopion")) return "Kopion";
        if (name.Contains("Consteted")) return "ContestedZone NPC";
        if (name.StartsWith("PU_")) return "NPC";
        return name = name.Length < 10 ? name : name.Substring(0, 10);; // Retourne le nom original si aucune règle de simplification ne s'applique
    }

    public static string ReduceName(string name, int len)
    {
        return name.Length < len ? name : name.Substring(0, len);
    }
}