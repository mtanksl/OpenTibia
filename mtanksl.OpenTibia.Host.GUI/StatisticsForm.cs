using OpenTibia.Game.Common;
using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace mtanksl.OpenTibia.Host.GUI
{
    public partial class StatisticsForm : Form
    {
        private static readonly string[] Sizes = { "B", "KB", "MB", "GB", "TB", "PB", "EB", "ZB", "YB" };

        private static string ConvertBytesToHumanReadable(ulong bytes)
        {
            double size = bytes;

            int magnitude = 0;

            while (size > 1024)
            {
                magnitude++;

                size /= 1024;
            }

            return size.ToString("0.00") + " " + Sizes[magnitude];
        }

        private Func<IServer> getServer;

        private List< (string Key, Func<IServer, string> GetValue) > rows = new List<(string Key, Func<IServer, string> GetValue)>()
        {
            ("Uptime", server => server.Statistics.Uptime.Days + " days " + server.Statistics.Uptime.Hours + " hours " + server.Statistics.Uptime.Minutes + " minutes"),

            ("Active connections", server => server.Statistics.ActiveConnections.ToString() ),

            ("Total messages sent", server => server.Statistics.TotalMessagesSent.ToString() ),

            ("Total bytes sent", server => server.Statistics.TotalBytesSent + " bytes (" + ConvertBytesToHumanReadable(server.Statistics.TotalBytesSent) + ")"),

            ("Total messages received", server => server.Statistics.TotalMessagesReceived.ToString() ),

            ("Total bytes received", server => server.Statistics.TotalBytesReceived + " bytes (" + ConvertBytesToHumanReadable(server.Statistics.TotalBytesReceived) + ")"),

            ("Average processing time", server =>server.Statistics.AverageProcessingTime.ToString("N3") + " milliseconds (" + (1000 / server.Statistics.AverageProcessingTime).ToString("N0") + " FPS)")
        };

        public StatisticsForm(Func<IServer> getServer)
        {
            InitializeComponent();

            this.getServer = getServer;

            for (int i = 0; i < rows.Count; i++)
            {
                dataGridView1.Rows.Add(new object[] { rows[i].Key, null } );
            }
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            IServer server = getServer();

            if (server == null)
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[1].Value = null;
                }
            }
            else
            {
                for (int i = 0; i < rows.Count; i++)
                {
                    dataGridView1.Rows[i].Cells[1].Value = rows[i].GetValue(server);
                }
            }
        }
    }
}