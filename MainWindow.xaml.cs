using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Threading;

namespace KeyboardTrainer
{
    public partial class MainWindow : Window
    {
        private string targetString = "";
        private int currentPos = 0;
        private int errors = 0;
        private DateTime startTime;
        private DispatcherTimer timer;
        private bool isUpperCase = false;

        public MainWindow()
        {
            InitializeComponent();
        }

        private void Start_Click(object sender, RoutedEventArgs e)
        {
            targetString = GenerateString();
            TargetText.Text = targetString;
            currentPos = 0;
            errors = 0;
            ErrorText.Text = "0";
            SpeedText.Text = "0";

            StartBtn.IsEnabled = false;
            StopBtn.IsEnabled = true;
            CaseCheck.IsEnabled = false;
            LengthSlider.IsEnabled = false;

            startTime = DateTime.Now;
            timer = new DispatcherTimer { Interval = TimeSpan.FromSeconds(1) };
            timer.Tick += UpdateStats;
            timer.Start();
        }

        private string GenerateString()
        {
            string chars = "qwertyuiopasdfghjklzxcvbnm`-=[]\\;',./";
            if (CaseCheck.IsChecked == true) chars += "QWERTYUIOPASDFGHJKLZXCVBNM";

            Random rnd = new Random();
            char[] result = new char[(int)LengthSlider.Value];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = chars[rnd.Next(chars.Length)];
            }
            return new string(result);
        }

        private void UpdateStats(object sender, EventArgs e)
        {
            double minutes = (DateTime.Now - startTime).TotalMinutes;
            if (minutes > 0)
            {
                int speed = (int)(currentPos / minutes);
                SpeedText.Text = speed.ToString();
            }
        }

        private void Stop_Click(object sender, RoutedEventArgs e)
        {
            timer.Stop();
            TargetText.Text = "";
            StartBtn.IsEnabled = true;
            StopBtn.IsEnabled = false;
            CaseCheck.IsEnabled = true;
            LengthSlider.IsEnabled = true;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (!StopBtn.IsEnabled) return;

            string keyStr = e.Key.ToString();
            bool isCorrect = false;
            char pressedChar = '\0';

            // Обработка специальных символов
            switch (e.Key)
            {
                case Key.OemMinus:
                    pressedChar = '-';
                    break;
                case Key.OemPlus:
                    pressedChar = '=';
                    break;
                case Key.OemTilde:
                    pressedChar = '`';
                    break;
                case Key.OemComma:
                    pressedChar = ',';
                    break;
                case Key.OemPeriod:
                    pressedChar = '.';
                    break;
                case Key.OemQuestion:
                    pressedChar = '/';
                    break;
                case Key.OemOpenBrackets:
                    pressedChar = '[';
                    break;
                case Key.OemCloseBrackets:
                    pressedChar = ']';
                    break;
                case Key.OemPipe:
                    pressedChar = '\\';
                    break;
                case Key.OemSemicolon:
                    pressedChar = ';';
                    break;
                case Key.OemQuotes:
                    pressedChar = '\'';
                    break;
                case Key.Space:
                    pressedChar = ' ';
                    break;

                default:
                    if (keyStr.Length == 1 && char.IsLetterOrDigit(keyStr[0]))
                    {
                        pressedChar = keyStr[0];
                    }
                    break;
            }

            if (pressedChar != '\0')
            {
                char targetChar = CaseCheck.IsChecked == true ?
                    targetString[currentPos] :
                    char.ToLower(targetString[currentPos]);

                char pressed = CaseCheck.IsChecked == true ?
                    pressedChar :
                    char.ToLower(pressedChar);

                isCorrect = pressed == targetChar;
            }
            // Обработка Shift
            else if (e.Key == Key.LeftShift || e.Key == Key.RightShift)
            {
                ToggleCase();
                return;
            }

            if (isCorrect)
            {
                currentPos++;
                if (currentPos >= targetString.Length)
                {
                    Stop_Click(null, null);
                    MessageBox.Show("Задание завершено!");
                }
                else
                {
                    TargetText.Text = targetString.Substring(currentPos);
                }
            }
            else if (pressedChar != '\0')
            {
                errors++;
                ErrorText.Text = errors.ToString();
            }

            HighlightKey(e.Key, true);
        }

        private void Window_KeyUp(object sender, KeyEventArgs e)
        {
            HighlightKey(e.Key, false);
        }

        private void ToggleCase()
        {
            isUpperCase = !isUpperCase;
            foreach (var child in ((StackPanel)((ScrollViewer)Content).Content).Children)
            {
                if (child is StackPanel row)
                {
                    foreach (var element in row.Children)
                    {
                        if (element is Button btn && btn.Content.ToString().Length == 1 && char.IsLetter(btn.Content.ToString()[0]))
                        {
                            btn.Content = isUpperCase ? btn.Content.ToString().ToUpper() : btn.Content.ToString().ToLower();
                        }
                    }
                }
            }
        }

        private void HighlightKey(Key key, bool isPressed)
        {
            Button keyButton = null;

            switch (key)
            {
                case Key.D1: keyButton = Key1; break;
                case Key.D2: keyButton = Key2; break;
                case Key.D3: keyButton = Key3; break;
                case Key.D4: keyButton = Key4; break;
                case Key.D5: keyButton = Key5; break;
                case Key.D6: keyButton = Key6; break;
                case Key.D7: keyButton = Key7; break;
                case Key.D8: keyButton = Key8; break;
                case Key.D9: keyButton = Key9; break;
                case Key.D0: keyButton = Key0; break;
                case Key.Q: keyButton = KeyQ; break;
                case Key.W: keyButton = KeyW; break;
                case Key.E: keyButton = KeyE; break;
                case Key.R: keyButton = KeyR; break;
                case Key.T: keyButton = KeyT; break;
                case Key.Y: keyButton = KeyY; break;
                case Key.U: keyButton = KeyU; break;
                case Key.I: keyButton = KeyI; break;
                case Key.O: keyButton = KeyO; break;
                case Key.P: keyButton = KeyP; break;
                case Key.A: keyButton = KeyA; break;
                case Key.S: keyButton = KeyS; break;
                case Key.D: keyButton = KeyD; break;
                case Key.F: keyButton = KeyF; break;
                case Key.G: keyButton = KeyG; break;
                case Key.H: keyButton = KeyH; break;
                case Key.J: keyButton = KeyJ; break;
                case Key.K: keyButton = KeyK; break;
                case Key.L: keyButton = KeyL; break;
                case Key.Z: keyButton = KeyZ; break;
                case Key.X: keyButton = KeyX; break;
                case Key.C: keyButton = KeyC; break;
                case Key.V: keyButton = KeyV; break;
                case Key.B: keyButton = KeyB; break;
                case Key.N: keyButton = KeyN; break;
                case Key.M: keyButton = KeyM; break;
                case Key.Space: keyButton = KeySpace; break;
                case Key.OemMinus: keyButton = KeyMinus; break;
                case Key.OemPlus: keyButton = KeyEquals; break;
                case Key.OemTilde: keyButton = KeyBackQuote; break;
                case Key.OemComma: keyButton = KeyComma; break;
                case Key.OemPeriod: keyButton = KeyPeriod; break;
                case Key.OemQuestion: keyButton = KeySlash; break;
                case Key.OemOpenBrackets: keyButton = KeyLeftBracket; break;
                case Key.OemCloseBrackets: keyButton = KeyRightBracket; break;
                case Key.OemPipe: keyButton = KeyBackslash; break;
                case Key.OemSemicolon: keyButton = KeySemicolon; break;
                case Key.OemQuotes: keyButton = KeyApostrophe; break;
            }

            if (keyButton != null)
            {
                var border = (Border)VisualTreeHelper.GetChild(keyButton, 0);
                border.Background = isPressed ? Brushes.LightBlue : Brushes.LightGray;
            }
        }
    }
}
