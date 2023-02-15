namespace UI
{
    using AntonPaarWordCounter.Interfaces;
    using AntonPaarWordCounter.Models;

    public partial class MainForm : Form
    {
        private CancellationTokenSource? _wordCountCancellationToken;
        private IAntonPaarWordCounter _wordCounter;

        public MainForm(IAntonPaarWordCounter wordCounter)
        {
            InitializeComponent();
            _wordCounter = wordCounter;
        }

        private void SelectFile(object sender, EventArgs e)
        {
            StopWordCounting();

            var result = openFileDialog.ShowDialog();

            if (result == DialogResult.OK)
            {
                fileNameLbl.Text = openFileDialog.FileName;
            }
            else
            {
                fileNameLbl.Text = string.Empty;
            }
        }

        private async void StartCounting(object sender, EventArgs e)
        {
            StopWordCounting();

            UpdateControlState(true);

            dataGrid.DataSource = Array.Empty<WordCountItem>();
            var progress = new Progress<int>();
            statusText.Text = String.Empty;

            progress.ProgressChanged += Progress_ProgressChanged;
            _wordCountCancellationToken = new CancellationTokenSource();

            try
            {
                var parseResult = await _wordCounter.CountAsync(openFileDialog.FileName, progress, _wordCountCancellationToken.Token);
                dataGrid.DataSource = parseResult.Data.ToList();
                dataGrid.Refresh();
                statusText.Text = "Successfully completed.";
            }
            catch (OperationCanceledException)
            {
                statusText.Text = "The operation has been canceled.";
                progressBar.Value = 0;
            }
            finally
            {
                UpdateControlState(false);
            }
        }

        private void StopCounting(object sender, EventArgs e)
        {
            StopWordCounting();
            UpdateControlState(false);
        }

        private void Progress_ProgressChanged(object? sender, int percentageComplete)
        {
            progressBar.Value = percentageComplete;
        }

        private void StopWordCounting()
        {
            _wordCountCancellationToken?.Cancel();
        }

        private void UpdateControlState(bool isRunning)
        {
            btnStop.Enabled = isRunning;
            btnStart.Enabled = !isRunning;
            selectFileBtn.Enabled = !isRunning;
        }
    }
}