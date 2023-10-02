using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;
using System.IO;

class Program
{
    private static Form galvenaisLogs;
    private static Panel panele;
    private static Dictionary<Color, string> krāsuNosaukumi;
    private static FlowLayoutPanel krāsuPanelis;
    private static System.Windows.Forms.Timer colorTransitionTimer;
    private static Color targetColor;
    private static Color currentColor;

    static void Main()
    {
        Application.EnableVisualStyles();
        Application.SetCompatibleTextRenderingDefault(false);

        string failaNosaukums = "programmas_dati.txt";

        DateTime pašreizējaisLaiks = DateTime.Now;

        string dati = "Programma iedarbināta: " + pašreizējaisLaiks.ToString() + Environment.NewLine;

        File.WriteAllText(failaNosaukums, dati);

        galvenaisLogs = new Form
        {
            Text = "Krāsu Mainītājs",
            FormBorderStyle = FormBorderStyle.FixedDialog,
            WindowState = FormWindowState.Maximized
        };

        panele = new Panel
        {
            Dock = DockStyle.Fill,
            BackColor = Color.White
        };

        krāsuNosaukumi = new Dictionary<Color, string>
        {
            { Color.Red, "Sarkana" },
            { Color.Blue, "Zila" },
            { Color.Yellow, "Dzeltena" },
            { Color.Green, "Zaļa" },
            { Color.Orange, "Oranža" }
        };

        krāsuPanelis = new FlowLayoutPanel
        {
            Dock = DockStyle.Top,
            FlowDirection = FlowDirection.LeftToRight,
            Padding = new Padding(10),
            Height = 50,
            AutoSize = true
        };

        foreach (var krāsaNosaukums in krāsuNosaukumi)
        {
            Button krāsuPoga = new Button
            {
                Text = "Mainīt Krāsu uz " + krāsaNosaukums.Value,
                BackColor = krāsaNosaukums.Key,
                ForeColor = IegūtKontrastējošuKrāsu(krāsaNosaukums.Key),
                Margin = new Padding(5),
                AutoSize = false,
                Size = new Size(100, 40)
            };

            krāsuPoga.Click += (sūtītājs, notikums) =>
            {
                targetColor = krāsaNosaukums.Key;
                colorTransitionTimer.Start();
            };

            krāsuPanelis.Controls.Add(krāsuPoga);
        }

        galvenaisLogs.Controls.Add(panele);
        galvenaisLogs.Controls.Add(krāsuPanelis);

        colorTransitionTimer = new System.Windows.Forms.Timer();
        colorTransitionTimer.Interval = 10;
        colorTransitionTimer.Enabled = false;
        colorTransitionTimer.Tick += ColorTransitionTimer_Tick;

        Application.Run(galvenaisLogs);
    }

    private static void ColorTransitionTimer_Tick(object sender, EventArgs e)
    {
        int stepSize = 2;
        int rDiff = targetColor.R - currentColor.R;
        int gDiff = targetColor.G - currentColor.G;
        int bDiff = targetColor.B - currentColor.B;

        if (Math.Abs(rDiff) <= stepSize && Math.Abs(gDiff) <= stepSize && Math.Abs(bDiff) <= stepSize)
        {
            currentColor = targetColor;
            colorTransitionTimer.Stop();
        }
        else
        {
            int newR = Math.Max(0, Math.Min(255, currentColor.R + (int)(rDiff * 0.1)));
            int newG = Math.Max(0, Math.Min(255, currentColor.G + (int)(gDiff * 0.1)));
            int newB = Math.Max(0, Math.Min(255, currentColor.B + (int)(bDiff * 0.1)));

            currentColor = Color.FromArgb(newR, newG, newB);
        }

        panele.BackColor = currentColor;
        galvenaisLogs.ForeColor = IegūtKontrastējošuKrāsu(currentColor);
    }


    static Color IegūtKontrastējošuKrāsu(Color krāsa)
    {
        double gaišums = (0.299 * krāsa.R + 0.587 * krāsa.G + 0.114 * krāsa.B) / 255;
        return gaišums > 0.5 ? Color.Black : Color.White;
    }
}
