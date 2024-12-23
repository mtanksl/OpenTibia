using System.Windows.Forms;

namespace mtanksl.OpenTibia.Host.GUI
{
    public partial class PromptForm : Form
    {
        public PromptForm(string text, string caption)
        {
            InitializeComponent();

            Text = text;

            labelCaption.Text = caption;
        }

        public string Message
        {
            get
            {
                return textBoxMessage.Text;
            }
        }

        private void buttonOk_Click(object sender, System.EventArgs e)
        {
            if ( !string.IsNullOrEmpty(Message) )
            {
                DialogResult = DialogResult.OK;
            }
            else
            {
                DialogResult = DialogResult.Cancel;
            }

            Close();
        }

        private void buttonCancel_Click(object sender, System.EventArgs e)
        {
            DialogResult = DialogResult.Cancel;

            Close();
        }
    }
}