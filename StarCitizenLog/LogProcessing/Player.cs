namespace StarCitizenLog.LogProcessing;

public class Player
{
    public string PlayerName;
    
    public int nbKills;
    public int nbNPCkills;
    public int nbPlayerkills;
    
    public List<string> killnames;
    
    public DateTime LastKill;

    public Player(string playerName)
    {
        this.PlayerName = playerName;
        this.killnames = new List<string>();
        this.LastKill = DateTime.Now;
    }

    public void AddKill(string kill)
    {
        this.killnames.Add(kill);
        this.nbKills++;
        this.LastKill = DateTime.Now;

        if (kill.Contains("NPC"))
        {
            this.nbNPCkills++;
        }
        else
        {
            this.nbPlayerkills++;
        }
    }
}