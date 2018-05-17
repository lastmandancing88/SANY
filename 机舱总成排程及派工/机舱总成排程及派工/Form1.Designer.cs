namespace 机舱总成排程及派工
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
            this.scMain = new System.Windows.Forms.SplitContainer();
            this.btnSend = new System.Windows.Forms.Button();
            this.bImport = new System.Windows.Forms.Button();
            this.bExport = new System.Windows.Forms.Button();
            this.bGenerate = new System.Windows.Forms.Button();
            this.tcMain = new System.Windows.Forms.TabControl();
            this.tpOutput = new System.Windows.Forms.TabPage();
            this.dgvOutput = new System.Windows.Forms.DataGridView();
            this.tpMasterData = new System.Windows.Forms.TabPage();
            this.dgvMasterData = new System.Windows.Forms.DataGridView();
            this.btnSendtoObserver = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).BeginInit();
            this.scMain.Panel1.SuspendLayout();
            this.scMain.Panel2.SuspendLayout();
            this.scMain.SuspendLayout();
            this.tcMain.SuspendLayout();
            this.tpOutput.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutput)).BeginInit();
            this.tpMasterData.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterData)).BeginInit();
            this.SuspendLayout();
            // 
            // scMain
            // 
            this.scMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.scMain.Location = new System.Drawing.Point(0, 0);
            this.scMain.Name = "scMain";
            this.scMain.Orientation = System.Windows.Forms.Orientation.Horizontal;
            // 
            // scMain.Panel1
            // 
            this.scMain.Panel1.Controls.Add(this.btnSendtoObserver);
            this.scMain.Panel1.Controls.Add(this.btnSend);
            this.scMain.Panel1.Controls.Add(this.bImport);
            this.scMain.Panel1.Controls.Add(this.bExport);
            this.scMain.Panel1.Controls.Add(this.bGenerate);
            this.scMain.Panel1MinSize = 60;
            // 
            // scMain.Panel2
            // 
            this.scMain.Panel2.Controls.Add(this.tcMain);
            this.scMain.Panel2MinSize = 497;
            this.scMain.Size = new System.Drawing.Size(784, 561);
            this.scMain.SplitterDistance = 60;
            this.scMain.TabIndex = 0;
            // 
            // btnSend
            // 
            this.btnSend.Location = new System.Drawing.Point(590, 15);
            this.btnSend.Name = "btnSend";
            this.btnSend.Size = new System.Drawing.Size(90, 30);
            this.btnSend.TabIndex = 10;
            this.btnSend.Text = "任务下发";
            this.btnSend.UseVisualStyleBackColor = true;
            this.btnSend.Click += new System.EventHandler(this.btnSend_Click);
            // 
            // bImport
            // 
            this.bImport.Location = new System.Drawing.Point(299, 15);
            this.bImport.Name = "bImport";
            this.bImport.Size = new System.Drawing.Size(90, 30);
            this.bImport.TabIndex = 9;
            this.bImport.Text = "导入主数据";
            this.bImport.UseVisualStyleBackColor = true;
            this.bImport.Click += new System.EventHandler(this.bImport_Click);
            // 
            // bExport
            // 
            this.bExport.Location = new System.Drawing.Point(493, 15);
            this.bExport.Name = "bExport";
            this.bExport.Size = new System.Drawing.Size(90, 30);
            this.bExport.TabIndex = 7;
            this.bExport.Text = "导出报表";
            this.bExport.UseVisualStyleBackColor = true;
            this.bExport.Click += new System.EventHandler(this.bExport_Click);
            // 
            // bGenerate
            // 
            this.bGenerate.Location = new System.Drawing.Point(396, 15);
            this.bGenerate.Name = "bGenerate";
            this.bGenerate.Size = new System.Drawing.Size(90, 30);
            this.bGenerate.TabIndex = 6;
            this.bGenerate.Text = "生成报表";
            this.bGenerate.UseVisualStyleBackColor = true;
            this.bGenerate.Click += new System.EventHandler(this.bGenerate_Click);
            // 
            // tcMain
            // 
            this.tcMain.Controls.Add(this.tpOutput);
            this.tcMain.Controls.Add(this.tpMasterData);
            this.tcMain.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tcMain.Location = new System.Drawing.Point(0, 0);
            this.tcMain.Name = "tcMain";
            this.tcMain.SelectedIndex = 0;
            this.tcMain.Size = new System.Drawing.Size(784, 497);
            this.tcMain.TabIndex = 0;
            // 
            // tpOutput
            // 
            this.tpOutput.Controls.Add(this.dgvOutput);
            this.tpOutput.Location = new System.Drawing.Point(4, 26);
            this.tpOutput.Name = "tpOutput";
            this.tpOutput.Padding = new System.Windows.Forms.Padding(3);
            this.tpOutput.Size = new System.Drawing.Size(776, 467);
            this.tpOutput.TabIndex = 0;
            this.tpOutput.Text = "排程及派工报表";
            this.tpOutput.UseVisualStyleBackColor = true;
            // 
            // dgvOutput
            // 
            this.dgvOutput.AllowUserToAddRows = false;
            this.dgvOutput.AllowUserToDeleteRows = false;
            this.dgvOutput.AllowUserToResizeRows = false;
            this.dgvOutput.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvOutput.ColumnHeadersVisible = false;
            this.dgvOutput.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvOutput.Location = new System.Drawing.Point(3, 3);
            this.dgvOutput.Name = "dgvOutput";
            this.dgvOutput.ReadOnly = true;
            this.dgvOutput.RowTemplate.Height = 37;
            this.dgvOutput.Size = new System.Drawing.Size(770, 461);
            this.dgvOutput.TabIndex = 0;
            this.dgvOutput.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.dgvOutput_CellPainting);
            // 
            // tpMasterData
            // 
            this.tpMasterData.Controls.Add(this.dgvMasterData);
            this.tpMasterData.Location = new System.Drawing.Point(4, 22);
            this.tpMasterData.Name = "tpMasterData";
            this.tpMasterData.Padding = new System.Windows.Forms.Padding(3);
            this.tpMasterData.Size = new System.Drawing.Size(776, 471);
            this.tpMasterData.TabIndex = 1;
            this.tpMasterData.Text = "主数据维护";
            this.tpMasterData.UseVisualStyleBackColor = true;
            // 
            // dgvMasterData
            // 
            this.dgvMasterData.AllowUserToAddRows = false;
            this.dgvMasterData.AllowUserToDeleteRows = false;
            this.dgvMasterData.AllowUserToResizeRows = false;
            this.dgvMasterData.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dgvMasterData.Dock = System.Windows.Forms.DockStyle.Fill;
            this.dgvMasterData.Location = new System.Drawing.Point(3, 3);
            this.dgvMasterData.Name = "dgvMasterData";
            this.dgvMasterData.ReadOnly = true;
            this.dgvMasterData.RowHeadersWidth = 20;
            this.dgvMasterData.RowTemplate.Height = 37;
            this.dgvMasterData.Size = new System.Drawing.Size(770, 465);
            this.dgvMasterData.TabIndex = 0;
            // 
            // btnSendtoObserver
            // 
            this.btnSendtoObserver.Location = new System.Drawing.Point(687, 15);
            this.btnSendtoObserver.Name = "btnSendtoObserver";
            this.btnSendtoObserver.Size = new System.Drawing.Size(90, 30);
            this.btnSendtoObserver.TabIndex = 11;
            this.btnSendtoObserver.Text = " 发送至监控";
            this.btnSendtoObserver.UseVisualStyleBackColor = true;
            this.btnSendtoObserver.Click += new System.EventHandler(this.btnSendtoObserver_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 17F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(784, 561);
            this.Controls.Add(this.scMain);
            this.Font = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.MaximumSize = new System.Drawing.Size(800, 600);
            this.MinimumSize = new System.Drawing.Size(800, 600);
            this.Name = "Form1";
            this.Text = "机舱总成排程及派工查询工具";
            this.scMain.Panel1.ResumeLayout(false);
            this.scMain.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.scMain)).EndInit();
            this.scMain.ResumeLayout(false);
            this.tcMain.ResumeLayout(false);
            this.tpOutput.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvOutput)).EndInit();
            this.tpMasterData.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.dgvMasterData)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer scMain;
        private System.Windows.Forms.TabControl tcMain;
        private System.Windows.Forms.TabPage tpOutput;
        private System.Windows.Forms.DataGridView dgvOutput;
        private System.Windows.Forms.TabPage tpMasterData;
        private System.Windows.Forms.DataGridView dgvMasterData;
        private System.Windows.Forms.Button bExport;
        private System.Windows.Forms.Button bGenerate;
        private System.Windows.Forms.Button bImport;
        private System.Windows.Forms.Button btnSend;
        private System.Windows.Forms.Button btnSendtoObserver;
    }
}

