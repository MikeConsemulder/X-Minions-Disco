namespace XminionsHeadSetColorDisco
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.RemoveGameButton = new System.Windows.Forms.Button();
            this.heartbeatTimer = new System.Windows.Forms.Timer(this.components);
            this.label1 = new System.Windows.Forms.Label();
            this.checkServer = new System.Windows.Forms.Timer(this.components);
            this.SuspendLayout();
            // 
            // RemoveGameButton
            // 
            this.RemoveGameButton.BackColor = System.Drawing.SystemColors.ActiveBorder;
            this.RemoveGameButton.Location = new System.Drawing.Point(143, 177);
            this.RemoveGameButton.Name = "RemoveGameButton";
            this.RemoveGameButton.Size = new System.Drawing.Size(148, 23);
            this.RemoveGameButton.TabIndex = 0;
            this.RemoveGameButton.Text = "Remove from gamesence";
            this.RemoveGameButton.UseVisualStyleBackColor = false;
            this.RemoveGameButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // heartbeatTimer
            // 
            this.heartbeatTimer.Enabled = true;
            this.heartbeatTimer.Interval = 14000;
            this.heartbeatTimer.Tick += new System.EventHandler(this.heartbeatTimer_Tick);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(9, 89);
            this.label1.Margin = new System.Windows.Forms.Padding(2, 0, 2, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(273, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Use shift+F1 for Red, shift+F2 for green, shift+F3 for blue";
            // 
            // checkServer
            // 
            this.checkServer.Enabled = true;
            this.checkServer.Interval = 5000;
            //this.checkServer.Tick += new System.EventHandler(this.checkServer_Tick);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.ClientSize = new System.Drawing.Size(301, 210);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.RemoveGameButton);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Form1";
            this.Text = "X-Minions ColorDisco MC swagon";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button RemoveGameButton;
        private System.Windows.Forms.Timer heartbeatTimer;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Timer checkServer;
    }
}

