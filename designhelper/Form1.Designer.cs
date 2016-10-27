namespace designhelper
{
    partial class Form1
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
            this.getSummonerButton = new System.Windows.Forms.Button();
            this.getSummonerText = new System.Windows.Forms.TextBox();
            this.listBox1 = new System.Windows.Forms.ListBox();
            this.SuspendLayout();
            // 
            // getSummonerButton
            // 
            this.getSummonerButton.Location = new System.Drawing.Point(39, 382);
            this.getSummonerButton.Name = "getSummonerButton";
            this.getSummonerButton.Size = new System.Drawing.Size(129, 51);
            this.getSummonerButton.TabIndex = 0;
            this.getSummonerButton.Text = "Få Summoner";
            this.getSummonerButton.UseVisualStyleBackColor = true;
            this.getSummonerButton.Click += new System.EventHandler(this.getSummonerButton_Click);
            // 
            // getSummonerText
            // 
            this.getSummonerText.Location = new System.Drawing.Point(39, 311);
            this.getSummonerText.Name = "getSummonerText";
            this.getSummonerText.Size = new System.Drawing.Size(225, 26);
            this.getSummonerText.TabIndex = 1;
            // 
            // listBox1
            // 
            this.listBox1.FormattingEnabled = true;
            this.listBox1.ItemHeight = 20;
            this.listBox1.Location = new System.Drawing.Point(362, 311);
            this.listBox1.Name = "listBox1";
            this.listBox1.Size = new System.Drawing.Size(317, 144);
            this.listBox1.TabIndex = 2;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(691, 472);
            this.Controls.Add(this.listBox1);
            this.Controls.Add(this.getSummonerText);
            this.Controls.Add(this.getSummonerButton);
            this.Name = "Form1";
            this.Text = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getSummonerButton;
        private System.Windows.Forms.TextBox getSummonerText;
        private System.Windows.Forms.ListBox listBox1;
    }
}

