namespace FC.Craswler
{
    partial class MainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
            this.btn_LeagueInfo = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btn_LeagueInfo
            // 
            this.btn_LeagueInfo.BackColor = System.Drawing.Color.Aquamarine;
            this.btn_LeagueInfo.Font = new System.Drawing.Font("宋体", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btn_LeagueInfo.ForeColor = System.Drawing.Color.Blue;
            this.btn_LeagueInfo.Location = new System.Drawing.Point(12, 24);
            this.btn_LeagueInfo.Name = "btn_LeagueInfo";
            this.btn_LeagueInfo.Size = new System.Drawing.Size(119, 26);
            this.btn_LeagueInfo.TabIndex = 0;
            this.btn_LeagueInfo.Text = "联赛信息采集";
            this.btn_LeagueInfo.UseVisualStyleBackColor = false;
            this.btn_LeagueInfo.Click += new System.EventHandler(this.btn_LeagueInfo_Click);
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1557, 768);
            this.Controls.Add(this.btn_LeagueInfo);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "MainForm";
            this.Text = "数据采集程序";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btn_LeagueInfo;
    }
}

