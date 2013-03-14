namespace GeneralTests
{
    partial class MainForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
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
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
					this.textResults = new System.Windows.Forms.TextBox();
					this.comboMethods = new System.Windows.Forms.ComboBox();
					this.btRunTest = new System.Windows.Forms.Button();
					this.button1 = new System.Windows.Forms.Button();
					this.comboTests = new System.Windows.Forms.ComboBox();
					this.label1 = new System.Windows.Forms.Label();
					this.label2 = new System.Windows.Forms.Label();
					this.btResetTest = new System.Windows.Forms.Button();
					this.lbRes = new System.Windows.Forms.Label();
					this.SuspendLayout();
					// 
					// textResults
					// 
					this.textResults.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
											| System.Windows.Forms.AnchorStyles.Left)
											| System.Windows.Forms.AnchorStyles.Right)));
					this.textResults.Location = new System.Drawing.Point(12, 103);
					this.textResults.Multiline = true;
					this.textResults.Name = "textResults";
					this.textResults.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
					this.textResults.Size = new System.Drawing.Size(466, 305);
					this.textResults.TabIndex = 0;
					// 
					// comboMethods
					// 
					this.comboMethods.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
					this.comboMethods.Enabled = false;
					this.comboMethods.FormattingEnabled = true;
					this.comboMethods.Location = new System.Drawing.Point(116, 42);
					this.comboMethods.Name = "comboMethods";
					this.comboMethods.Size = new System.Drawing.Size(281, 24);
					this.comboMethods.TabIndex = 1;
					this.comboMethods.SelectedIndexChanged += new System.EventHandler(this.comboMethods_SelectedIndexChanged);
					// 
					// btRunTest
					// 
					this.btRunTest.Enabled = false;
					this.btRunTest.Location = new System.Drawing.Point(403, 42);
					this.btRunTest.Name = "btRunTest";
					this.btRunTest.Size = new System.Drawing.Size(75, 24);
					this.btRunTest.TabIndex = 2;
					this.btRunTest.Text = "Run";
					this.btRunTest.UseVisualStyleBackColor = true;
					this.btRunTest.Click += new System.EventHandler(this.btRunTest_Click);
					// 
					// button1
					// 
					this.button1.Location = new System.Drawing.Point(403, 72);
					this.button1.Name = "button1";
					this.button1.Size = new System.Drawing.Size(75, 25);
					this.button1.TabIndex = 3;
					this.button1.Text = "Clear";
					this.button1.UseVisualStyleBackColor = true;
					this.button1.Click += new System.EventHandler(this.button1_Click);
					// 
					// comboTests
					// 
					this.comboTests.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
					this.comboTests.FormattingEnabled = true;
					this.comboTests.Location = new System.Drawing.Point(116, 12);
					this.comboTests.Name = "comboTests";
					this.comboTests.Size = new System.Drawing.Size(281, 24);
					this.comboTests.TabIndex = 4;
					this.comboTests.SelectedIndexChanged += new System.EventHandler(this.comboTests_SelectedIndexChanged);
					// 
					// label1
					// 
					this.label1.AutoSize = true;
					this.label1.Location = new System.Drawing.Point(12, 19);
					this.label1.Name = "label1";
					this.label1.Size = new System.Drawing.Size(36, 17);
					this.label1.TabIndex = 5;
					this.label1.Text = "Test";
					// 
					// label2
					// 
					this.label2.AutoSize = true;
					this.label2.Location = new System.Drawing.Point(12, 49);
					this.label2.Name = "label2";
					this.label2.Size = new System.Drawing.Size(55, 17);
					this.label2.TabIndex = 6;
					this.label2.Text = "Method";
					// 
					// btResetTest
					// 
					this.btResetTest.Enabled = false;
					this.btResetTest.Location = new System.Drawing.Point(403, 11);
					this.btResetTest.Name = "btResetTest";
					this.btResetTest.Size = new System.Drawing.Size(75, 24);
					this.btResetTest.TabIndex = 7;
					this.btResetTest.Text = "Reset";
					this.btResetTest.UseVisualStyleBackColor = true;
					this.btResetTest.Click += new System.EventHandler(this.btResetTest_Click);
					// 
					// lbRes
					// 
					this.lbRes.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
											| System.Windows.Forms.AnchorStyles.Right)));
					this.lbRes.Location = new System.Drawing.Point(14, 421);
					this.lbRes.Name = "lbRes";
					this.lbRes.Size = new System.Drawing.Size(464, 25);
					this.lbRes.TabIndex = 8;
					this.lbRes.Text = "-";
					this.lbRes.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
					// 
					// MainForm
					// 
					this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
					this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
					this.ClientSize = new System.Drawing.Size(490, 457);
					this.Controls.Add(this.lbRes);
					this.Controls.Add(this.btResetTest);
					this.Controls.Add(this.label2);
					this.Controls.Add(this.label1);
					this.Controls.Add(this.comboTests);
					this.Controls.Add(this.button1);
					this.Controls.Add(this.btRunTest);
					this.Controls.Add(this.comboMethods);
					this.Controls.Add(this.textResults);
					this.Name = "MainForm";
					this.Text = "Form1";
					this.ResumeLayout(false);
					this.PerformLayout();

        }

        #endregion

				private System.Windows.Forms.TextBox textResults;
				private System.Windows.Forms.ComboBox comboMethods;
				private System.Windows.Forms.Button btRunTest;
				private System.Windows.Forms.Button button1;
				private System.Windows.Forms.ComboBox comboTests;
				private System.Windows.Forms.Label label1;
				private System.Windows.Forms.Label label2;
				private System.Windows.Forms.Button btResetTest;
				private System.Windows.Forms.Label lbRes;
    }
}

