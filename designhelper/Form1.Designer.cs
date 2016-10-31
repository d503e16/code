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
            this.getMatchesButton = new System.Windows.Forms.Button();
            this.summonerIdInput = new System.Windows.Forms.TextBox();
            this.fillDbWithMatchInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // getMatchesButton
            // 
            this.getMatchesButton.Location = new System.Drawing.Point(166, 222);
            this.getMatchesButton.Margin = new System.Windows.Forms.Padding(2);
            this.getMatchesButton.Name = "getMatchesButton";
            this.getMatchesButton.Size = new System.Drawing.Size(133, 33);
            this.getMatchesButton.TabIndex = 0;
            this.getMatchesButton.Text = "Få ~1000 kampe!";
            this.getMatchesButton.UseVisualStyleBackColor = true;
            this.getMatchesButton.Click += new System.EventHandler(this.getMatches_Click);
            // 
            // summonerIdInput
            // 
            this.summonerIdInput.Location = new System.Drawing.Point(166, 198);
            this.summonerIdInput.Margin = new System.Windows.Forms.Padding(2);
            this.summonerIdInput.Name = "summonerIdInput";
            this.summonerIdInput.Size = new System.Drawing.Size(133, 20);
            this.summonerIdInput.TabIndex = 1;
            // 
            // fillDbWithMatchInfo
            // 
            this.fillDbWithMatchInfo.Location = new System.Drawing.Point(166, 223);
            this.fillDbWithMatchInfo.Name = "fillDbWithMatchInfo";
            this.fillDbWithMatchInfo.Size = new System.Drawing.Size(133, 32);
            this.fillDbWithMatchInfo.TabIndex = 2;
            this.fillDbWithMatchInfo.Text = "Fyld db med kampinfo";
            this.fillDbWithMatchInfo.UseVisualStyleBackColor = true;
            this.fillDbWithMatchInfo.Visible = false;
            this.fillDbWithMatchInfo.Click += new System.EventHandler(this.fillDbWithMatchInfo_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(461, 307);
            this.Controls.Add(this.fillDbWithMatchInfo);
            this.Controls.Add(this.summonerIdInput);
            this.Controls.Add(this.getMatchesButton);
            this.Margin = new System.Windows.Forms.Padding(2);
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getMatchesButton;
        private System.Windows.Forms.TextBox summonerIdInput;
        private System.Windows.Forms.Button fillDbWithMatchInfo;
    }
}

