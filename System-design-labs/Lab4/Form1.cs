using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Lab4
{
    public partial class Form1 : Form
    {
        // Object to control the cancellation of the async task
        private CancellationTokenSource _cancellationTokenSource;

        public Form1()
        {
            InitializeComponent();
        }

        private async void btnStart_Click(object sender, EventArgs e)
        {
            // UI setup before starting the task
            btnStart.Enabled = false;
            btnCancel.Enabled = true;
            progressBar1.Value = 0;
            lblProgress.Text = "0";

            _cancellationTokenSource = new CancellationTokenSource();

            // Setup the progress reporter (Task 3)
            var progress = new Progress<int>(value =>
            {
                lblProgress.Text = value.ToString();
                progressBar1.Value = value;
            });

            try
            {
                // Start the async operation (Task 1) and await its result (Task 2)
                int finalResult = await RunLongOperationAsync(100, progress, _cancellationTokenSource.Token);

                if (_cancellationTokenSource.Token.IsCancellationRequested)
                {
                    lblProgress.Text = $"Cancelled at {finalResult}%";
                }
                else
                {
                    lblProgress.Text = $"Done! Result: {finalResult}";
                }
            }
            finally
            {
                // Reset UI state after completion or cancellation
                btnStart.Enabled = true;
                btnCancel.Enabled = false;
                _cancellationTokenSource.Dispose();
                _cancellationTokenSource = null;
            }
        }

        private void btnCancel_Click(object sender, EventArgs e)
        {
            // Trigger cancellation (Task 4)
            if (_cancellationTokenSource != null)
            {
                _cancellationTokenSource.Cancel();
            }
        }

        // The asynchronous method running in a separate thread
        private Task<int> RunLongOperationAsync(int iterations, IProgress<int> progress, CancellationToken token)
        {
            return Task.Run(() =>
            {
                int i;
                for (i = 1; i <= iterations; i++)
                {
                    // Check if cancellation was requested
                    if (token.IsCancellationRequested)
                    {
                        return i;
                    }

                    // Report progress to the main UI thread
                    progress.Report(i);

                    // Simulate time-consuming work
                    Thread.Sleep(50);
                }

                return i - 1; // Return the final calculation result
            });
        }
    }
}
