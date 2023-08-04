namespace WithInterface
{

    public partial class FunctionBar : UserControl
    {       
        public event EventHandler? OnFunctionSelected;
        void InitializeTags()
        {
          
            btnSin.Tag = SharpCalc.StaticData.Sine;
            btnCos.Tag = SharpCalc.StaticData.Cosine;
            btnTan.Tag = SharpCalc.StaticData.Tangent;
            btnCbrt.Tag = SharpCalc.StaticData.CubicRoot;
            btnLn.Tag = SharpCalc.StaticData.NaturalLogarithm;
            btnRandom.Tag = SharpCalc.StaticData.Random;
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
