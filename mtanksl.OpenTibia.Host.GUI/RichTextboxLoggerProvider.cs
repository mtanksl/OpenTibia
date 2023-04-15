using OpenTibia.Game;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace mtanksl.OpenTibia.Host.GUI
{
    public class RichTextboxLoggerProvider : ILoggerProvider
    {
        private RichTextBox richTextBox;

        public RichTextboxLoggerProvider(RichTextBox richTextBox)
        {
            this.richTextBox = richTextBox;
        }

        public void BeginWrite(LogLevel level)
        {
            richTextBox.BeginInvoke( () =>
            {
                Color color;

                switch (level)
                {
                    case LogLevel.Debug:

                        color = Color.Green;

                        break;

                    case LogLevel.Information:

                        color = Color.Blue;

                        break;

                    case LogLevel.Warning:

                        color = Color.Orange;

                        break;

                    case LogLevel.Error:

                        color = Color.Red;

                        break;

                    default:

                        color = richTextBox.ForeColor;

                        break;
                }

                richTextBox.SuspendLayout();

                richTextBox.SelectionStart = richTextBox.TextLength;

                richTextBox.SelectionLength = 0;

                richTextBox.SelectionColor = color;

                richTextBox.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - ");
            } );
        }

        public void Write(string message)
        {
            richTextBox.BeginInvoke( () =>
            {
                richTextBox.AppendText(message);

                if (richTextBox.Lines.Length >= 100)
                {
                    richTextBox.SelectionStart = 0;

                    richTextBox.SelectionLength = richTextBox.GetFirstCharIndexFromLine(richTextBox.Lines.Length - 100);

                    richTextBox.ReadOnly = false;

                    richTextBox.SelectedText = "";

                    richTextBox.ReadOnly = true;
                }
            } );
        }

        public void EndWrite()
        {
            richTextBox.BeginInvoke( () =>
            {
                richTextBox.SelectionStart = richTextBox.Text.Length;

                richTextBox.ScrollToCaret();

                richTextBox.ResumeLayout();
            } );
        }
    }
}