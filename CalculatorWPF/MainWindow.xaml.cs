using System;
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

namespace CalculatorWPF
{
    /// <summary>
    /// Logika interakcji dla klasy MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        string Equation { get; set; } = "";
        double Memory { get; set; }

        public MainWindow()
        {
            InitializeComponent();
            DataContext = this;
        }

        private void TextButton_Click(object sender, RoutedEventArgs e)
        {
            Equation += (sender as Button).Content.ToString();
            EquationBox.Text = Equation;
            Keyboard.Focus(buttonEquals);
        }

        private void MPlusButton_Click(object sender, RoutedEventArgs e)
        {
            double tmp;
            if (double.TryParse(Equation, out tmp))
            {
                Memory += tmp;
                MemoryBox.Text = Memory.ToString();
                Keyboard.Focus(buttonEquals);
            }
        }

        private void MRButton_Click(object sender, RoutedEventArgs e)
        {
            Memory = 0;
            MemoryBox.Text = Memory.ToString();
            Keyboard.Focus(buttonEquals);
        }

        private void MMinusButton_Click(object sender, RoutedEventArgs e)
        {
            double tmp;
            if (double.TryParse(Equation, out tmp))
            {
                Memory -= tmp;
                MemoryBox.Text = Memory.ToString();
                Keyboard.Focus(buttonEquals);
            }
        }

        private void MButton_Click(object sender, RoutedEventArgs e)
        {
            Equation += Memory.ToString();
            EquationBox.Text = Equation;
            Keyboard.Focus(buttonEquals);
        }

        private void KeyPressed(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                case Key.Enter:
                    buttonEquals.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D0:
                case Key.NumPad0:
                    button0.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D1:
                case Key.NumPad1:
                    button1.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D2:
                case Key.NumPad2:
                    button2.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D3:
                case Key.NumPad3:
                    button3.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D4:
                case Key.NumPad4:
                    button4.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D5:
                case Key.NumPad5:
                    button5.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D6:
                case Key.NumPad6:
                    button6.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D7:
                case Key.NumPad7:
                    button7.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D8:
                case Key.NumPad8:
                    button8.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.D9:
                case Key.NumPad9:
                    button9.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.OemPeriod:
                case Key.OemComma:
                case Key.Decimal:
                    buttonDot.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Add:
                    buttonPlus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Subtract:
                    buttonMinus.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Divide:
                    buttonDivide.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Multiply:
                    buttonTimes.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Back:
                    buttonBack.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
                case Key.Delete:
                    buttonClear.RaiseEvent(new RoutedEventArgs(Button.ClickEvent));
                    break;
            }

            Keyboard.Focus(buttonEquals);
        }

        private void BackspaceButton_Click(object sender, RoutedEventArgs e)
        {
            if (Equation.Length > 0)
            {
                if (Equation[Equation.Length - 1] != ' ')
                {
                    Equation = Equation.Substring(0, Equation.Length - 1);
                }
                else
                {
                    Equation = Equation.Substring(0, Equation.Length - 3);
                }
                EquationBox.Text = Equation;
            }
            Keyboard.Focus(buttonEquals);
        }

        private void EqualsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Equation.Length > 0)
            {
                double result = CalculatorBackEnd.Calculate(Equation);
                EquationBox.Text = result.ToString();
                previousList.Items.Insert(0, new Equation(Equation, result));
                Equation = result.ToString();
            }
            Keyboard.Focus(buttonEquals);
        }

        private void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            Equation = "";
            EquationBox.Text = "0";
            Keyboard.Focus(buttonEquals);
        }

        private void PreviousList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            Equation eq = (Equation)previousList.SelectedItem;
            Equation = eq.Text;
            EquationBox.Text = eq.Text;
            Keyboard.Focus(buttonEquals);
        }

        private void ButtonPlusMinus_Click(object sender, RoutedEventArgs e)
        {
            if (Equation[0] != '-')
            {
                Equation = "- 1 * ( " + Equation + " ) ";
            }
            else
            {
                Equation = Equation.Substring(8, Equation.Length - 11);
            }
            EquationBox.Text = Equation;
            Keyboard.Focus(buttonEquals);
        }
    }
}
