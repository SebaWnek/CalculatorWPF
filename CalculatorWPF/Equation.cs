namespace CalculatorWPF
{
    internal class Equation
    {
        internal string Text { get; }
        double Result { get; }
        public Equation(string text, double result)
        {
            Text = text;
            Result = result;
        }
        public override string ToString()
        {
            return Text + " = " + Result;
        }
    }
}