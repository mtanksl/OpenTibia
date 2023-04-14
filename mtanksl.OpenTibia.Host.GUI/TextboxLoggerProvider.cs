using OpenTibia.Game;
using System;
using System.Windows.Forms;

namespace mtanksl.OpenTibia.Host.GUI
{
    public class TextboxLoggerProvider : ILoggerProvider
    {
        private TextBox textbox;

        public TextboxLoggerProvider(TextBox textbox)
        {
            this.textbox = textbox;
        }

        public void BeginWrite(LogLevel level)
        {
            if (textbox.InvokeRequired)
            {
                textbox.Invoke( () =>
                {
                    textbox.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - ");
                } );
            }
            else
            {
                textbox.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - ");
            }
        }

        public void Write(string message)
        {
            if (textbox.InvokeRequired)
            {
                textbox.Invoke( () =>
                {
                    textbox.AppendText(message);
                } );
            }
            else
            {
                textbox.AppendText(message);
            }
        }

        public void EndWrite()
        {
            
        }

        public void Line()
        {
            if (textbox.InvokeRequired)
            {
                textbox.Invoke( () =>
                {
                    textbox.AppendText(Environment.NewLine);
                } );
            }
            else
            {
                textbox.AppendText(Environment.NewLine);
            }
        }
    }
}