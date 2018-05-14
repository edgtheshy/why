namespace 联机三子棋
{
    partial class Form1
    {
        /// <summary>
        /// 必需的设计器变量。
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// 清理所有正在使用的资源。
        /// </summary>
        /// <param name="disposing">如果应释放托管资源，为 true；否则为 false。</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows 窗体设计器生成的代码

        /// <summary>
        /// 设计器支持所需的方法 - 不要修改
        /// 使用代码编辑器修改此方法的内容。
        /// </summary>
        private void InitializeComponent()
        {
            this.btnConInternet = new System.Windows.Forms.Button();
            this.lblState = new System.Windows.Forms.Label();
            this.lblCanStart = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnConInternet
            // 
            this.btnConInternet.Location = new System.Drawing.Point(353, 50);
            this.btnConInternet.Name = "btnConInternet";
            this.btnConInternet.Size = new System.Drawing.Size(83, 34);
            this.btnConInternet.TabIndex = 0;
            this.btnConInternet.Text = "联机";
            this.btnConInternet.UseVisualStyleBackColor = true;
            this.btnConInternet.Click += new System.EventHandler(this.button1_Click);
            // 
            // lblState
            // 
            this.lblState.AutoSize = true;
            this.lblState.Location = new System.Drawing.Point(353, 109);
            this.lblState.Name = "lblState";
            this.lblState.Size = new System.Drawing.Size(52, 15);
            this.lblState.TabIndex = 1;
            this.lblState.Text = "未连接";
            // 
            // lblCanStart
            // 
            this.lblCanStart.AutoSize = true;
            this.lblCanStart.Location = new System.Drawing.Point(356, 152);
            this.lblCanStart.Name = "lblCanStart";
            this.lblCanStart.Size = new System.Drawing.Size(67, 15);
            this.lblCanStart.TabIndex = 2;
            this.lblCanStart.Text = "等待开始";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.lblCanStart);
            this.Controls.Add(this.lblState);
            this.Controls.Add(this.btnConInternet);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Form1";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.Form1_Paint);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Form1_MouseClick);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnConInternet;
        private System.Windows.Forms.Label lblState;
        private System.Windows.Forms.Label lblCanStart;
    }
}

