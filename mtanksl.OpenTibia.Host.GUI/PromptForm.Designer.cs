namespace mtanksl.OpenTibia.Host.GUI
{
    partial class PromptForm
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
            labelCaption = new System.Windows.Forms.Label();
            buttonOk = new System.Windows.Forms.Button();
            buttonCancel = new System.Windows.Forms.Button();
            textBoxMessage = new System.Windows.Forms.TextBox();
            SuspendLayout();
            // 
            // labelCaption
            // 
            labelCaption.AutoSize = true;
            labelCaption.Location = new System.Drawing.Point(12, 9);
            labelCaption.Name = "labelCaption";
            labelCaption.Size = new System.Drawing.Size(49, 15);
            labelCaption.TabIndex = 0;
            labelCaption.Text = "Caption";
            // 
            // buttonOk
            // 
            buttonOk.Location = new System.Drawing.Point(166, 56);
            buttonOk.Name = "buttonOk";
            buttonOk.Size = new System.Drawing.Size(75, 23);
            buttonOk.TabIndex = 2;
            buttonOk.Text = "OK";
            buttonOk.UseVisualStyleBackColor = true;
            buttonOk.Click += buttonOk_Click;
            // 
            // buttonCancel
            // 
            buttonCancel.Location = new System.Drawing.Point(247, 56);
            buttonCancel.Name = "buttonCancel";
            buttonCancel.Size = new System.Drawing.Size(75, 23);
            buttonCancel.TabIndex = 3;
            buttonCancel.Text = "Cancel";
            buttonCancel.UseVisualStyleBackColor = true;
            buttonCancel.Click += buttonCancel_Click;
            // 
            // textBoxMessage
            // 
            textBoxMessage.Location = new System.Drawing.Point(12, 27);
            textBoxMessage.Name = "textBoxMessage";
            textBoxMessage.Size = new System.Drawing.Size(310, 23);
            textBoxMessage.TabIndex = 1;
            // 
            // PromptForm
            // 
            AcceptButton = buttonOk;
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            CancelButton = buttonCancel;
            ClientSize = new System.Drawing.Size(334, 91);
            Controls.Add(textBoxMessage);
            Controls.Add(buttonCancel);
            Controls.Add(buttonOk);
            Controls.Add(labelCaption);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "PromptForm";
            ShowIcon = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            Text = "Text";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private System.Windows.Forms.Label labelCaption;
        private System.Windows.Forms.Button buttonOk;
        private System.Windows.Forms.Button buttonCancel;
        private System.Windows.Forms.TextBox textBoxMessage;
    }
}