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
            textbox.BeginInvoke( () =>
            {
                textbox.AppendText(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + " - ");
            } );
        }

        public void Write(string message)
        {
            textbox.BeginInvoke( () =>
            {
                textbox.AppendText(message);
            } );
        }

        public void EndWrite()
        {
            
        }

        public void Line()
        {
            textbox.BeginInvoke( () =>
            {
                textbox.AppendText(Environment.NewLine);
            } );
        }
    }
}