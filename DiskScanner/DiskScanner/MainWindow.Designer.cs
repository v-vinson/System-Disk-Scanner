namespace DiskScanner
{
	partial class MainWindow
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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainWindow));
			this.txtScanningMsg = new System.Windows.Forms.TextBox();
			this.btnScan = new System.Windows.Forms.Button();
			this.mainTimer = new System.Windows.Forms.Timer(this.components);
			this.btnCompare = new System.Windows.Forms.Button();
			this.lbMsg = new System.Windows.Forms.Label();
			this.lbBackupData = new System.Windows.Forms.Label();
			this.btnSave = new System.Windows.Forms.Button();
			this.txtCompareMsg = new System.Windows.Forms.TextBox();
			this.label = new System.Windows.Forms.Label();
			this.txtDeltaVal = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.SuspendLayout();
			// 
			// txtScanningMsg
			// 
			this.txtScanningMsg.Location = new System.Drawing.Point(12, 25);
			this.txtScanningMsg.Multiline = true;
			this.txtScanningMsg.Name = "txtScanningMsg";
			this.txtScanningMsg.ReadOnly = true;
			this.txtScanningMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtScanningMsg.Size = new System.Drawing.Size(349, 98);
			this.txtScanningMsg.TabIndex = 5;
			// 
			// btnScan
			// 
			this.btnScan.Location = new System.Drawing.Point(367, 25);
			this.btnScan.Name = "btnScan";
			this.btnScan.Size = new System.Drawing.Size(93, 46);
			this.btnScan.TabIndex = 0;
			this.btnScan.Text = "开始扫描";
			this.btnScan.UseVisualStyleBackColor = true;
			this.btnScan.Click += new System.EventHandler(this.BtnScan_Click);
			// 
			// mainTimer
			// 
			this.mainTimer.Interval = 1000;
			this.mainTimer.Tick += new System.EventHandler(this.MainTimer_Tick);
			// 
			// btnCompare
			// 
			this.btnCompare.Location = new System.Drawing.Point(12, 253);
			this.btnCompare.Name = "btnCompare";
			this.btnCompare.Size = new System.Drawing.Size(448, 46);
			this.btnCompare.TabIndex = 1;
			this.btnCompare.Text = "开始比较";
			this.btnCompare.UseVisualStyleBackColor = true;
			this.btnCompare.Click += new System.EventHandler(this.BtnCompare_Click);
			// 
			// lbMsg
			// 
			this.lbMsg.AutoEllipsis = true;
			this.lbMsg.Location = new System.Drawing.Point(12, 5);
			this.lbMsg.Name = "lbMsg";
			this.lbMsg.Size = new System.Drawing.Size(448, 17);
			this.lbMsg.TabIndex = 3;
			this.lbMsg.Text = "等待扫描";
			// 
			// lbBackupData
			// 
			this.lbBackupData.AutoSize = true;
			this.lbBackupData.Location = new System.Drawing.Point(12, 129);
			this.lbBackupData.Name = "lbBackupData";
			this.lbBackupData.Size = new System.Drawing.Size(104, 17);
			this.lbBackupData.TabIndex = 0;
			this.lbBackupData.Text = "BackupDataMsg";
			// 
			// btnSave
			// 
			this.btnSave.Location = new System.Drawing.Point(367, 77);
			this.btnSave.Name = "btnSave";
			this.btnSave.Size = new System.Drawing.Size(93, 46);
			this.btnSave.TabIndex = 2;
			this.btnSave.Text = "保存数据";
			this.btnSave.UseVisualStyleBackColor = true;
			this.btnSave.Click += new System.EventHandler(this.BtnSave_Click);
			// 
			// txtCompareMsg
			// 
			this.txtCompareMsg.Location = new System.Drawing.Point(12, 149);
			this.txtCompareMsg.Multiline = true;
			this.txtCompareMsg.Name = "txtCompareMsg";
			this.txtCompareMsg.ReadOnly = true;
			this.txtCompareMsg.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.txtCompareMsg.Size = new System.Drawing.Size(448, 98);
			this.txtCompareMsg.TabIndex = 6;
			// 
			// label
			// 
			this.label.AutoSize = true;
			this.label.Location = new System.Drawing.Point(240, 129);
			this.label.Name = "label";
			this.label.Size = new System.Drawing.Size(124, 17);
			this.label.TabIndex = 7;
			this.label.Text = "大小差值 (单位：MB)";
			// 
			// txtDeltaVal
			// 
			this.txtDeltaVal.Font = new System.Drawing.Font("Microsoft YaHei UI", 6.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.txtDeltaVal.Location = new System.Drawing.Point(367, 127);
			this.txtDeltaVal.MaxLength = 8;
			this.txtDeltaVal.Name = "txtDeltaVal";
			this.txtDeltaVal.Size = new System.Drawing.Size(93, 19);
			this.txtDeltaVal.TabIndex = 8;
			this.txtDeltaVal.Text = "1024";
			this.txtDeltaVal.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(281, 302);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(179, 17);
			this.label1.TabIndex = 9;
			this.label1.Text = "V1.0.0（开源版）   Author: Vin";
			// 
			// MainWindow
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(472, 323);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.txtDeltaVal);
			this.Controls.Add(this.label);
			this.Controls.Add(this.txtCompareMsg);
			this.Controls.Add(this.lbBackupData);
			this.Controls.Add(this.btnSave);
			this.Controls.Add(this.lbMsg);
			this.Controls.Add(this.btnCompare);
			this.Controls.Add(this.btnScan);
			this.Controls.Add(this.txtScanningMsg);
			this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Margin = new System.Windows.Forms.Padding(3, 4, 3, 4);
			this.Name = "MainWindow";
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "DiskScanner";
			this.Load += new System.EventHandler(this.MainWindow_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.TextBox txtScanningMsg;
		private System.Windows.Forms.Button btnScan;
		private System.Windows.Forms.Timer mainTimer;
		private System.Windows.Forms.Button btnCompare;
		private System.Windows.Forms.Label lbMsg;
		private System.Windows.Forms.Label lbBackupData;
		private System.Windows.Forms.Button btnSave;
		private System.Windows.Forms.TextBox txtCompareMsg;
		private System.Windows.Forms.Label label;
		private System.Windows.Forms.TextBox txtDeltaVal;
		private System.Windows.Forms.Label label1;
	}
}

