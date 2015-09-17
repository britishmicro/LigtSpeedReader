using System;
using System.Windows.Forms;
using System.Speech.Synthesis;

namespace LigtSpeedReaderLite
{
    public partial class VisualReader : Form
    {
        SpeechSynthesizer synth = new SpeechSynthesizer();
        int selStart = 0;
        string lastWord = "";
        int offset = 0;

        public VisualReader()
        {
            InitializeComponent();
            synth.SpeakProgress += new EventHandler<SpeakProgressEventArgs>(synth_SpeakProgress);
            synth.SpeakCompleted += new EventHandler<SpeakCompletedEventArgs>(synth_SpeakCompleted);
            synth.Rate = 2;
        }

        private void synth_SpeakProgress(object sender, SpeakProgressEventArgs e)
        {
            textboxWordDisplay.Text = e.Text;
            if (lastWord == e.Text)
            {
                return;
            }
            int selLength = e.Text.Length;
            offset = textboxToSpeak.Text.IndexOf(e.Text, selStart, StringComparison.InvariantCultureIgnoreCase);
            if (offset >= 0)
            {
                textboxToSpeak.Select(offset, selLength);
                textboxToSpeak.ScrollToCaret();
                selStart = offset + selLength;
                lastWord = e.Text;
            }
        }

        private void synth_SpeakCompleted(object sender, SpeakCompletedEventArgs e)
        {
            selStart = 0;
            lastWord = "";
            textboxWordDisplay.Text = "....";
            textboxToSpeak.Select(selStart, 0);
        }

        private void VisualReader_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                PlayPause();
            }
        }

        private void textboxToSpeak_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Alt && e.KeyCode == Keys.F5)
            {
                Stop();
            }
            else if (e.KeyCode == Keys.F5)
            {
                PlayPause();
            }

            if (e.Alt && e.KeyCode == Keys.Left)
            {
                Slower();
            }

            if (e.Alt && e.KeyCode == Keys.Right)
            {
                Faster();
            }

            if (e.Alt && e.KeyCode == Keys.Insert)
            {
                Stop();
                textboxToSpeak.Enabled = true;
                textboxToSpeak.Text = Clipboard.GetText();
            }
        }

        private void Faster()
        {
            if (synth.Rate < 10)
            {
                synth.Pause();
                synth.SpeakAsyncCancelAll();
                synth.Rate = synth.Rate + 1;
                synth.SpeakAsync(this.textboxToSpeak.Text.Substring(offset));
                synth.Resume();
            }
        }

        private void Slower()
        {
            if (synth.Rate > -10)
            {
                synth.Pause();
                synth.SpeakAsyncCancelAll();
                synth.Rate = synth.Rate - 1;
                synth.SpeakAsync(this.textboxToSpeak.Text.Substring(offset));
                synth.Resume();
            }
        }

        public void Stop()
        {
            synth.SpeakAsyncCancelAll();
            selStart = 0;
            lastWord = "";
            textboxWordDisplay.Text = "....";
            textboxToSpeak.Select(selStart, 0);
        }


        private void PlayPause()
        {
            switch (synth.State)
            {
                case SynthesizerState.Speaking:
                    synth.Pause();
                    break;
                case SynthesizerState.Paused:
                    synth.Resume();
                    break;
                default:
                    synth.SpeakAsync(textboxToSpeak.Text);
                    break;
            }
        }

        protected override void OnVisibleChanged(EventArgs e)
        {
            base.OnVisibleChanged(e);
            if (this.Visible == false)
            {
                if (synth.State == SynthesizerState.Speaking)
                {
                    synth.Pause();
                }
            }
        }

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            //Stop();
            synth.Pause();
            e.Cancel = true;
            Hide();
        }

    }
}
