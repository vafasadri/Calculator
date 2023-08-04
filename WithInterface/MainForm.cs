using SharpCalc;
using System.Text;
#pragma warning disable IDE1006 // Naming Styles
namespace WithInterface
{
    public partial class MainForm : Form
    {
        readonly Stack<LinkedList<object>> layers = new();
        private readonly LinkedList<object> expression = new();
        string ExpressionToString(LinkedList<object> expression)
        {
            StringBuilder builder = new();
            foreach (var item in expression)
            {
                if (item is SharpCalc.DataModels.IDataModel model)
                {
                    builder.Append(model.Name);
                }
                else if (item is SharpCalc.Symbol symbol)
                {
                    builder.Append(SharpCalc.SymbolIO.Represent(symbol));
                }
                else if (item is double num)
                {
                    builder.Append(num);
                }
                else if (item is LinkedList<object> child)
                {
                    builder.Append('(');
                    builder.Append(ExpressionToString(child));
                    if (!layers.Contains(child)) builder.Append(')');
                }
            }
            return builder.ToString();
        }
        void InitializeTags()
        {
            btnPower.Tag = SharpCalc.Symbol.Power;
            btnMinus.Tag = SharpCalc.Symbol.Minus;
            btnPlus.Tag = SharpCalc.Symbol.Plus;
            btnMultiply.Tag = SharpCalc.Symbol.Cross;
            btnDivide.Tag = SharpCalc.Symbol.Slash;
            btnAbs.Tag = SharpCalc.StaticData.AbsoluteValue;
            btnCeil.Tag = SharpCalc.StaticData.Ceiling;
            btnFloor.Tag = SharpCalc.StaticData.Floor;
            btnSqrt.Tag = SharpCalc.StaticData.SquareRoot;
            btnDeg.Tag = SharpCalc.StaticData.Degree;
        }
        void UpdateInterface(bool redrawExpression)
        {
            if (redrawExpression) txtExpression.Text = ExpressionToString(expression);
            btnBracketClose.Enabled = layers.Count > 1;
            btnDot.Enabled = txtNumber.TextLength > 0 & !txtNumber.Text.Contains('.');            
            btnBackspace.Enabled = txtNumber.TextLength > 0;
            btnClear.Enabled = txtNumber.TextLength > 0 || expression.Count > 0;           
        }


        private void btnExtraFunction_Click(object sender, EventArgs e)
        {
            functionBar1.Visible = !functionBar1.Visible;
            functionBar1.Focus();
        }

        private void FunctionBar1_Leave(object sender, EventArgs e)
        {
            functionBar1.Visible = false;
        }
        private void btnEquals_Click(object sender, EventArgs e)
        {
            EmptyNumber();
            txtExpression.Text = String.Empty;
            var result = SharpCalc.Translator.SolveExternal(expression);
            txtNumber.Text = result.ToText();
            expression.Clear();
            layers.Clear();
        }
        private void btnDigit_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            txtNumber.Text += btn.Text;
        }
        private void btnBackSpace_Click(object sender, EventArgs e)
        {
            if (txtNumber.TextLength > 0)
                txtNumber.Text = txtNumber.Text.Remove(txtNumber.Text.Length - 1);
            UpdateInterface(false);
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            txtExpression.Text = String.Empty;
            txtNumber.Clear();
            expression.Clear();
            layers.Clear();
            layers.Push(expression);
            UpdateInterface(false);
        }
        void EmptyNumber()
        {
            if (txtNumber.TextLength > 0)
            {
                layers.Peek().AddLast(double.Parse(txtNumber.Text));
                txtNumber.Clear();
            }
        }

        private void btnOP_Click(object sender, EventArgs e)
        {
            EmptyNumber();
            var btn = (Button)sender;
            layers.Peek().AddLast(btn.Tag);
            
            UpdateInterface(true);
        }
        private void btnFunction_Click(object sender, EventArgs e)
        {
            var btn = (Button)sender;
            EmptyNumber();
            var current = layers.Peek();
            current.AddLast(btn.Tag);
            current.AddLast(SharpCalc.Symbol.Invisible_FunctionCall);          
            
            btnBracketOpen_Click(this,EventArgs.Empty);
            
        }
        private void txtNumber_TextChanged(object sender, EventArgs e)
        {
            UpdateInterface(false);
        }

        private void btnBracketOpen_Click(object sender, EventArgs e)
        {
            EmptyNumber();
            var newlayer = new LinkedList<object>();
            layers.Peek().AddLast(newlayer);
            layers.Push(newlayer);
            
            UpdateInterface(true);
        }

        private void btnBracketClose_Click(object sender, EventArgs e)
        {
            EmptyNumber();
            layers.Pop();
            
            UpdateInterface(true);
        }
        private void MainForm_KeyPress(object sender, KeyEventArgs e)
        {
            var btn = e.KeyCode switch
            {
                Keys.NumPad0 or Keys.D0 => btnDigit0,
                Keys.NumPad1 or Keys.D1 => btnDigit1,
                Keys.NumPad2 or Keys.D2 => btnDigit2,
                Keys.NumPad3 or Keys.D3 => btnDigit3,
                Keys.NumPad4 or Keys.D4 => btnDigit4,
                Keys.NumPad5 or Keys.D5 => btnDigit5,
                Keys.NumPad6 or Keys.D6 => btnDigit6,
                Keys.NumPad7 or Keys.D7 => btnDigit7,
                Keys.NumPad8 or Keys.D8 => btnDigit8,
                Keys.NumPad9 or Keys.D9 => btnDigit9,
                Keys.OemPeriod => btnDot,
                Keys.Add => btnPlus,
                Keys.Enter => btnEquals,
                Keys.Subtract => btnMinus,
                Keys.Multiply => btnMultiply,
                Keys.Divide => btnDivide,
                Keys.Back => btnBackspace,

                _ => null
            };
            if (btn != null)
            {
                if (btn.Enabled)
                {
                    btn.PerformClick();
                }
                e.Handled = true;
            }
        }
        public MainForm()
        {
            InitializeComponent();
            InitializeTags();
            layers.Push(expression);
            functionBar1.OnFunctionSelected += btnFunction_Click;
        }
    }
}