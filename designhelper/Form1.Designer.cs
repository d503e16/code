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
            this.moreMinionsButton = new System.Windows.Forms.Button();
            this.dmgToChampsButton = new System.Windows.Forms.Button();
            this.assistsSupportButton = new System.Windows.Forms.Button();
            this.deathsSupportButton = new System.Windows.Forms.Button();
            this.structureDmgSupportButton = new System.Windows.Forms.Button();
            this.minionsPrMinTopButton = new System.Windows.Forms.Button();
            this.deathsTopButton = new System.Windows.Forms.Button();
            this.dmgDealtToChampionsTopButton = new System.Windows.Forms.Button();
            this.wardsTopButton = new System.Windows.Forms.Button();
            this.moreMinionsTopButton = new System.Windows.Forms.Button();
            this.assistsTopButton = new System.Windows.Forms.Button();
            this.minionsPrMinMidButton = new System.Windows.Forms.Button();
            this.enemyJungleMidButton = new System.Windows.Forms.Button();
            this.wardsMidButton = new System.Windows.Forms.Button();
            this.dmgToChampsMidButton = new System.Windows.Forms.Button();
            this.moreMinionsMidButton = new System.Windows.Forms.Button();
            this.monstersKilledJglButton = new System.Windows.Forms.Button();
            this.enemyJungleJglButton = new System.Windows.Forms.Button();
            this.wardsJglButton = new System.Windows.Forms.Button();
            this.kdaJglButton = new System.Windows.Forms.Button();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.textBox2 = new System.Windows.Forms.TextBox();
            this.textBox3 = new System.Windows.Forms.TextBox();
            this.textBox4 = new System.Windows.Forms.TextBox();
            this.textBox5 = new System.Windows.Forms.TextBox();
            this.textBox6 = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // getMatchesButton
            // 
            this.getMatchesButton.Location = new System.Drawing.Point(13, 977);
            this.getMatchesButton.Name = "getMatchesButton";
            this.getMatchesButton.Size = new System.Drawing.Size(200, 51);
            this.getMatchesButton.TabIndex = 0;
            this.getMatchesButton.Text = "Få ~1000 kampe!";
            this.getMatchesButton.UseVisualStyleBackColor = true;
            this.getMatchesButton.Click += new System.EventHandler(this.getMatches_Click);
            // 
            // summonerIdInput
            // 
            this.summonerIdInput.Location = new System.Drawing.Point(218, 979);
            this.summonerIdInput.Name = "summonerIdInput";
            this.summonerIdInput.Size = new System.Drawing.Size(198, 26);
            this.summonerIdInput.TabIndex = 1;
            // 
            // fillDbWithMatchInfo
            // 
            this.fillDbWithMatchInfo.Location = new System.Drawing.Point(11, 979);
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
            this.creepsButton.Location = new System.Drawing.Point(1268, 12);
            this.creepsButton.Name = "creepsButton";
            this.creepsButton.Size = new System.Drawing.Size(148, 49);
            this.creepsButton.TabIndex = 10;
            this.creepsButton.Text = "Creeps";
            this.creepsButton.UseVisualStyleBackColor = true;
            this.creepsButton.Click += new System.EventHandler(this.creepsButton_Click);
            // 
            // wardsSupportButton
            // 
            this.wardsSupportButton.Location = new System.Drawing.Point(43, 335);
            this.wardsSupportButton.Name = "wardsSupportButton";
            this.wardsSupportButton.Size = new System.Drawing.Size(148, 49);
            this.wardsSupportButton.TabIndex = 11;
            this.wardsSupportButton.Text = "Wards";
            this.wardsSupportButton.UseVisualStyleBackColor = true;
            this.wardsSupportButton.Click += new System.EventHandler(this.wardsSupportButton_Click);
            // 
            // structureDmgButton
            // 
            this.structureDmgButton.Location = new System.Drawing.Point(1268, 67);
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
            // moreMinionsButton
            // 
            this.moreMinionsButton.Location = new System.Drawing.Point(197, 218);
            this.moreMinionsButton.Name = "moreMinionsButton";
            this.moreMinionsButton.Size = new System.Drawing.Size(148, 49);
            this.moreMinionsButton.TabIndex = 14;
            this.moreMinionsButton.Text = "More minions";
            this.moreMinionsButton.UseVisualStyleBackColor = true;
            this.moreMinionsButton.Click += new System.EventHandler(this.moreMinionsButton_Click);
            // 
            // dmgToChampsButton
            // 
            this.dmgToChampsButton.Location = new System.Drawing.Point(351, 218);
            this.dmgToChampsButton.Name = "dmgToChampsButton";
            this.dmgToChampsButton.Size = new System.Drawing.Size(148, 49);
            this.dmgToChampsButton.TabIndex = 15;
            this.dmgToChampsButton.Text = "Dmg to Champs";
            this.dmgToChampsButton.UseVisualStyleBackColor = true;
            this.dmgToChampsButton.Click += new System.EventHandler(this.dmgToChampsButton_Click);
            // 
            // assistsSupportButton
            // 
            this.assistsSupportButton.Location = new System.Drawing.Point(197, 335);
            this.assistsSupportButton.Name = "assistsSupportButton";
            this.assistsSupportButton.Size = new System.Drawing.Size(148, 49);
            this.assistsSupportButton.TabIndex = 16;
            this.assistsSupportButton.Text = "Assists";
            this.assistsSupportButton.UseVisualStyleBackColor = true;
            this.assistsSupportButton.Click += new System.EventHandler(this.assistsSupportButton_Click);
            // 
            // deathsSupportButton
            // 
            this.deathsSupportButton.Location = new System.Drawing.Point(351, 335);
            this.deathsSupportButton.Name = "deathsSupportButton";
            this.deathsSupportButton.Size = new System.Drawing.Size(148, 49);
            this.deathsSupportButton.TabIndex = 17;
            this.deathsSupportButton.Text = "Deaths";
            this.deathsSupportButton.UseVisualStyleBackColor = true;
            this.deathsSupportButton.Click += new System.EventHandler(this.deathsSupportButton_Click);
            // 
            // structureDmgSupportButton
            // 
            this.structureDmgSupportButton.Location = new System.Drawing.Point(1268, 122);
            this.structureDmgSupportButton.Name = "structureDmgSupportButton";
            this.structureDmgSupportButton.Size = new System.Drawing.Size(148, 49);
            this.structureDmgSupportButton.TabIndex = 18;
            this.structureDmgSupportButton.Text = "Structure Dmg";
            this.structureDmgSupportButton.UseVisualStyleBackColor = true;
            // 
            // minionsPrMinTopButton
            // 
            this.minionsPrMinTopButton.Location = new System.Drawing.Point(43, 451);
            this.minionsPrMinTopButton.Name = "minionsPrMinTopButton";
            this.minionsPrMinTopButton.Size = new System.Drawing.Size(148, 49);
            this.minionsPrMinTopButton.TabIndex = 19;
            this.minionsPrMinTopButton.Text = "Minions pr min";
            this.minionsPrMinTopButton.UseVisualStyleBackColor = true;
            this.minionsPrMinTopButton.Click += new System.EventHandler(this.minionsPrMinTopButton_Click);
            // 
            // deathsTopButton
            // 
            this.deathsTopButton.Location = new System.Drawing.Point(659, 451);
            this.deathsTopButton.Name = "deathsTopButton";
            this.deathsTopButton.Size = new System.Drawing.Size(148, 49);
            this.deathsTopButton.TabIndex = 20;
            this.deathsTopButton.Text = "Deaths";
            this.deathsTopButton.UseVisualStyleBackColor = true;
            this.deathsTopButton.Click += new System.EventHandler(this.deathsSupportButton_Click);
            // 
            // dmgDealtToChampionsTopButton
            // 
            this.dmgDealtToChampionsTopButton.Location = new System.Drawing.Point(505, 451);
            this.dmgDealtToChampionsTopButton.Name = "dmgDealtToChampionsTopButton";
            this.dmgDealtToChampionsTopButton.Size = new System.Drawing.Size(148, 49);
            this.dmgDealtToChampionsTopButton.TabIndex = 21;
            this.dmgDealtToChampionsTopButton.Text = "Dmg to Champs";
            this.dmgDealtToChampionsTopButton.UseVisualStyleBackColor = true;
            this.dmgDealtToChampionsTopButton.Click += new System.EventHandler(this.dmgDealtToChampionsTopButton_Click);
            // 
            // wardsTopButton
            // 
            this.wardsTopButton.Location = new System.Drawing.Point(351, 451);
            this.wardsTopButton.Name = "wardsTopButton";
            this.wardsTopButton.Size = new System.Drawing.Size(148, 49);
            this.wardsTopButton.TabIndex = 22;
            this.wardsTopButton.Text = "Wards";
            this.wardsTopButton.UseVisualStyleBackColor = true;
            this.wardsTopButton.Click += new System.EventHandler(this.wardsTopButton_Click);
            // 
            // moreMinionsTopButton
            // 
            this.moreMinionsTopButton.Location = new System.Drawing.Point(197, 451);
            this.moreMinionsTopButton.Name = "moreMinionsTopButton";
            this.moreMinionsTopButton.Size = new System.Drawing.Size(148, 49);
            this.moreMinionsTopButton.TabIndex = 23;
            this.moreMinionsTopButton.Text = "More minions";
            this.moreMinionsTopButton.UseVisualStyleBackColor = true;
            this.moreMinionsTopButton.Click += new System.EventHandler(this.moreMinionsTopButton_Click);
            // 
            // assistsTopButton
            // 
            this.assistsTopButton.Location = new System.Drawing.Point(43, 506);
            this.assistsTopButton.Name = "assistsTopButton";
            this.assistsTopButton.Size = new System.Drawing.Size(148, 49);
            this.assistsTopButton.TabIndex = 24;
            this.assistsTopButton.Text = "Assists";
            this.assistsTopButton.UseVisualStyleBackColor = true;
            this.assistsTopButton.Click += new System.EventHandler(this.assistsTopButton_Click);
            // 
            // minionsPrMinMidButton
            // 
            this.minionsPrMinMidButton.Location = new System.Drawing.Point(43, 602);
            this.minionsPrMinMidButton.Name = "minionsPrMinMidButton";
            this.minionsPrMinMidButton.Size = new System.Drawing.Size(148, 49);
            this.minionsPrMinMidButton.TabIndex = 25;
            this.minionsPrMinMidButton.Text = "Minions pr min";
            this.minionsPrMinMidButton.UseVisualStyleBackColor = true;
            this.minionsPrMinMidButton.Click += new System.EventHandler(this.minionsPrMinMidButton_Click);
            // 
            // enemyJungleMidButton
            // 
            this.enemyJungleMidButton.Location = new System.Drawing.Point(659, 602);
            this.enemyJungleMidButton.Name = "enemyJungleMidButton";
            this.enemyJungleMidButton.Size = new System.Drawing.Size(148, 49);
            this.enemyJungleMidButton.TabIndex = 26;
            this.enemyJungleMidButton.Text = "Enemy Jungle";
            this.enemyJungleMidButton.UseVisualStyleBackColor = true;
            this.enemyJungleMidButton.Click += new System.EventHandler(this.enemyJungleMidButton_Click);
            // 
            // wardsMidButton
            // 
            this.wardsMidButton.Location = new System.Drawing.Point(505, 602);
            this.wardsMidButton.Name = "wardsMidButton";
            this.wardsMidButton.Size = new System.Drawing.Size(148, 49);
            this.wardsMidButton.TabIndex = 27;
            this.wardsMidButton.Text = "Wards";
            this.wardsMidButton.UseVisualStyleBackColor = true;
            this.wardsMidButton.Click += new System.EventHandler(this.wardsMidButton_Click);
            // 
            // dmgToChampsMidButton
            // 
            this.dmgToChampsMidButton.Location = new System.Drawing.Point(351, 602);
            this.dmgToChampsMidButton.Name = "dmgToChampsMidButton";
            this.dmgToChampsMidButton.Size = new System.Drawing.Size(148, 49);
            this.dmgToChampsMidButton.TabIndex = 28;
            this.dmgToChampsMidButton.Text = "Dmg to Champs";
            this.dmgToChampsMidButton.UseVisualStyleBackColor = true;
            this.dmgToChampsMidButton.Click += new System.EventHandler(this.dmgToChampsMidButton_Click);
            // 
            // moreMinionsMidButton
            // 
            this.moreMinionsMidButton.Location = new System.Drawing.Point(197, 602);
            this.moreMinionsMidButton.Name = "moreMinionsMidButton";
            this.moreMinionsMidButton.Size = new System.Drawing.Size(148, 49);
            this.moreMinionsMidButton.TabIndex = 29;
            this.moreMinionsMidButton.Text = "More Minions";
            this.moreMinionsMidButton.UseVisualStyleBackColor = true;
            this.moreMinionsMidButton.Click += new System.EventHandler(this.moreMinionsMidButton_Click);
            // 
            // monstersKilledJglButton
            // 
            this.monstersKilledJglButton.Location = new System.Drawing.Point(43, 713);
            this.monstersKilledJglButton.Name = "monstersKilledJglButton";
            this.monstersKilledJglButton.Size = new System.Drawing.Size(148, 49);
            this.monstersKilledJglButton.TabIndex = 30;
            this.monstersKilledJglButton.Text = "Monsters Killed";
            this.monstersKilledJglButton.UseVisualStyleBackColor = true;
            this.monstersKilledJglButton.Click += new System.EventHandler(this.monstersKilledJglButton_Click);
            // 
            // enemyJungleJglButton
            // 
            this.enemyJungleJglButton.Location = new System.Drawing.Point(197, 713);
            this.enemyJungleJglButton.Name = "enemyJungleJglButton";
            this.enemyJungleJglButton.Size = new System.Drawing.Size(148, 49);
            this.enemyJungleJglButton.TabIndex = 31;
            this.enemyJungleJglButton.Text = "Enemy Jungle";
            this.enemyJungleJglButton.UseVisualStyleBackColor = true;
            this.enemyJungleJglButton.Click += new System.EventHandler(this.enemyJungleJglButton_Click);
            // 
            // wardsJglButton
            // 
            this.wardsJglButton.Location = new System.Drawing.Point(351, 713);
            this.wardsJglButton.Name = "wardsJglButton";
            this.wardsJglButton.Size = new System.Drawing.Size(148, 49);
            this.wardsJglButton.TabIndex = 32;
            this.wardsJglButton.Text = "Wards";
            this.wardsJglButton.UseVisualStyleBackColor = true;
            this.wardsJglButton.Click += new System.EventHandler(this.wardsJglButton_Click);
            // 
            // kdaJglButton
            // 
            this.kdaJglButton.Location = new System.Drawing.Point(505, 713);
            this.kdaJglButton.Name = "kdaJglButton";
            this.kdaJglButton.Size = new System.Drawing.Size(148, 49);
            this.kdaJglButton.TabIndex = 33;
            this.kdaJglButton.Text = "KDA";
            this.kdaJglButton.UseVisualStyleBackColor = true;
            this.kdaJglButton.Click += new System.EventHandler(this.kdaJglButton_Click);
            // 
            // textBox1
            // 
            this.textBox1.Location = new System.Drawing.Point(43, 12);
            this.textBox1.Name = "textBox1";
            this.textBox1.ReadOnly = true;
            this.textBox1.Size = new System.Drawing.Size(100, 26);
            this.textBox1.TabIndex = 34;
            this.textBox1.Text = "Overall";
            // 
            // textBox2
            // 
            this.textBox2.Location = new System.Drawing.Point(43, 186);
            this.textBox2.Name = "textBox2";
            this.textBox2.ReadOnly = true;
            this.textBox2.Size = new System.Drawing.Size(100, 26);
            this.textBox2.TabIndex = 35;
            this.textBox2.Text = "ADC";
            // 
            // textBox3
            // 
            this.textBox3.Location = new System.Drawing.Point(43, 419);
            this.textBox3.Name = "textBox3";
            this.textBox3.ReadOnly = true;
            this.textBox3.Size = new System.Drawing.Size(100, 26);
            this.textBox3.TabIndex = 36;
            this.textBox3.Text = "Top";
            // 
            // textBox4
            // 
            this.textBox4.Location = new System.Drawing.Point(43, 570);
            this.textBox4.Name = "textBox4";
            this.textBox4.ReadOnly = true;
            this.textBox4.Size = new System.Drawing.Size(100, 26);
            this.textBox4.TabIndex = 37;
            this.textBox4.Text = "Mid";
            // 
            // textBox5
            // 
            this.textBox5.Location = new System.Drawing.Point(43, 681);
            this.textBox5.Name = "textBox5";
            this.textBox5.ReadOnly = true;
            this.textBox5.Size = new System.Drawing.Size(100, 26);
            this.textBox5.TabIndex = 38;
            this.textBox5.Text = "Jungle";
            // 
            // textBox6
            // 
            this.textBox6.Location = new System.Drawing.Point(43, 303);
            this.textBox6.Name = "textBox6";
            this.textBox6.ReadOnly = true;
            this.textBox6.Size = new System.Drawing.Size(100, 26);
            this.textBox6.TabIndex = 39;
            this.textBox6.Text = "Support";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(9F, 20F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1418, 1094);
            this.Controls.Add(this.textBox6);
            this.Controls.Add(this.textBox5);
            this.Controls.Add(this.textBox4);
            this.Controls.Add(this.textBox3);
            this.Controls.Add(this.textBox2);
            this.Controls.Add(this.textBox1);
            this.Controls.Add(this.kdaJglButton);
            this.Controls.Add(this.wardsJglButton);
            this.Controls.Add(this.enemyJungleJglButton);
            this.Controls.Add(this.monstersKilledJglButton);
            this.Controls.Add(this.moreMinionsMidButton);
            this.Controls.Add(this.dmgToChampsMidButton);
            this.Controls.Add(this.wardsMidButton);
            this.Controls.Add(this.enemyJungleMidButton);
            this.Controls.Add(this.minionsPrMinMidButton);
            this.Controls.Add(this.assistsTopButton);
            this.Controls.Add(this.moreMinionsTopButton);
            this.Controls.Add(this.wardsTopButton);
            this.Controls.Add(this.dmgDealtToChampionsTopButton);
            this.Controls.Add(this.deathsTopButton);
            this.Controls.Add(this.minionsPrMinTopButton);
            this.Controls.Add(this.structureDmgSupportButton);
            this.Controls.Add(this.deathsSupportButton);
            this.Controls.Add(this.assistsSupportButton);
            this.Controls.Add(this.dmgToChampsButton);
            this.Controls.Add(this.moreMinionsButton);
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
        private System.Windows.Forms.Button moreMinionsButton;
        private System.Windows.Forms.Button dmgToChampsButton;
        private System.Windows.Forms.Button assistsSupportButton;
        private System.Windows.Forms.Button deathsSupportButton;
        private System.Windows.Forms.Button structureDmgSupportButton;
        private System.Windows.Forms.Button minionsPrMinTopButton;
        private System.Windows.Forms.Button deathsTopButton;
        private System.Windows.Forms.Button dmgDealtToChampionsTopButton;
        private System.Windows.Forms.Button wardsTopButton;
        private System.Windows.Forms.Button moreMinionsTopButton;
        private System.Windows.Forms.Button assistsTopButton;
        private System.Windows.Forms.Button minionsPrMinMidButton;
        private System.Windows.Forms.Button enemyJungleMidButton;
        private System.Windows.Forms.Button wardsMidButton;
        private System.Windows.Forms.Button dmgToChampsMidButton;
        private System.Windows.Forms.Button moreMinionsMidButton;
        private System.Windows.Forms.Button monstersKilledJglButton;
        private System.Windows.Forms.Button enemyJungleJglButton;
        private System.Windows.Forms.Button wardsJglButton;
        private System.Windows.Forms.Button kdaJglButton;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.TextBox textBox2;
        private System.Windows.Forms.TextBox textBox3;
        private System.Windows.Forms.TextBox textBox4;
        private System.Windows.Forms.TextBox textBox5;
        private System.Windows.Forms.TextBox textBox6;
    }
}

