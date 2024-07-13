using SharpCalc.Components;

namespace WithInterface
{

    public partial class FunctionBar : UserControl
    {       
        public event EventHandler? OnFunctionSelected;
        void InitializeTags()
        {
          
            btnSin.Tag = StaticDataBank.Sine;
            btnCos.Tag = StaticDataBank.Cosine;
            btnTan.Tag = StaticDataBank.Tangent;
            btnCbrt.Tag = StaticDataBank.CubicRoot;
            btnLn.Tag = StaticDataBank.NaturalLogarithm;
            btnRandom.Tag = StaticDataBank.Random;
        }
        public FunctionBar()
        {
            InitializeComponent();
            InitializeTags();
        }
        private void OnButtonClick(object sender, EventArgs eventArgs)
        {                      
            OnFunctionSelected?.Invoke(sender, EventArgs.Empty);
        }
    }
}
