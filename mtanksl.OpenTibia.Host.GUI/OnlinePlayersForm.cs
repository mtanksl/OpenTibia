using OpenTibia.Common.Objects;
using OpenTibia.Common.Structures;
using OpenTibia.Data.Models;
using OpenTibia.Game.Commands;
using OpenTibia.Game.Common;
using OpenTibia.Network.Packets.Outgoing;
using System;
using System.Linq;
using System.Windows.Forms;

namespace mtanksl.OpenTibia.Host.GUI
{
    public partial class OnlinePlayersForm : Form
    {
        private Func<IServer> getServer;

        public OnlinePlayersForm(Func<IServer> getServer)
        {
            InitializeComponent();

            this.getServer = getServer;

            RefreshList();
        }

        private void RefreshList()
        {
            var server = getServer();

            dataGridViewPlayers.Rows.Clear();

            if (server != null)
            {
                foreach (var player in server.GameObjects.GetPlayers().OrderBy(p => p.Name))
                {
                    var rowIndex = dataGridViewPlayers.Rows.Add(player.Name, player.Level.ToString(), Enum.GetName(player.Vocation), Enum.GetName(player.Rank), player.Client.Connection.IpAddress);

                    var row = dataGridViewPlayers.Rows[rowIndex];

                    row.Tag = player;
                }
            }

            toolStripStatusLabelPlayers.Text = "Online Players: " + dataGridViewPlayers.Rows.Count;
        }

        private int rowIndex;

        private void dataGridViewPlayers_MouseDown(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Right)
            {
                var hitTestInfo = dataGridViewPlayers.HitTest(e.X, e.Y);

                if (hitTestInfo.RowIndex >= 0)
                {
                    rowIndex = hitTestInfo.RowIndex;

                    var row = dataGridViewPlayers.Rows[rowIndex];

                    row.Selected = true;

                    contextMenuStripRowSelected.Show(dataGridViewPlayers, e.Location);
                }
                else
                {
                    rowIndex = 0;

                    dataGridViewPlayers.ClearSelection();

                    contextMenuStripNoRowSelected.Show(dataGridViewPlayers, e.Location);
                }
            }
        }

        private void refreshToolStripMenuItem_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void refreshToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            RefreshList();
        }

        private void sendMessageToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var row = dataGridViewPlayers.Rows[rowIndex];

            var player = (Player)row.Tag;

            var promptForm = new PromptForm("Send Message", "Message to " + player.Name + ":");

            if (promptForm.ShowDialog() != DialogResult.OK)
            {
                return;
            }
                 
            var server = getServer();

            if (server != null)
            {
                server.QueueForExecution( () =>
                {
                    if (player.Tile != null && !player.IsDestroyed)
                    {
                        Context.Current.AddPacket(player, new ShowWindowTextOutgoingPacket(TextColor.RedCenterGameWindowAndServerLog, promptForm.Message) );
                    }

                    return Promise.Completed;
                } );
            }
        }

        private void kickPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var row = dataGridViewPlayers.Rows[rowIndex];

            var player = (Player)row.Tag;

            if (MessageBox.Show("Do you want to kick the player " + player.Name + "?", "Confimation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            var server = getServer();

            if (server != null)
            {
                server.QueueForExecution(async () =>
                {
                    if (player.Tile != null && !player.IsDestroyed)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.Puff));

                        await Context.Current.AddCommand(new CreatureDestroyCommand(player));
                    }
                } );
            }
        }

        private void banPlayerToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var row = dataGridViewPlayers.Rows[rowIndex];

            var player = (Player)row.Tag;

            if (MessageBox.Show("Do you want to ban the player " + player.Name + "?", "Confimation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            var server = getServer();

            if (server != null)
            {
                server.QueueForExecution(async () =>
                {
                    using (var database = Context.Current.Server.DatabaseFactory.Create())
                    {
                        DbBan dbBan = await database.BanRepository.GetBanByPlayerId(player.DatabasePlayerId);

                        if (dbBan == null)
                        {
                            dbBan = new DbBan()
                            {
                                Type = BanType.Player,

                                PlayerId = player.DatabasePlayerId,

                                Message = "This player has been banned.",

                                CreationDate = DateTime.UtcNow
                            };

                            database.BanRepository.AddBan(dbBan);

                            await database.Commit();
                        }
                    }

                    if (player.Tile != null && !player.IsDestroyed)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.Puff));

                        await Context.Current.AddCommand(new CreatureDestroyCommand(player));
                    }
                } );
            }
        }

        private void banIPAddressToolStripMenuItem_Click(object sender, EventArgs e)
        {
            var row = dataGridViewPlayers.Rows[rowIndex];

            var player = (Player)row.Tag;

            if (MessageBox.Show("Do you want to ban the IP Address " + player.Client.Connection.IpAddress + "?", "Confimation", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Warning) != DialogResult.Yes)
            {
                return;
            }

            var server = getServer();

            if (server != null)
            {
                server.QueueForExecution(async () =>
                {
                    using (var database = Context.Current.Server.DatabaseFactory.Create())
                    {
                        DbBan dbBan = await database.BanRepository.GetBanByIpAddress(player.Client.Connection.IpAddress);

                        if (dbBan == null)
                        {
                            dbBan = new DbBan()
                            {
                                Type = BanType.IpAddress,

                                IpAddress = player.Client.Connection.IpAddress,

                                Message = "This IP Address has been banned.",

                                CreationDate = DateTime.UtcNow
                            };

                            database.BanRepository.AddBan(dbBan);

                            await database.Commit();
                        }
                    }

                    if (player.Tile != null && !player.IsDestroyed)
                    {
                        await Context.Current.AddCommand(new ShowMagicEffectCommand(player, MagicEffectType.Puff));

                        await Context.Current.AddCommand(new CreatureDestroyCommand(player));
                    }
                } );
            }
        }        
    }
}