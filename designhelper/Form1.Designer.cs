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
            this.kdaButton = new System.Windows.Forms.Button();
            this.firstTowerButton = new System.Windows.Forms.Button();
            this.firstBloodButton = new System.Windows.Forms.Button();
            this.killParticipationButton = new System.Windows.Forms.Button();
            this.baronKillsButton = new System.Windows.Forms.Button();
            this.dragonKillsButton = new System.Windows.Forms.Button();
            this.baronAndBaronKillsButton = new System.Windows.Forms.Button();
            this.creepsButton = new System.Windows.Forms.Button();
            this.wardsSupportButton = new System.Windows.Forms.Button();
            this.structureDmgButton = new System.Windows.Forms.Button();
            this.minionsPrMinAdcButton = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // getMatchesButton
            // 
            this.getMatchesButton.Location = new System.Drawing.Point(13, 795);
            this.getMatchesButton.Name = "getMatchesButton";
            this.getMatchesButton.Size = new System.Drawing.Size(200, 51);
            this.getMatchesButton.TabIndex = 0;
            this.getMatchesButton.Text = "Få ~1000 kampe!";
            this.getMatchesButton.UseVisualStyleBackColor = true;
            this.getMatchesButton.Click += new System.EventHandler(this.getMatches_Click);
            // 
            // summonerIdInput
            // 
            this.summonerIdInput.Location = new System.Drawing.Point(13, 763);
            this.summonerIdInput.Name = "summonerIdInput";
            this.summonerIdInput.Size = new System.Drawing.Size(198, 26);
            this.summonerIdInput.TabIndex = 1;
            // 
            // fillDbWithMatchInfo
            // 
            this.fillDbWithMatchInfo.Location = new System.Drawing.Point(13, 795);
            this.fillDbWithMatchInfo.Margin = new System.Windows.Forms.Padding(4, 5, 4, 5);
            this.fillDbWithMatchInfo.Name = "fillDbWithMatchInfo";
            this.fillDbWithMatchInfo.Size = new System.Drawing.Size(200, 49);
            this.fillDbWithMatchInfo.TabIndex = 2;
            this.fillDbWithMatchInfo.Text = "Fyld db med kampinfo";
            this.fillDbWithMatchInfo.UseVisualStyleBackColor = true;
            this.fillDbWithMatchInfo.Visible = false;
            this.fillDbWithMatchInfo.Click += new System.EventHandler(this.fillDbWithMatchInfo_Click);
            // 
            // kdaButton
            // 
            this.kdaButton.Location = new System.Drawing.Point(43, 101);
            this.kdaButton.Name = "kdaButton";
            this.kdaButton.Size = new System.Drawing.Size(148, 49);
            this.kdaButton.TabIndex = 3;
            this.kdaButton.Text = "KDA";
            this.kdaButton.UseVisualStyleBackColor = true;
            this.kdaButton.Click += new System.EventHandler(this.kdaButton_Click);
            // 
            // firstTowerButton
            // 
            this.firstTowerButton.Location = new System.Drawing.Point(659, 46);
            this.firstTowerButton.Name = "firstTowerButton";
            this.firstTowerButton.Size = new System.Drawing.Size(148, 49);
            this.firstTowerButton.TabIndex = 4;
            this.firstTowerButton.Text = "First Tower";
            this.firstTowerButton.UseVisualStyleBackColor = true;
            this.firstTowerButton.Click += new System.EventHandler(this.firstTowerButton_Click);
            // 
            // firstBloodButton
            // 
            this.firstBloodButton.Location = new System.Drawing.Point(505, 46);
            this.firstBloodButton.Name = "firstBloodButton";
            this.firstBloodButton.Size = new System.Drawing.Size(148, 49);
            this.firstBloodButton.TabIndex = 5;
            this.firstBloodButton.Text = "First Blood";
            this.firstBloodButton.UseVisualStyleBackColor = true;
            this.firstBloodButton.Click += new System.EventHandler(this.firstBloodButton_Click);
            // 
            // killParticipationButton
            // 
            this.killParticipationButton.Location = new System.Drawing.Point(197, 101);
            this.killParticipationButton.Name = "killParticipationButton";
            this.killParticipationButton.Size = new System.Drawing.Size(148, 49);
            this.killParticipationButton.TabIndex = 6;
            this.killParticipationButton.Text = "Kill Participation";
            this.killParticipationButton.UseVisualStyleBackColor = true;
            this.killParticipationButton.Click += new System.EventHandler(this.killParticipationButton_Click);
            // 
            // baronKillsButton
            // 
            this.baronKillsButton.Location = new System.Drawing.Point(43, 46);
            this.baronKillsButton.Name = "baronKillsButton";
            this.baronKillsButton.Size = new System.Drawing.Size(148, 49);
            this.baronKillsButton.TabIndex = 7;
            this.baronKillsButton.Text = "Baron Kills";
            this.baronKillsButton.UseVisualStyleBackColor = true;
            this.baronKillsButton.Click += new System.EventHandler(this.baronKillsButton_Click);
            // 
            // dragonKillsButton
            // 
            this.dragonKillsButton.Location = new System.Drawing.Point(197, 46);
            this.dragonKillsButton.Name = "dragonKillsButton";
            this.dragonKillsButton.Size = new System.Drawing.Size(148, 49);
            this.dragonKillsButton.TabIndex = 8;
            this.dragonKillsButton.Text = "Dragon Kills";
            this.dragonKillsButton.UseVisualStyleBackColor = true;
            this.dragonKillsButton.Click += new System.EventHandler(this.dragonKillsButton_Click);
            // 
            // baronAndBaronKillsButton
            // 
            this.baronAndBaronKillsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 6F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.baronAndBaronKillsButton.Location = new System.Drawing.Point(351, 46);
            this.baronAndBaronKillsButton.Name = "baronAndBaronKillsButton";
            this.baronAndBaronKillsButton.Size = new System.Drawing.Size(148, 49);
            this.baronAndBaronKillsButton.TabIndex = 9;
            this.baronAndBaronKillsButton.Text = "Baron and Dragon Kills";
            this.baronAndBaronKillsButton.UseVisualStyleBackColor = true;
            this.baronAndBaronKillsButton.Click += new System.EventHandler(this.baronAndBaronKillsButton_Click);
            // 
            // creepsButton
            // 
            this.creepsButton.Location = new System.Drawing.Point(1375, 12);
            this.creepsButton.Name = "creepsButton";
            this.creepsButton.Size = new System.Drawing.Size(148, 49);
            this.creepsButton.TabIndex = 10;
            this.creepsButton.Text = "Creeps";
            this.creepsButton.UseVisualStyleBackColor = true;
            this.creepsButton.Click += new System.EventHandler(this.creepsButton_Click);
            // 
            // wardsSupportButton
            // 
            this.wardsSupportButton.Location = new System.Drawing.Point(43, 453);
            this.wardsSupportButton.Name = "wardsSupportButton";
            this.wardsSupportButton.Size = new System.Drawing.Size(148, 49);
            this.wardsSupportButton.TabIndex = 11;
            this.wardsSupportButton.Text = "Wards Support";
            this.wardsSupportButton.UseVisualStyleBackColor = true;
            this.wardsSupportButton.Click += new System.EventHandler(this.wardsSupportButton_Click);
            // 
            // structureDmgButton
            // 
            this.structureDmgButton.Location = new System.Drawing.Point(1385, 809);
            this.structureDmgButton.Name = "structureDmgButton";
            this.structureDmgButton.Size = new System.Drawing.Size(148, 49);
            this.structureDmgButton.TabIndex = 12;
            this.structureDmgButton.Text = "Structure Dmg";
            this.structureDmgButton.UseVisualStyleBackColor = true;
            this.structureDmgButton.Click += new System.EventHandler(this.structureDmgButton_Click);
            // 
            // minionsPrMinAdcButton
            // 
            this.minionsPrMinAdcButton.Location = new System.Drawing.Point(43, 218);
            this.minionsPrMinAdcButton.Name = "minionsPrMinAdcButton";
            this.minionsPrMinAdcButton.Size = new System.Drawing.Size(148, 49);
            this.minionsPrMinAdcButton.TabIndex = 13;
            this.minionsPrMinAdcButton.Text = "Minions pr min";
            this.minionsPrMinAdcButton.UseVisualStyleBackColor = true;
            this.minionsPrMinAdcButton.Click += new System.EventHandler(this.minionsPrMinAdcButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1535, 858);
            this.Controls.Add(this.minionsPrMinAdcButton);
            this.Controls.Add(this.structureDmgButton);
            this.Controls.Add(this.wardsSupportButton);
            this.Controls.Add(this.creepsButton);
            this.Controls.Add(this.baronAndBaronKillsButton);
            this.Controls.Add(this.dragonKillsButton);
            this.Controls.Add(this.baronKillsButton);
            this.Controls.Add(this.killParticipationButton);
            this.Controls.Add(this.firstBloodButton);
            this.Controls.Add(this.firstTowerButton);
            this.Controls.Add(this.kdaButton);
            this.Controls.Add(this.fillDbWithMatchInfo);
            this.Controls.Add(this.summonerIdInput);
            this.Controls.Add(this.getMatchesButton);
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
        private System.Windows.Forms.Button kdaButton;
        private System.Windows.Forms.Button firstTowerButton;
        private System.Windows.Forms.Button firstBloodButton;
        private System.Windows.Forms.Button killParticipationButton;
        private System.Windows.Forms.Button baronKillsButton;
        private System.Windows.Forms.Button dragonKillsButton;
        private System.Windows.Forms.Button baronAndBaronKillsButton;
        private System.Windows.Forms.Button creepsButton;
        private System.Windows.Forms.Button wardsSupportButton;
        private System.Windows.Forms.Button structureDmgButton;
        private System.Windows.Forms.Button minionsPrMinAdcButton;
    }
}

