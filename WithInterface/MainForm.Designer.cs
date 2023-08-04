namespace WithInterface
{
    partial class MainForm
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.txtNumber = new System.Windows.Forms.TextBox();
            this.btnEquals = new System.Windows.Forms.Button();
            this.btnDot = new System.Windows.Forms.Button();
            this.btnDigit3 = new System.Windows.Forms.Button();
            this.btnDigit2 = new System.Windows.Forms.Button();
            this.btnDigit1 = new System.Windows.Forms.Button();
            this.btnDigit4 = new System.Windows.Forms.Button();
            this.btnDigit5 = new System.Windows.Forms.Button();
            this.btnDigit6 = new System.Windows.Forms.Button();
            this.btnDigit7 = new System.Windows.Forms.Button();
            this.btnDigit8 = new System.Windows.Forms.Button();
            this.btnDigit9 = new System.Windows.Forms.Button();
            this.btnDigit0 = new System.Windows.Forms.Button();
            this.btnMinus = new System.Windows.Forms.Button();
            this.btnPlus = new System.Windows.Forms.Button();
            this.btnMultiply = new System.Windows.Forms.Button();
            this.btnDivide = new System.Windows.Forms.Button();
            this.btnBracketClose = new System.Windows.Forms.Button();
            this.btnBracketOpen = new System.Windows.Forms.Button();
            this.btnDeg = new System.Windows.Forms.Button();
            this.btnPower = new System.Windows.Forms.Button();
            this.btnExtraFunction = new System.Windows.Forms.Button();
            this.btnSqrt = new System.Windows.Forms.Button();
            this.functionBar1 = new WithInterface.FunctionBar();
            this.btnBackspace = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnCeil = new System.Windows.Forms.Button();
            this.btnAbs = new System.Windows.Forms.Button();
            this.btnFloor = new System.Windows.Forms.Button();
            this.txtExpression = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // txtNumber
            // 
            this.txtNumber.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(64)))), ((int)(((byte)(0)))));
            this.txtNumber.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtNumber.Font = new System.Drawing.Font("Digital-7", 30F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtNumber.Location = new System.Drawing.Point(14, 31);
            this.txtNumber.Margin = new System.Windows.Forms.Padding(3, 3, 100, 3);
            this.txtNumber.Name = "txtNumber";
            this.txtNumber.PlaceholderText = "----";
            this.txtNumber.ReadOnly = true;
            this.txtNumber.Size = new System.Drawing.Size(279, 40);
            this.txtNumber.TabIndex = 0;
            this.txtNumber.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtNumber.WordWrap = false;
            this.txtNumber.TextChanged += new System.EventHandler(this.txtNumber_TextChanged);
            // 
            // btnEquals
            // 
            this.btnEquals.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnEquals.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnEquals.FlatAppearance.BorderSize = 3;
            this.btnEquals.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnEquals.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnEquals.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnEquals.Location = new System.Drawing.Point(242, 304);
            this.btnEquals.Name = "btnEquals";
            this.btnEquals.Size = new System.Drawing.Size(51, 105);
            this.btnEquals.TabIndex = 2;
            this.btnEquals.Text = "=";
            this.btnEquals.UseVisualStyleBackColor = false;
            this.btnEquals.Click += new System.EventHandler(this.btnEquals_Click);
            // 
            // btnDot
            // 
            this.btnDot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDot.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDot.FlatAppearance.BorderSize = 3;
            this.btnDot.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDot.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDot.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDot.Location = new System.Drawing.Point(185, 359);
            this.btnDot.Name = "btnDot";
            this.btnDot.Size = new System.Drawing.Size(51, 50);
            this.btnDot.TabIndex = 3;
            this.btnDot.Text = ".";
            this.btnDot.UseVisualStyleBackColor = false;
            this.btnDot.Click += new System.EventHandler(this.btnDigit_Click);
            // 
            // btnDigit3
            // 
            this.btnDigit3.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDigit3.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDigit3.FlatAppearance.BorderSize = 3;
            this.btnDigit3.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigit3.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDigit3.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDigit3.Location = new System.Drawing.Point(185, 303);
            this.btnDigit3.Name = "btnDigit3";
            this.btnDigit3.Size = new System.Drawing.Size(51, 50);
            this.btnDigit3.TabIndex = 4;
            this.btnDigit3.Text = "3";
            this.btnDigit3.UseVisualStyleBackColor = false;
            this.btnDigit3.Click += new System.EventHandler(this.btnDigit_Click);
            // 
            // btnDigit2
            // 
            this.btnDigit2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDigit2.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDigit2.FlatAppearance.BorderSize = 3;
            this.btnDigit2.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigit2.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDigit2.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDigit2.Location = new System.Drawing.Point(128, 303);
            this.btnDigit2.Name = "btnDigit2";
            this.btnDigit2.Size = new System.Drawing.Size(51, 50);
            this.btnDigit2.TabIndex = 5;
            this.btnDigit2.Text = "2";
            this.btnDigit2.UseVisualStyleBackColor = false;
            this.btnDigit2.Click += new System.EventHandler(this.btnDigit_Click);
            // 
            // btnDigit1
            // 
            this.btnDigit1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDigit1.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDigit1.FlatAppearance.BorderSize = 3;
            this.btnDigit1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigit1.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDigit1.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDigit1.Location = new System.Drawing.Point(71, 303);
            this.btnDigit1.Name = "btnDigit1";
            this.btnDigit1.Size = new System.Drawing.Size(51, 50);
            this.btnDigit1.TabIndex = 6;
            this.btnDigit1.Tag = "1";
            this.btnDigit1.Text = "1";
            this.btnDigit1.UseVisualStyleBackColor = false;
            this.btnDigit1.Click += new System.EventHandler(this.btnDigit_Click);
            // 
            // btnDigit4
            // 
            this.btnDigit4.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDigit4.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDigit4.FlatAppearance.BorderSize = 3;
            this.btnDigit4.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigit4.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDigit4.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDigit4.Location = new System.Drawing.Point(71, 247);
            this.btnDigit4.Name = "btnDigit4";
            this.btnDigit4.Size = new System.Drawing.Size(51, 50);
            this.btnDigit4.TabIndex = 9;
            this.btnDigit4.Text = "4";
            this.btnDigit4.UseVisualStyleBackColor = false;
            this.btnDigit4.Click += new System.EventHandler(this.btnDigit_Click);
            // 
            // btnDigit5
            // 
            this.btnDigit5.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDigit5.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDigit5.FlatAppearance.BorderSize = 3;
            this.btnDigit5.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigit5.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDigit5.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDigit5.Location = new System.Drawing.Point(128, 247);
            this.btnDigit5.Name = "btnDigit5";
            this.btnDigit5.Size = new System.Drawing.Size(51, 50);
            this.btnDigit5.TabIndex = 8;
            this.btnDigit5.Text = "5";
            this.btnDigit5.UseVisualStyleBackColor = false;
            this.btnDigit5.Click += new System.EventHandler(this.btnDigit_Click);
            // 
            // btnDigit6
            // 
            this.btnDigit6.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDigit6.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDigit6.FlatAppearance.BorderSize = 3;
            this.btnDigit6.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigit6.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDigit6.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDigit6.Location = new System.Drawing.Point(185, 247);
            this.btnDigit6.Name = "btnDigit6";
            this.btnDigit6.Size = new System.Drawing.Size(51, 50);
            this.btnDigit6.TabIndex = 7;
            this.btnDigit6.Text = "6";
            this.btnDigit6.UseVisualStyleBackColor = false;
            this.btnDigit6.Click += new System.EventHandler(this.btnDigit_Click);
            // 
            // btnDigit7
            // 
            this.btnDigit7.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDigit7.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDigit7.FlatAppearance.BorderSize = 3;
            this.btnDigit7.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigit7.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDigit7.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDigit7.Location = new System.Drawing.Point(71, 191);
            this.btnDigit7.Name = "btnDigit7";
            this.btnDigit7.Size = new System.Drawing.Size(51, 50);
            this.btnDigit7.TabIndex = 12;
            this.btnDigit7.Text = "7";
            this.btnDigit7.UseVisualStyleBackColor = false;
            this.btnDigit7.Click += new System.EventHandler(this.btnDigit_Click);
            // 
            // btnDigit8
            // 
            this.btnDigit8.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDigit8.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDigit8.FlatAppearance.BorderSize = 3;
            this.btnDigit8.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigit8.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDigit8.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDigit8.Location = new System.Drawing.Point(128, 191);
            this.btnDigit8.Name = "btnDigit8";
            this.btnDigit8.Size = new System.Drawing.Size(51, 50);
            this.btnDigit8.TabIndex = 11;
            this.btnDigit8.Text = "8";
            this.btnDigit8.UseVisualStyleBackColor = false;
            this.btnDigit8.Click += new System.EventHandler(this.btnDigit_Click);
            // 
            // btnDigit9
            // 
            this.btnDigit9.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDigit9.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDigit9.FlatAppearance.BorderSize = 3;
            this.btnDigit9.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigit9.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDigit9.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDigit9.Location = new System.Drawing.Point(185, 191);
            this.btnDigit9.Name = "btnDigit9";
            this.btnDigit9.Size = new System.Drawing.Size(51, 50);
            this.btnDigit9.TabIndex = 10;
            this.btnDigit9.Text = "9";
            this.btnDigit9.UseVisualStyleBackColor = false;
            this.btnDigit9.Click += new System.EventHandler(this.btnDigit_Click);
            // 
            // btnDigit0
            // 
            this.btnDigit0.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDigit0.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDigit0.FlatAppearance.BorderSize = 3;
            this.btnDigit0.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDigit0.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDigit0.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDigit0.Location = new System.Drawing.Point(14, 359);
            this.btnDigit0.Name = "btnDigit0";
            this.btnDigit0.Size = new System.Drawing.Size(165, 50);
            this.btnDigit0.TabIndex = 13;
            this.btnDigit0.Text = "0";
            this.btnDigit0.UseVisualStyleBackColor = false;
            this.btnDigit0.Click += new System.EventHandler(this.btnDigit_Click);
            // 
            // btnMinus
            // 
            this.btnMinus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnMinus.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnMinus.FlatAppearance.BorderSize = 3;
            this.btnMinus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMinus.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnMinus.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnMinus.Location = new System.Drawing.Point(242, 191);
            this.btnMinus.Name = "btnMinus";
            this.btnMinus.Size = new System.Drawing.Size(51, 50);
            this.btnMinus.TabIndex = 15;
            this.btnMinus.Text = "-";
            this.btnMinus.UseVisualStyleBackColor = false;
            this.btnMinus.Click += new System.EventHandler(this.btnOP_Click);
            // 
            // btnPlus
            // 
            this.btnPlus.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnPlus.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnPlus.FlatAppearance.BorderSize = 3;
            this.btnPlus.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPlus.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPlus.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnPlus.Location = new System.Drawing.Point(242, 247);
            this.btnPlus.Name = "btnPlus";
            this.btnPlus.Size = new System.Drawing.Size(51, 50);
            this.btnPlus.TabIndex = 14;
            this.btnPlus.Text = "+";
            this.btnPlus.UseVisualStyleBackColor = false;
            this.btnPlus.Click += new System.EventHandler(this.btnOP_Click);
            // 
            // btnMultiply
            // 
            this.btnMultiply.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnMultiply.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnMultiply.FlatAppearance.BorderSize = 3;
            this.btnMultiply.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnMultiply.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnMultiply.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnMultiply.Location = new System.Drawing.Point(242, 135);
            this.btnMultiply.Name = "btnMultiply";
            this.btnMultiply.Size = new System.Drawing.Size(51, 50);
            this.btnMultiply.TabIndex = 16;
            this.btnMultiply.Tag = "";
            this.btnMultiply.Text = "×";
            this.btnMultiply.UseVisualStyleBackColor = false;
            this.btnMultiply.Click += new System.EventHandler(this.btnOP_Click);
            // 
            // btnDivide
            // 
            this.btnDivide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDivide.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDivide.FlatAppearance.BorderSize = 3;
            this.btnDivide.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDivide.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDivide.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDivide.Location = new System.Drawing.Point(185, 135);
            this.btnDivide.Name = "btnDivide";
            this.btnDivide.Size = new System.Drawing.Size(51, 50);
            this.btnDivide.TabIndex = 16;
            this.btnDivide.Tag = "";
            this.btnDivide.Text = "÷";
            this.btnDivide.UseVisualStyleBackColor = false;
            this.btnDivide.Click += new System.EventHandler(this.btnOP_Click);
            // 
            // btnBracketClose
            // 
            this.btnBracketClose.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnBracketClose.Enabled = false;
            this.btnBracketClose.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnBracketClose.FlatAppearance.BorderSize = 3;
            this.btnBracketClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBracketClose.Font = new System.Drawing.Font("Noto Sans Math", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnBracketClose.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnBracketClose.Location = new System.Drawing.Point(128, 135);
            this.btnBracketClose.Name = "btnBracketClose";
            this.btnBracketClose.Size = new System.Drawing.Size(51, 50);
            this.btnBracketClose.TabIndex = 16;
            this.btnBracketClose.Text = ")";
            this.btnBracketClose.UseVisualStyleBackColor = false;
            this.btnBracketClose.Click += new System.EventHandler(this.btnBracketClose_Click);
            // 
            // btnBracketOpen
            // 
            this.btnBracketOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnBracketOpen.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnBracketOpen.FlatAppearance.BorderSize = 3;
            this.btnBracketOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBracketOpen.Font = new System.Drawing.Font("Noto Sans Math", 16F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnBracketOpen.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnBracketOpen.Location = new System.Drawing.Point(71, 135);
            this.btnBracketOpen.Name = "btnBracketOpen";
            this.btnBracketOpen.Size = new System.Drawing.Size(51, 50);
            this.btnBracketOpen.TabIndex = 16;
            this.btnBracketOpen.Text = "(";
            this.btnBracketOpen.UseVisualStyleBackColor = false;
            this.btnBracketOpen.Click += new System.EventHandler(this.btnBracketOpen_Click);
            // 
            // btnDeg
            // 
            this.btnDeg.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnDeg.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnDeg.FlatAppearance.BorderSize = 3;
            this.btnDeg.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeg.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnDeg.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnDeg.Location = new System.Drawing.Point(13, 303);
            this.btnDeg.Name = "btnDeg";
            this.btnDeg.Size = new System.Drawing.Size(51, 50);
            this.btnDeg.TabIndex = 6;
            this.btnDeg.Tag = "";
            this.btnDeg.Text = "°";
            this.btnDeg.UseVisualStyleBackColor = false;
            this.btnDeg.Click += new System.EventHandler(this.btnFunction_Click);
            // 
            // btnPower
            // 
            this.btnPower.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnPower.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnPower.FlatAppearance.BorderSize = 3;
            this.btnPower.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnPower.Font = new System.Drawing.Font("Noto Sans Math", 16F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnPower.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnPower.Location = new System.Drawing.Point(13, 247);
            this.btnPower.Name = "btnPower";
            this.btnPower.Size = new System.Drawing.Size(51, 50);
            this.btnPower.TabIndex = 9;
            this.btnPower.Text = "^";
            this.btnPower.UseVisualStyleBackColor = false;
            this.btnPower.Click += new System.EventHandler(this.btnOP_Click);
            // 
            // btnExtraFunction
            // 
            this.btnExtraFunction.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnExtraFunction.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnExtraFunction.FlatAppearance.BorderSize = 3;
            this.btnExtraFunction.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnExtraFunction.Font = new System.Drawing.Font("Noto Sans Math", 14F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnExtraFunction.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnExtraFunction.Location = new System.Drawing.Point(13, 191);
            this.btnExtraFunction.Name = "btnExtraFunction";
            this.btnExtraFunction.Size = new System.Drawing.Size(51, 50);
            this.btnExtraFunction.TabIndex = 12;
            this.btnExtraFunction.Text = "ƒx";
            this.btnExtraFunction.UseVisualStyleBackColor = false;
            this.btnExtraFunction.Click += new System.EventHandler(this.btnExtraFunction_Click);
            // 
            // btnSqrt
            // 
            this.btnSqrt.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnSqrt.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnSqrt.FlatAppearance.BorderSize = 3;
            this.btnSqrt.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnSqrt.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnSqrt.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnSqrt.Location = new System.Drawing.Point(14, 135);
            this.btnSqrt.Name = "btnSqrt";
            this.btnSqrt.Size = new System.Drawing.Size(51, 50);
            this.btnSqrt.TabIndex = 16;
            this.btnSqrt.Tag = "";
            this.btnSqrt.Text = "√";
            this.btnSqrt.UseVisualStyleBackColor = false;
            this.btnSqrt.Click += new System.EventHandler(this.btnFunction_Click);
            // 
            // functionBar1
            // 
            this.functionBar1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(29)))), ((int)(((byte)(29)))), ((int)(((byte)(29)))));
            this.functionBar1.Location = new System.Drawing.Point(30, 77);
            this.functionBar1.Name = "functionBar1";
            this.functionBar1.Size = new System.Drawing.Size(174, 114);
            this.functionBar1.TabIndex = 0;
            this.functionBar1.TabStop = false;
            this.functionBar1.Visible = false;
            // 
            // btnBackspace
            // 
            this.btnBackspace.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnBackspace.Enabled = false;
            this.btnBackspace.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnBackspace.FlatAppearance.BorderSize = 3;
            this.btnBackspace.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnBackspace.Font = new System.Drawing.Font("Noto Sans Math", 12F, ((System.Drawing.FontStyle)((System.Drawing.FontStyle.Bold | System.Drawing.FontStyle.Italic))), System.Drawing.GraphicsUnit.Point);
            this.btnBackspace.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnBackspace.Location = new System.Drawing.Point(242, 79);
            this.btnBackspace.Name = "btnBackspace";
            this.btnBackspace.Size = new System.Drawing.Size(51, 50);
            this.btnBackspace.TabIndex = 16;
            this.btnBackspace.Text = "del";
            this.btnBackspace.UseVisualStyleBackColor = false;
            this.btnBackspace.Click += new System.EventHandler(this.btnBackSpace_Click);
            // 
            // btnClear
            // 
            this.btnClear.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnClear.Enabled = false;
            this.btnClear.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnClear.FlatAppearance.BorderSize = 3;
            this.btnClear.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnClear.Font = new System.Drawing.Font("Noto Sans Math", 20F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.btnClear.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnClear.Location = new System.Drawing.Point(185, 79);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(51, 50);
            this.btnClear.TabIndex = 16;
            this.btnClear.Text = "C";
            this.btnClear.UseVisualStyleBackColor = false;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnCeil
            // 
            this.btnCeil.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnCeil.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnCeil.FlatAppearance.BorderSize = 3;
            this.btnCeil.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCeil.Font = new System.Drawing.Font("Noto Sans Math", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnCeil.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnCeil.Location = new System.Drawing.Point(128, 79);
            this.btnCeil.Name = "btnCeil";
            this.btnCeil.Size = new System.Drawing.Size(51, 50);
            this.btnCeil.TabIndex = 16;
            this.btnCeil.Tag = "";
            this.btnCeil.Text = "⌈x⌉";
            this.btnCeil.UseVisualStyleBackColor = false;
            this.btnCeil.Click += new System.EventHandler(this.btnFunction_Click);
            // 
            // btnAbs
            // 
            this.btnAbs.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnAbs.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnAbs.FlatAppearance.BorderSize = 3;
            this.btnAbs.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnAbs.Font = new System.Drawing.Font("Noto Sans Math", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnAbs.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnAbs.Location = new System.Drawing.Point(71, 79);
            this.btnAbs.Name = "btnAbs";
            this.btnAbs.Size = new System.Drawing.Size(51, 50);
            this.btnAbs.TabIndex = 16;
            this.btnAbs.Tag = "";
            this.btnAbs.Text = "|x|";
            this.btnAbs.UseVisualStyleBackColor = false;
            this.btnAbs.Click += new System.EventHandler(this.btnFunction_Click);
            // 
            // btnFloor
            // 
            this.btnFloor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(81)))), ((int)(((byte)(6)))), ((int)(((byte)(0)))));
            this.btnFloor.FlatAppearance.BorderColor = System.Drawing.Color.Gray;
            this.btnFloor.FlatAppearance.BorderSize = 3;
            this.btnFloor.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnFloor.Font = new System.Drawing.Font("Noto Sans Math", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point);
            this.btnFloor.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.btnFloor.Location = new System.Drawing.Point(14, 79);
            this.btnFloor.Name = "btnFloor";
            this.btnFloor.Size = new System.Drawing.Size(51, 50);
            this.btnFloor.TabIndex = 16;
            this.btnFloor.Tag = "";
            this.btnFloor.Text = "⌊x⌋";
            this.btnFloor.UseVisualStyleBackColor = false;
            this.btnFloor.Click += new System.EventHandler(this.btnFunction_Click);
            // 
            // txtExpression
            // 
            this.txtExpression.Font = new System.Drawing.Font("Segoe UI", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point);
            this.txtExpression.ForeColor = System.Drawing.SystemColors.InactiveCaption;
            this.txtExpression.Location = new System.Drawing.Point(13, 7);
            this.txtExpression.Name = "txtExpression";
            this.txtExpression.RightToLeft = System.Windows.Forms.RightToLeft.No;
            this.txtExpression.Size = new System.Drawing.Size(279, 19);
            this.txtExpression.TabIndex = 18;
            this.txtExpression.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(20)))), ((int)(((byte)(20)))), ((int)(((byte)(20)))));
            this.ClientSize = new System.Drawing.Size(305, 430);
            this.Controls.Add(this.functionBar1);
            this.Controls.Add(this.txtExpression);
            this.Controls.Add(this.btnFloor);
            this.Controls.Add(this.btnSqrt);
            this.Controls.Add(this.btnAbs);
            this.Controls.Add(this.btnBracketOpen);
            this.Controls.Add(this.btnCeil);
            this.Controls.Add(this.btnBracketClose);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnBackspace);
            this.Controls.Add(this.btnDivide);
            this.Controls.Add(this.btnMultiply);
            this.Controls.Add(this.btnMinus);
            this.Controls.Add(this.btnPlus);
            this.Controls.Add(this.btnDigit0);
            this.Controls.Add(this.btnExtraFunction);
            this.Controls.Add(this.btnDigit7);
            this.Controls.Add(this.btnDigit8);
            this.Controls.Add(this.btnDigit9);
            this.Controls.Add(this.btnPower);
            this.Controls.Add(this.btnDigit4);
            this.Controls.Add(this.btnDigit5);
            this.Controls.Add(this.btnDigit6);
            this.Controls.Add(this.btnDeg);
            this.Controls.Add(this.btnDigit1);
            this.Controls.Add(this.btnDigit2);
            this.Controls.Add(this.btnDigit3);
            this.Controls.Add(this.btnDot);
            this.Controls.Add(this.btnEquals);
            this.Controls.Add(this.txtNumber);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.KeyPreview = true;
            this.MaximizeBox = false;
            this.Name = "MainForm";
            this.Text = "Calculator";
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.MainForm_KeyPress);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private TextBox txtNumber;
        private Button btnEquals;
        private Button btnDot;
        private Button btnDigit3;
        private Button btnDigit2;
        private Button btnDigit1;
        private Button btnDigit4;
        private Button btnDigit5;
        private Button btnDigit6;
        private Button btnDigit7;
        private Button btnDigit8;
        private Button btnDigit9;
        private Button btnDigit0;
        private Button btnMinus;
        private Button btnPlus;
        private Button btnMultiply;
        private Button btnDivide;
        private Button btnBracketClose;
        private Button btnBracketOpen;
        private Button btnDeg;
        private Button btnPower;
        private Button btnExtraFunction;
        private Button btnSqrt;
        private FunctionBar functionBar1;
        private Button btnBackspace;
        private Button btnClear;
        private Button btnCeil;
        private Button btnAbs;
        private Button btnFloor;
        private Label txtExpression;
    }
}