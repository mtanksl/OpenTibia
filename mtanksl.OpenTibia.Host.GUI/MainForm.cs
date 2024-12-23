using OpenTibia.Common.Structures;
using OpenTibia.Game.Common;
using OpenTibia.Game.Common.ServerObjects;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace mtanksl.OpenTibia.Host.GUI
{
    public partial class MainForm : Form
    {
        private IServer server;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {

        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", e.LinkText);
        }

        private async void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server != null)
            {
                MessageBox.Show("Server is already running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            try
            {
                Enabled = false;

                await Task.Run( () =>
                {
                    server = new Server();
#if DEBUG
                    server.Logger = new Logger(new RichTextboxLoggerProvider(richTextBox1), LogLevel.Debug);
#else
                    server.Logger = new Logger(new RichTextboxLoggerProvider(richTextBox1), LogLevel.Information);
#endif
                    server.Start();
                } );

                startToolStripMenuItem.Enabled = false;

                reloadToolStripMenuItem.Enabled = true;

                restartToolStripMenuItem.Enabled = true;

                stopToolStripMenuItem.Enabled = true;

                broadcastMessageToolStripMenuItem.Enabled = true;

                maintenanceToolStripMenuItem.Enabled = true;

                maintenanceToolStripMenuItem.Checked = false;

                kickAllToolStripMenuItem.Enabled = true;

                saveToolStripMenuItem.Enabled = true;
            }
            catch
            {
                server = null;
            }
            finally
            {
                Enabled = true;
            }
        }

        private async void pluginsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                MessageBox.Show("Server is not running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            try
            {
                Enabled = false;

                await Task.Run( () =>
                {
                    server.ReloadPlugins();
                } );
            }
            catch
            {

            }
            finally
            {
                Enabled = true;
            }
        }

        private async void restartToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                MessageBox.Show("Server is not running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            try
            {
                Enabled = false;

                await Task.Run( () =>
                {
                    server.KickAll();

                    server.Save();

                    server.Stop();

                    server.Dispose();

                    server = null;

                    server = new Server();
#if DEBUG
                    server.Logger = new Logger(new RichTextboxLoggerProvider(richTextBox1), LogLevel.Debug);
#else
                    server.Logger = new Logger(new RichTextboxLoggerProvider(richTextBox1), LogLevel.Information);
#endif
                    server.Start();
                } );

                maintenanceToolStripMenuItem.Checked = false;
            }
            catch
            {
                server = null;
            }
            finally
            {
                Enabled = true;
            }
        }

        private async void stopToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                MessageBox.Show("Server is not running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            try
            {
                Enabled = false;

                await Task.Run( () =>
                {
                    server.KickAll();

                    server.Save();

                    server.Stop();

                    server.Dispose();

                    server = null;
                } );

                startToolStripMenuItem.Enabled = true;

                reloadToolStripMenuItem.Enabled = false;

                restartToolStripMenuItem.Enabled = false;

                stopToolStripMenuItem.Enabled = false;

                broadcastMessageToolStripMenuItem.Enabled = false;               

                maintenanceToolStripMenuItem.Enabled = false;

                maintenanceToolStripMenuItem.Checked = false;

                kickAllToolStripMenuItem.Enabled = false;

                saveToolStripMenuItem.Enabled = false;
            }
            catch
            {

            }
            finally
            {
                Enabled = true;
            }
        }

        private void broadcastMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                MessageBox.Show("Server is not running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            var promptForm = new PromptForm("Broadcast Message", "Message:");

            if (promptForm.ShowDialog() != DialogResult.OK)
            {
                return;
            }

            server.QueueForExecution( () =>
            {
                ShowWindowTextOutgoingPacket showTextOutgoingPacket = new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, promptForm.Message);

                foreach (var observer in Context.Current.Server.GameObjects.GetPlayers() )
                {
                    Context.Current.AddPacket(observer, showTextOutgoingPacket);
                }

                return Promise.Completed;
            } );
        }

        private void maintenanceToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                MessageBox.Show("Server is not running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            if (server.Status == ServerStatus.Running)
            {
                server.Pause();

                maintenanceToolStripMenuItem.Checked = true;
            }
            else if (server.Status == ServerStatus.Paused)
            {
                server.Continue();

                maintenanceToolStripMenuItem.Checked = false;
            }
        }

        private async void kickAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                MessageBox.Show("Server is not running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            try
            {
                Enabled = false;

                await Task.Run( () =>
                {
                    server.KickAll();
                } );
            }
            catch
            {

            }
            finally
            {
                Enabled = true;
            }
        }

        private async void saveToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                MessageBox.Show("Server is not running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            try
            {
                Enabled = false;

                await Task.Run( () =>
                {
                    server.Save();
                } );
            }
            catch
            {

            }
            finally
            {
                Enabled = true;
            }
        }

        private void clearToolStripMenuItem_Click(object sender, EventArgs e)
        {
            richTextBox1.Clear();
        }

        private bool ignoreCloseEvent = false;

        private async void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (ignoreCloseEvent)
            {
                return;
            }

            if (server != null)
            {
                e.Cancel = true;

                if (MessageBox.Show("Server is running, do you really want to shutdown?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    try
                    {
                        Enabled = false;

                        await Task.Run( () =>
                        {
                            server.KickAll();

                            server.Save();

                            server.Stop();

                            server.Dispose();

                            server = null;
                        } );

                        startToolStripMenuItem.Enabled = true;

                        reloadToolStripMenuItem.Enabled = false;

                        restartToolStripMenuItem.Enabled = false;

                        stopToolStripMenuItem.Enabled = false;

                        broadcastMessageToolStripMenuItem.Enabled = false;

                        maintenanceToolStripMenuItem.Enabled = false;

                        maintenanceToolStripMenuItem.Checked = false;

                        kickAllToolStripMenuItem.Enabled = false;

                        saveToolStripMenuItem.Enabled = false;

                        ignoreCloseEvent = true;

                        Close();
                    }
                    catch
                    {

                    }
                    finally
                    {
                        Enabled = true;
                    }
                }
            }
        }

        private StatisticsForm statisticsForm;

        private void statisticsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (statisticsForm == null)
            {
                statisticsForm = new StatisticsForm( () => server);

                statisticsForm.FormClosed += (s, e) =>
                {
                    statisticsForm.Dispose();

                    statisticsForm = null;

                    statisticsToolStripMenuItem.Checked = false;
                };

                statisticsForm.Show();

                statisticsToolStripMenuItem.Checked = true;
            }
            else
            {
                statisticsForm.Close();
            }
        }

        private OnlinePlayersForm onlinePlayersForm;

        private void onlinePlayersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (onlinePlayersForm == null)
            {
                onlinePlayersForm = new OnlinePlayersForm( () => server);

                onlinePlayersForm.FormClosed += (s, e) =>
                {
                    onlinePlayersForm.Dispose();

                    onlinePlayersForm = null;

                    onlinePlayersToolStripMenuItem.Checked = false;
                };

                onlinePlayersForm.Show();

                onlinePlayersToolStripMenuItem.Checked = true;
            }
            else
            {
                onlinePlayersForm.Close();
            }
        }
    }
}