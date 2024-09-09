using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace MultiProcessWPF
{
    /// <summary>
    /// Interaction logic for PrimeNumber.xaml
    /// </summary>
    public partial class PrimeNumber : UserControl
    {
        // Current number to check
        private long _num = 3;
        private bool _runCalculation = false;

        public PrimeNumber()
        {
            InitializeComponent();
            bigPrime.Text = _num.ToString();
        }

        private void StartStopButton_Click(object sender, RoutedEventArgs e)
        {
            _runCalculation = !_runCalculation;

            if (_runCalculation)
            {
                StartStopButton.Content = "Stop";
                StartStopButton.Dispatcher.InvokeAsync(CheckNextNumber, DispatcherPriority.SystemIdle);
            }
            else
                StartStopButton.Content = "Resume";
        }

        public void CheckNextNumber()
        {
            // Reset flag.
            _isPrime = true;

            for (long i = 3; i <= Math.Sqrt(_num); i++)
            {
                if (_num % i == 0)
                {
                    // Set not a prime flag to true.
                    _isPrime = false;
                    break;
                }
            }

            // If a prime number, update the UI text
            if (_isPrime)
                bigPrime.Text = _num.ToString();

            _num += 2;

            // Requeue this method on the dispatcher
            if (_runCalculation)
                StartStopButton.Dispatcher.InvokeAsync(CheckNextNumber, DispatcherPriority.SystemIdle);
        }

        private bool _isPrime = false;
    }
}
