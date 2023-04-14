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
            } );
        }

        public void EndWrite()
        {
            richTextBox.BeginInvoke( () =>
            {
                richTextBox.ScrollToCaret();
            } );
        }

        public void Line()
        {
            richTextBox.BeginInvoke( () =>
            {
                richTextBox.AppendText(Environment.NewLine);
            } );
        }
    }
}