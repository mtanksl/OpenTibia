﻿using OpenTibia.Game;
using System;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;

namespace mtanksl.OpenTibia.Host.GUI
{
    public partial class MainForm : Form
    {
        private Server server;

        public MainForm()
        {
            InitializeComponent();
        }

        private void MainForm_Load(object sender, EventArgs e)
        {
            if ( !File.Exists("data\\database.db") )
            {
                File.Copy("data\\template.db", "data\\database.db");
            }
        }

        private void richTextBox1_LinkClicked(object sender, LinkClickedEventArgs e)
        {
            Process.Start("explorer.exe", e.LinkText);
        }

        private void startToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server != null)
            {
                MessageBox.Show("Server is already running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            server = new Server(7171, 7172);
#if DEBUG
            server.Logger = new Logger(new RichTextboxLoggerProvider(richTextBox1), LogLevel.Debug);
#else
            server.Logger = new Logger(new RichTextboxLoggerProvider(richTextBox1), LogLevel.Information);
#endif
            server.Start();
        }

        private void shutdownToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                MessageBox.Show("Server is not running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            server.KickAll();

            server.Stop();

            server.Dispose();

            server = null;
        }

        private void kickAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (server == null)
            {
                MessageBox.Show("Server is not running.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);

                return;
            }

            server.KickAll();
        }

        private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (server != null)
            {
                if (MessageBox.Show("Server is running, do you really want to shutdown?", "Warning", MessageBoxButtons.YesNoCancel, MessageBoxIcon.Question) == DialogResult.Yes)
                {
                    server.KickAll();

                    server.Stop();

                    server.Dispose();

                    server = null;
                }
                else
                {
                    e.Cancel = true;
                }
            }
        }        
    }
}