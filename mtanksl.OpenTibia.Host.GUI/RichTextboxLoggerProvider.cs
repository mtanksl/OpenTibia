using OpenTibia.Game;
using System;
using System.Drawing;
using System.Windows.Forms;

namespace mtanksl.OpenTibia.Host.GUI
{
    public class RichTextboxLoggerProvider : ILoggerProvider
    {
        private RichTextBox richTextBox;

        public RichTextboxLoggerProvider(RichTextBox textbox)
        {
            this.richTextBox = textbox;
        }

        public void BeginWrite(LogLevel level)
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
        
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke( () =>
                {
                    richTextBox.SelectionStart = richTextBox.TextLength;

                    richTextBox.SelectionLength = 0;

                    richTextBox.SelectionColor = color;

                    richTextBox.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - ");
                } );
            }
            else
            {
                richTextBox.SelectionStart = richTextBox.TextLength;

                richTextBox.SelectionLength = 0;

                richTextBox.SelectionColor = color;

                richTextBox.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - ");
            }
        }

        public void Write(string message)
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke( () =>
                {
                    richTextBox.AppendText(message);
                } );
            }
            else
            {
                richTextBox.AppendText(message);
            }
        }

        public void EndWrite()
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke( () =>
                {
                    richTextBox.ScrollToCaret();
                } );
            }
            else
            {
                richTextBox.ScrollToCaret();
            }
        }

        public void Line()
        {
            if (richTextBox.InvokeRequired)
            {
                richTextBox.Invoke( () =>
                {
                    richTextBox.AppendText(Environment.NewLine);
                } );
            }
            else
            {
                richTextBox.AppendText(Environment.NewLine);
            }
        }
    }
}