namespace ExcelMergeV7
{
    partial class BigDGV
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BigDGV));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.BigDGV1 = new System.Windows.Forms.DataGridView();
            this.loged = new System.Windows.Forms.Label();
            this.charge2 = new System.Windows.Forms.ProgressBar();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.reporte = new System.Windows.Forms.Button();
            this.warning = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.CheckModDate = new System.Windows.Forms.CheckBox();
            this.NewDate = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.configuraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.BigDGV1)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // BigDGV1
            // 
            this.BigDGV1.AllowUserToAddRows = false;
            this.BigDGV1.AllowUserToDeleteRows = false;
            resources.ApplyResources(this.BigDGV1, "BigDGV1");
            this.BigDGV1.BackgroundColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            this.BigDGV1.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.BigDGV1.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.SingleVertical;
            this.BigDGV1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.BigDGV1.Name = "BigDGV1";
            this.BigDGV1.ReadOnly = true;
            dataGridViewCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter;
            dataGridViewCellStyle1.Format = "N2";
            dataGridViewCellStyle1.NullValue = null;
            this.BigDGV1.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.BigDGV1.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.BigDGV1_CellContentClick);
            this.BigDGV1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.BigDGV1_CellContentDoubleClick);
            this.BigDGV1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.BigDGV1_CellMouseDoubleClick);
            this.BigDGV1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.BigDGV1_DataBindingComplete);
            this.BigDGV1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.BigDGV1_RowPostPaint);
            // 
            // loged
            // 
            resources.ApplyResources(this.loged, "loged");
            this.loged.Name = "loged";
            // 
            // charge2
            // 
            resources.ApplyResources(this.charge2, "charge2");
            this.charge2.Name = "charge2";
            this.charge2.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // reporte
            // 
            resources.ApplyResources(this.reporte, "reporte");
            this.reporte.BackColor = System.Drawing.SystemColors.ControlLight;
            this.reporte.FlatAppearance.BorderSize = 0;
            this.reporte.FlatAppearance.MouseDownBackColor = System.Drawing.SystemColors.ActiveCaption;
            this.reporte.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            this.reporte.Name = "reporte";
            this.reporte.UseVisualStyleBackColor = false;
            this.reporte.Click += new System.EventHandler(this.reporte_Click);
            // 
            // warning
            // 
            resources.ApplyResources(this.warning, "warning");
            this.warning.ForeColor = System.Drawing.Color.Red;
            this.warning.Name = "warning";
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // CheckModDate
            // 
            resources.ApplyResources(this.CheckModDate, "CheckModDate");
            this.CheckModDate.Name = "CheckModDate";
            this.CheckModDate.UseVisualStyleBackColor = true;
            this.CheckModDate.CheckedChanged += new System.EventHandler(this.CheckModDate_CheckedChanged);
            // 
            // NewDate
            // 
            this.NewDate.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.NewDate, "NewDate");
            this.NewDate.Name = "NewDate";
            this.NewDate.TextChanged += new System.EventHandler(this.NewDate_TextChanged);
            this.NewDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NewDate_KeyPress);
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.configuraciónToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // configuraciónToolStripMenuItem
            // 
            resources.ApplyResources(this.configuraciónToolStripMenuItem, "configuraciónToolStripMenuItem");
            this.configuraciónToolStripMenuItem.Name = "configuraciónToolStripMenuItem";
            this.configuraciónToolStripMenuItem.Click += new System.EventHandler(this.configuraciónToolStripMenuItem_Click);
            // 
            // BigDGV
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(224)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.NewDate);
            this.Controls.Add(this.CheckModDate);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.warning);
            this.Controls.Add(this.reporte);
            this.Controls.Add(this.charge2);
            this.Controls.Add(this.loged);
            this.Controls.Add(this.BigDGV1);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "BigDGV";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.BigDGV_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.BigDGV_FormClosed);
            this.Load += new System.EventHandler(this.BigDGV_Load);
            this.VisibleChanged += new System.EventHandler(this.BigDGV_VisibleChanged);
            ((System.ComponentModel.ISupportInitialize)(this.BigDGV1)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        public System.Windows.Forms.DataGridView BigDGV1;
        private System.Windows.Forms.Label loged;
        public System.Windows.Forms.ProgressBar charge2;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Button reporte;
        private System.Windows.Forms.Label warning;
        public System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox CheckModDate;
        private System.Windows.Forms.TextBox NewDate;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem configuraciónToolStripMenuItem;
    }
}