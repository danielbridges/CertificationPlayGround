namespace WinformWithThreading
{
    using System;
    using System.ComponentModel;
    using System.Windows.Forms;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, EventArgs e)
        {
            StartThread();
        }

        private void Cancel_Click(object sender, EventArgs e)
        {
            // Cancel the asynchronous operation.
            backgroundWorker1.CancelAsync();
        }

        private void StartThread()
        {
            // This method runs on the main thread.
            WordsCounted.Text = 0.ToString();

            // Initialize the object that the background worker calls.
            var wc = new Words
            {
                CompareString = CompareString.Text,
                SourceFile = SourceFile.Text
            };

            // Start the asynchronous operation.
            backgroundWorker1.RunWorkerAsync(wc);
        }

        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            // This event handler is called when the background thread finishes.
            // This method runs on the main thread.
            if (e.Error != null)
                MessageBox.Show("Error: " + e.Error.Message);
            else if (e.Cancelled)
                MessageBox.Show("Words counting canceled.");
            else
                MessageBox.Show("Finished counting words.");
        }

        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            // This method runs on the main thread.
            var state =
                (Words.CurrentState)e.UserState;
            LinesCounted.Text = state.LinesCounted.ToString();
            WordsCounted.Text = state.WordsMatched.ToString();
        }

        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            // This event handler is where the actual work is done.
            // This method runs on the background thread.

            // Get the BackgroundWorker object that raised this event.
            var worker = (BackgroundWorker)sender;

            // Get the Words object and call the main method.
            var wc = (Words)e.Argument;
            wc.CountWords(worker, e);
        }
    }
}
