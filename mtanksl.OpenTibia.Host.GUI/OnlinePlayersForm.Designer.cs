namespace mtanksl.OpenTibia.Host.GUI
{
    partial class OnlinePlayersForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            components = new System.ComponentModel.Container();
            dataGridViewPlayers = new System.Windows.Forms.DataGridView();
            Column1 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column2 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column3 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column4 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column7 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column5 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            Column6 = new System.Windows.Forms.DataGridViewTextBoxColumn();
            contextMenuStripNoRowSelected = new System.Windows.Forms.ContextMenuStrip(components);
            refreshToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            contextMenuStripRowSelected = new System.Windows.Forms.ContextMenuStrip(components);
            refreshToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            sendMessageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            kickPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            banPlayerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            statusStrip1 = new System.Windows.Forms.StatusStrip();
            toolStripStatusLabelPlayers = new System.Windows.Forms.ToolStripStatusLabel();
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayers).BeginInit();
            contextMenuStripNoRowSelected.SuspendLayout();
            contextMenuStripRowSelected.SuspendLayout();
            statusStrip1.SuspendLayout();
            SuspendLayout();
            // 
            // dataGridViewPlayers
            // 
            dataGridViewPlayers.AllowUserToAddRows = false;
            dataGridViewPlayers.AllowUserToDeleteRows = false;
            dataGridViewPlayers.AllowUserToResizeRows = false;
            dataGridViewPlayers.BackgroundColor = System.Drawing.SystemColors.Window;
            dataGridViewPlayers.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            dataGridViewPlayers.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            dataGridViewPlayers.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] { Column1, Column2, Column3, Column4, Column7, Column5, Column6 });
            dataGridViewPlayers.Dock = System.Windows.Forms.DockStyle.Fill;
            dataGridViewPlayers.Location = new System.Drawing.Point(0, 0);
            dataGridViewPlayers.MultiSelect = false;
            dataGridViewPlayers.Name = "dataGridViewPlayers";
            dataGridViewPlayers.ReadOnly = true;
            dataGridViewPlayers.RowHeadersVisible = false;
            dataGridViewPlayers.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            dataGridViewPlayers.Size = new System.Drawing.Size(334, 209);
            dataGridViewPlayers.TabIndex = 1;
            dataGridViewPlayers.MouseDown += dataGridViewPlayers_MouseDown;
            // 
            // Column1
            // 
            Column1.HeaderText = "Player";
            Column1.Name = "Column1";
            Column1.ReadOnly = true;
            Column1.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            Column1.Width = 150;
            // 
            // Column2
            // 
            Column2.HeaderText = "Level";
            Column2.Name = "Column2";
            Column2.ReadOnly = true;
            Column2.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column3
            // 
            Column3.HeaderText = "Vocation";
            Column3.Name = "Column3";
            Column3.ReadOnly = true;
            Column3.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column4
            // 
            Column4.HeaderText = "Rank";
            Column4.Name = "Column4";
            Column4.ReadOnly = true;
            Column4.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column7
            // 
            Column7.HeaderText = "Account Status";
            Column7.Name = "Column7";
            Column7.ReadOnly = true;
            Column7.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column5
            // 
            Column5.HeaderText = "IP Address";
            Column5.Name = "Column5";
            Column5.ReadOnly = true;
            Column5.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // Column6
            // 
            Column6.HeaderText = "Latency";
            Column6.Name = "Column6";
            Column6.ReadOnly = true;
            Column6.SortMode = System.Windows.Forms.DataGridViewColumnSortMode.NotSortable;
            // 
            // contextMenuStripNoRowSelected
            // 
            contextMenuStripNoRowSelected.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { refreshToolStripMenuItem });
            contextMenuStripNoRowSelected.Name = "contextMenuStrip1";
            contextMenuStripNoRowSelected.Size = new System.Drawing.Size(114, 26);
            // 
            // refreshToolStripMenuItem
            // 
            refreshToolStripMenuItem.Name = "refreshToolStripMenuItem";
            refreshToolStripMenuItem.Size = new System.Drawing.Size(113, 22);
            refreshToolStripMenuItem.Text = "Refresh";
            refreshToolStripMenuItem.Click += refreshToolStripMenuItem_Click;
            // 
            // contextMenuStripRowSelected
            // 
            contextMenuStripRowSelected.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { refreshToolStripMenuItem1, toolStripSeparator1, sendMessageToolStripMenuItem, kickPlayerToolStripMenuItem, banPlayerToolStripMenuItem });
            contextMenuStripRowSelected.Name = "contextMenuStrip1";
            contextMenuStripRowSelected.Size = new System.Drawing.Size(181, 120);
            // 
            // refreshToolStripMenuItem1
            // 
            refreshToolStripMenuItem1.Name = "refreshToolStripMenuItem1";
            refreshToolStripMenuItem1.Size = new System.Drawing.Size(180, 22);
            refreshToolStripMenuItem1.Text = "Refresh";
            refreshToolStripMenuItem1.Click += refreshToolStripMenuItem1_Click;
            // 
            // toolStripSeparator1
            // 
            toolStripSeparator1.Name = "toolStripSeparator1";
            toolStripSeparator1.Size = new System.Drawing.Size(177, 6);
            // 
            // sendMessageToolStripMenuItem
            // 
            sendMessageToolStripMenuItem.Name = "sendMessageToolStripMenuItem";
            sendMessageToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            sendMessageToolStripMenuItem.Text = "Send Message";
            sendMessageToolStripMenuItem.Click += sendMessageToolStripMenuItem_Click;
            // 
            // kickPlayerToolStripMenuItem
            // 
            kickPlayerToolStripMenuItem.Name = "kickPlayerToolStripMenuItem";
            kickPlayerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            kickPlayerToolStripMenuItem.Text = "Kick Player";
            kickPlayerToolStripMenuItem.Click += kickPlayerToolStripMenuItem_Click;
            // 
            // banPlayerToolStripMenuItem
            // 
            banPlayerToolStripMenuItem.Name = "banPlayerToolStripMenuItem";
            banPlayerToolStripMenuItem.Size = new System.Drawing.Size(180, 22);
            banPlayerToolStripMenuItem.Text = "Ban Player";
            banPlayerToolStripMenuItem.Click += banPlayerToolStripMenuItem_Click;
            // 
            // statusStrip1
            // 
            statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] { toolStripStatusLabelPlayers });
            statusStrip1.Location = new System.Drawing.Point(0, 209);
            statusStrip1.Name = "statusStrip1";
            statusStrip1.Size = new System.Drawing.Size(334, 22);
            statusStrip1.TabIndex = 0;
            statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabelPlayers
            // 
            toolStripStatusLabelPlayers.Name = "toolStripStatusLabelPlayers";
            toolStripStatusLabelPlayers.Size = new System.Drawing.Size(94, 17);
            toolStripStatusLabelPlayers.Text = "Online Players: 0";
            // 
            // OnlinePlayersForm
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(334, 231);
            Controls.Add(dataGridViewPlayers);
            Controls.Add(statusStrip1);
            MinimumSize = new System.Drawing.Size(350, 270);
            Name = "OnlinePlayersForm";
            ShowIcon = false;
            Text = "Online Players";
            ((System.ComponentModel.ISupportInitialize)dataGridViewPlayers).EndInit();
            contextMenuStripNoRowSelected.ResumeLayout(false);
            contextMenuStripRowSelected.ResumeLayout(false);
            statusStrip1.ResumeLayout(false);
            statusStrip1.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridViewPlayers;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripNoRowSelected;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem;
        private System.Windows.Forms.ContextMenuStrip contextMenuStripRowSelected;
        private System.Windows.Forms.ToolStripMenuItem refreshToolStripMenuItem1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem kickPlayerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem banPlayerToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabelPlayers;
        private System.Windows.Forms.ToolStripMenuItem sendMessageToolStripMenuItem;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column1;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column2;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column3;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column4;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column7;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column5;
        private System.Windows.Forms.DataGridViewTextBoxColumn Column6;
    }
}