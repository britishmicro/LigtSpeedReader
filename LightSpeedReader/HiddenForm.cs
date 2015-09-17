using System;
using System.ComponentModel;
using System.Windows.Forms;

namespace LigtSpeedReaderLite
{
    public partial class HiddenForm : Form
    {

        VisualReader visualReader = new VisualReader();
        private OSHotkey _osHotKey;

        public HiddenForm()
        {
            InitializeComponent();
            this._osHotKey = new OSHotkey();
            this._osHotKey.HotkeyPress += this.osHotKey_HotKeyPress;
            this._osHotKey.RegisterHotkey(KeyCategories.AltKey, Keys.F12);
        }

        private void notifyIcon_DoubleClick(object sender, EventArgs e)
        {
            ShowHideVisualReader();
        }

        private void osHotKey_HotKeyPress(object sender, EventArgs e)
        {
            ShowHideVisualReader();
        }

        private void ShowHideVisualReader()
        {
            if (visualReader.Visible == false)
            {
                visualReader.Show();
            }
            else
            {
                visualReader.Hide();
            }
        }
        
        protected override void OnClosing(CancelEventArgs e)
        {
            base.OnClosing(e);
            this._osHotKey.UnregisterHotkey();
        }

        private void toolStripMenuItem1_Click(object sender, EventArgs e)
        {
            visualReader.Stop();
            visualReader.Close();
            visualReader = null;
            this.Close();
        }
    }
}
