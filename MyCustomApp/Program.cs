using System;
using System.Drawing;
using System.Windows.Forms;
using MathLibrary;

namespace MyCustomApp
{
    public class ModernCalculatorForm : Form
    {
        private TextBox txtFirstNum;
        private Label lblOperator;
        private TextBox txtSecondNum;
        private Label lblResultTag;
        private TextBox txtResult;
        private TextBox currentInput;

        public ModernCalculatorForm()
        {
            this.Text = "Swinburne Math Calculator";
            this.Size = new Size(360, 500);
            this.StartPosition = FormStartPosition.CenterScreen;
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.BackColor = Color.FromArgb(240, 243, 246);

            InitializeComponents();
        }

        private void InitializeComponents()
        {
            Panel cardPanel = new Panel
            {
                Location = new Point(20, 20),
                Size = new Size(305, 110),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            txtFirstNum = new TextBox
            {
                Location = new Point(15, 15),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 12),
                TextAlign = HorizontalAlignment.Center
            };
            txtFirstNum.Enter += (s, e) => currentInput = txtFirstNum;

            lblOperator = new Label
            {
                Text = "+",
                Location = new Point(122, 16),
                Size = new Size(55, 28),
                Font = new Font("Segoe UI", 13, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleCenter,
                ForeColor = Color.FromArgb(0, 102, 204)
            };

            txtSecondNum = new TextBox
            {
                Location = new Point(185, 15),
                Size = new Size(100, 30),
                Font = new Font("Segoe UI", 12),
                TextAlign = HorizontalAlignment.Center
            };
            txtSecondNum.Enter += (s, e) => currentInput = txtSecondNum;

            lblResultTag = new Label
            {
                Text = "Result =",
                Location = new Point(15, 65),
                Size = new Size(70, 30),
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                TextAlign = ContentAlignment.MiddleLeft,
                ForeColor = Color.DimGray
            };

            txtResult = new TextBox
            {
                Location = new Point(90, 63),
                Size = new Size(195, 30),
                Font = new Font("Segoe UI", 12, FontStyle.Bold),
                ReadOnly = true,
                BackColor = Color.FromArgb(245, 247, 250),
                TextAlign = HorizontalAlignment.Center
            };

            cardPanel.Controls.Add(txtFirstNum);
            cardPanel.Controls.Add(lblOperator);
            cardPanel.Controls.Add(txtSecondNum);
            cardPanel.Controls.Add(lblResultTag);
            cardPanel.Controls.Add(txtResult);

            this.Controls.Add(cardPanel);
            currentInput = txtFirstNum;

            CreateKeypad();
        }

        private void CreateKeypad()
        {
            string[,] buttons = {
                { "1", "2", "3", "+", "-" },
                { "4", "5", "6", "*", "/" },
                { "7", "8", "9", "CLR", "=" }
            };

            int startX = 20;
            int startY = 150;
            int btnWidth = 55;
            int btnHeight = 80;
            int gap = 8;

            for (int row = 0; row < 3; row++)
            {
                for (int col = 0; col < 5; col++)
                {
                    string btnText = buttons[row, col];
                    Button btn = new Button
                    {
                        Text = btnText,
                        Size = new Size(btnWidth, btnHeight),
                        Location = new Point(startX + col * (btnWidth + gap), startY + row * (btnHeight + gap)),
                        Font = new Font("Segoe UI", 10, FontStyle.Bold),
                        BackColor = Color.White,
                        UseVisualStyleBackColor = true
                    };

                    if (btnText == "=")
                    {
                        btn.BackColor = Color.FromArgb(0, 122, 255);
                        btn.ForeColor = Color.White;
                        btn.FlatStyle = FlatStyle.Flat;
                    }
                    else if (btnText == "CLR")
                    {
                        btn.ForeColor = Color.DarkRed;
                    }

                    btn.Click += KeypadButton_Click;
                    this.Controls.Add(btn);
                }
            }
        }

        private void KeypadButton_Click(object sender, EventArgs e)
        {
            Button btn = sender as Button;
            string val = btn.Text;

            if (char.IsDigit(val[0]))
            {
                if (currentInput != null)
                    currentInput.Text += val;
            }
            else if (val == "+" || val == "-" || val == "*" || val == "/")
            {
                lblOperator.Text = val;
                currentInput = txtSecondNum;
                txtSecondNum.Focus();
            }
            else if (val == "CLR")
            {
                txtFirstNum.Clear();
                txtSecondNum.Clear();
                txtResult.Clear();
                lblOperator.Text = "+";
                currentInput = txtFirstNum;
                txtFirstNum.Focus();
            }
            else if (val == "=")
            {
                CalculateResult();
            }
        }

        private void CalculateResult()
        {
            if (double.TryParse(txtFirstNum.Text, out double num1) &&
                double.TryParse(txtSecondNum.Text, out double num2))
            {
                double res = 0;
                switch (lblOperator.Text)
                {
                    case "+": res = Calculator.Add(num1, num2); break;
                    case "-": res = Calculator.Subtract(num1, num2); break;
                    case "*": res = Calculator.Multiply(num1, num2); break;
                    case "/":
                        if (num2 == 0)
                        {
                            txtResult.Text = "Error (Div/0)";
                            return;
                        }
                        res = Calculator.Divide(num1, num2);
                        break;
                }
                txtResult.Text = res.ToString();
            }
            else
            {
                txtResult.Text = "Invalid Input";
            }
        }

        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ModernCalculatorForm());
        }
    }
}