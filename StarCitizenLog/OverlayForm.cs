using System;
using System.Linq;
using System.Windows.Forms;
using System.Drawing;
using StarCitizenLog.LogProcessing;
using Timer = System.Windows.Forms.Timer;

public class OverlayForm : Form
{
    private Timer updateTimer; // Timer pour actualiser l'overlay
    private Label label;
    private Label label2;
    private bool isVisible = true;

    public OverlayForm()
    {
        
        InitializeOverlay();

        // Timer pour actualiser le texte
        updateTimer = new Timer();
        updateTimer.Interval = 500; // 0.5 seconde
        updateTimer.Tick += (s, e) => UpdateOverlay();
        updateTimer.Start();
    }

    private void InitializeOverlay()
    {
        // Configuration de la fenêtre
        this.FormBorderStyle = FormBorderStyle.None;
        this.StartPosition = FormStartPosition.Manual;
        this.TopMost = true;
        this.BackColor = Color.Black;
        this.TransparencyKey = Color.Black; // Fenêtre transparente
        this.Opacity = 1.0;

        // Taille et position de la fenêtre
        this.Width = 1900;
        this.Height = 1000;
        this.Location = new Point(10, 500);

        // Label pour afficher le texte
        label = new Label()
        {
            AutoSize = true,
            ForeColor = Color.White,
            Location = new Point(10, 10),
            Font = new Font("Arial", 12),
            Text = "Overlay Not Started"
        };
        
        label2 = new Label()
        {
            AutoSize = true,
            ForeColor = Color.White,
            Location = new Point(10, 115),
            Font = new Font("Arial", 12),
            Text = "Overlay Not Started"
        };

        this.Controls.Add(label);
        this.Controls.Add(label2);

        // Gestionnaire d'événements pour les touches
        this.KeyDown += OnKeyDown;
        this.KeyPreview = true; // Permet de capturer les touches
    }

    private void UpdateOverlay()
    {

        LogParser.Events.RemoveAll(e => DateTime.Now - e.EventDate > TimeSpan.FromSeconds(60));
        
        if (LogParser.Events.Count == 0)
        {
            label.Text = "[     No Events     ]\n";
        }
        else
        {
            label.Text = string.Join("\n", LogParser.Events.TakeLast(5).Select(e => e.Message));
        }

        
        
        string l2 = "";
        foreach (var player in LogParser.Players)
        {
            //Console.WriteLine($"{player.PlayerName} KillCount: {player.nbKills}");
            if (DateTime.Now - player.LastKill > TimeSpan.FromSeconds(180))
            {
                l2 += $"{player.PlayerName,-10} Kill: {player.nbKills}\n";
            }
        }

        if (l2.Length == 0)
        {
            l2 = "[    No KillCount   ]";
        }

        label2.Text = l2;
    }


    private void OnKeyDown(object sender, KeyEventArgs e)
    {
        if (e.KeyCode == Keys.NumPad5)
        {
            isVisible = !isVisible;
            this.Opacity = isVisible ? 1.0 : 0.0; // Afficher ou cacher
        }
    }
}
