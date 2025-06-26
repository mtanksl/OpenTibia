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

        private ILogger logger;

        public MainForm()
        {
            InitializeComponent();

#if DEBUG
            logger = new Logger(new RichTextboxLoggerProvider(richTextBox1), LogLevel.Debug);
#else
            logger = new Logger(new RichTextboxLoggerProvider(richTextBox1), LogLevel.Information);
#endif
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

                await Task.Run(() =>
                {
                    server = new Server();

                    server.Logger = logger;

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

                cleanToolStripMenuItem.Enabled = true;
            }
            catch (Exception ex)
            {
                server = null;

                logger.WriteLine(ex.ToString(), LogLevel.Error);
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

                await Task.Run(() =>
                {
                    server.ReloadPlugins();
                } );
            }
            catch (Exception ex)
            {
                logger.WriteLine(ex.ToString(), LogLevel.Error);
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

                await Task.Run(() =>
                {
                    server.KickAll();

                    server.Save();

                    server.Stop();

                    server.Dispose();

                    server = null;

                    server = new Server();

                    server.Logger = logger;

                    server.Start();
                } );

                maintenanceToolStripMenuItem.Checked = false;
            }
            catch (Exception ex)
            {
                server = null;

                logger.WriteLine(ex.ToString(), LogLevel.Error);
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

                await Task.Run(() =>
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

                cleanToolStripMenuItem.Enabled = false;
            }
            catch (Exception ex)
            {
                logger.WriteLine(ex.ToString(), LogLevel.Error);
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

            server.QueueForExecution(() =>
            {
                ShowWindowTextOutgoingPacket showTextOutgoingPacket = new ShowWindowTextOutgoingPacket(MessageMode.Warning, promptForm.Message);

                foreach (var observer in Context.Current.Server.GameObjects.GetPlayers())
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

                await Task.Run(() =>
                {
                    server.KickAll();
                } );
            }
            catch (Exception ex)
            {
                logger.WriteLine(ex.ToString(), LogLevel.Error);
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

                await Task.Run(() =>
                {
                    server.Save();
                } );
            }
            catch (Exception ex)
            {
                logger.WriteLine(ex.ToString(), LogLevel.Error);
            }
            finally
            {
                Enabled = true;
            }
        }

        private async void cleanToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                MessageBox.Show("Server is not running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            try
            {
                Enabled = false;

                await Task.Run(() =>
                {
                    server.Clean();
                } );
            }
            catch (Exception ex)
            {
                logger.WriteLine(ex.ToString(), LogLevel.Error);
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

                        await Task.Run(() =>
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

                        cleanToolStripMenuItem.Enabled = false;

                        ignoreCloseEvent = true;

                        Close();
                    }
                    catch (Exception ex)
                    {
                        logger.WriteLine(ex.ToString(), LogLevel.Error);
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
                statisticsForm = new StatisticsForm(() => server);

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
                onlinePlayersForm = new OnlinePlayersForm(() => server);

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

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger.WriteLine("MTOTS - An open Tibia server developed by mtanksl");
            logger.WriteLine("Copyright (C) 2024 mtanksl");
            logger.WriteLine();
            logger.WriteLine("This program is free software: you can redistribute it and/or modify");
            logger.WriteLine("it under the terms of the GNU General Public License as published by");
            logger.WriteLine("the Free Software Foundation, either version 3 of the License, or");
            logger.WriteLine("(at your option) any later version.");
            logger.WriteLine();
            logger.WriteLine("This program is distributed in the hope that it will be useful,");
            logger.WriteLine("but WITHOUT ANY WARRANTY; without even the implied warranty of");
            logger.WriteLine("MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the");
            logger.WriteLine("GNU General Public License for more details.");
            logger.WriteLine();
            logger.WriteLine("You should have received a copy of the GNU General Public License");
            logger.WriteLine("along with this program. If not, see <https://www.gnu.org/licenses/>.");
            logger.WriteLine();
        }

        private void supportUsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            logger.WriteLine("If you enjoy using open source projects and would like to support our work,");
            logger.WriteLine("consider making a donation! Your contributions help us maintain and improve");
            logger.WriteLine("the project. You can support us by sending directly to the following address:");
            logger.WriteLine("");
            logger.WriteLine("Bitcoin (BTC) Address: bc1qc2p79gtjhnpff78su86u8vkynukt8pmfnr43lf");
            logger.WriteLine("");
            logger.WriteLine("Monero (XMR) Address: 87KefRhqaf72bYBUF3EsUjY89iVRH72GsRsEYZmKou9ZPFhGaGzc1E4URbCV9oxtdTYNcGXkhi9XsRhd2ywtt1bq7PoBfd4");
            logger.WriteLine("");
            logger.WriteLine("Thank you for your support!");
            logger.WriteLine("Every contribution, no matter the size, makes a difference.");
            logger.WriteLine("");
        }
    }
}