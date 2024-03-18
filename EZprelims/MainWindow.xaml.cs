using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;

namespace EZprelims
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private Random random = new Random();
        private int targetNumber;
        private DispatcherTimer gameTimer;
        private int time = 60;
        private int timeLeft; // Initial timer duration in seconds
        private int score = 0;
        private int round = 0;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            targetNumber = random.Next(1, 256); // Generate random number from 1 to 255
            NumberTextBlock.Text = $"Decimal Number: {targetNumber}";
            score = 0;
            ResetAnswerBoxes();
            StartTimer();

            Start.Visibility = Visibility.Collapsed;
        }

        private void StartTimer()
        {
            timeLeft = time;
            gameTimer = new DispatcherTimer();
            gameTimer.Interval = new TimeSpan(0, 0, 0, 1);
            gameTimer.Tick += GameTimer_Tick;
            gameTimer.Start();
        }

        private void GameTimer_Tick(object sender, EventArgs e)
        {
            timeLeft--;
            UpdateTimerDisplay();

            if (timeLeft <= 0) // Check if time has run out meanig the game is over
            {
                gameTimer.Stop();
                MessageBox.Show($"Time's up! Game Over! Your score: {score}");
                time = 60;
                Start.Visibility = Visibility.Visible;
            }
        }

        private void UpdateTimerDisplay()
        {
            TimerTextBlock.Text = $"Time Left: {timeLeft} seconds";
        }

        private void NewTime()
        {
            if (round < 11)
            {
                time -= (int)Math.Ceiling(60 * 0.066);
                round++;
            }

            timeLeft = time;
            UpdateTimerDisplay();
        }

        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            string binaryAnswer = "";
            binaryAnswer += Bit1.Text.Trim();
            binaryAnswer += Bit2.Text.Trim();
            binaryAnswer += Bit3.Text.Trim();
            binaryAnswer += Bit4.Text.Trim();
            binaryAnswer += Bit5.Text.Trim();
            binaryAnswer += Bit6.Text.Trim();
            binaryAnswer += Bit7.Text.Trim();
            binaryAnswer += Bit8.Text.Trim();

            if (IsValidBinary(binaryAnswer))
            {
                int userAnswer = Convert.ToInt32(binaryAnswer, 2);
                if (userAnswer == targetNumber)
                {
                    score++;
                    targetNumber = random.Next(1, 256);
                    NumberTextBlock.Text = $"Decimal Number: {targetNumber}";
                    UpdateScoreDisplay();
                    NewTime();
                    ResetAnswerBoxes();
                }
            }
            else
            {
                MessageBox.Show("Invalid binary input. Please enter a valid 8-bit binary number.");
            }
        }

        private void UpdateScoreDisplay()
        {
            ScoreTextBlock.Text = $"Score: {score}";
        }

        private bool IsValidBinary(string input)
        {
            return input.Length == 8 && IsBinary(input);
        }

        private bool IsBinary(string input)
        {
            foreach (char c in input)
            {
                if (c != '0' && c != '1')
                {
                    return false;
                }
            }
            return true;
        }

        private void ResetAnswerBoxes()
        {
            Bit1.Text = "";
            Bit2.Text = "";
            Bit3.Text = "";
            Bit4.Text = "";
            Bit5.Text = "";
            Bit6.Text = "";
            Bit7.Text = "";
            Bit8.Text = "";
        }

        private void BitTextBox_PreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            TextBox currentTextBox = sender as TextBox;
            if (currentTextBox != null)
            {
                int index = int.Parse(currentTextBox.Name.Substring(3));
                if (index < 8)
                {
                    TextBox nextTextBox = FindNextTextBox(index);
                    if (nextTextBox != null && currentTextBox.Text.Length > 0)
                    {
                        nextTextBox.Focus();
                    }
                }
                else if (currentTextBox.Text.Length > 0)
                {
                    SubmitButton.Focus();
                }
            }
        }

        private TextBox FindNextTextBox(int currentIndex)
        {
            int nextIndex = currentIndex + 1;
            string nextTextBoxName = "Bit" + nextIndex;
            return FindName(nextTextBoxName) as TextBox;
        }
    }
}