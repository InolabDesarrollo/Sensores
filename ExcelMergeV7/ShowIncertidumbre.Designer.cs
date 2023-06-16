namespace ExcelMergeV7
{
    partial class ShowIncertidumbre
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowIncertidumbre));
            System.Windows.Forms.DataGridViewCellStyle dataGridViewCellStyle1 = new System.Windows.Forms.DataGridViewCellStyle();
            this.IncDGV = new System.Windows.Forms.DataGridView();
            this.dataSet11 = new ExcelMergeV7.DataSet1();
            this.data = new System.Windows.Forms.Label();
            this.report = new System.Windows.Forms.Button();
            this.grafica = new System.Windows.Forms.Button();
            this.titulo = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.referencia = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.EncRep = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.anexoG = new System.Windows.Forms.TextBox();
            this.apertura = new System.Windows.Forms.Button();
            this.SD = new System.Windows.Forms.Label();
            this.Date1TB = new System.Windows.Forms.TextBox();
            this.Date2TB = new System.Windows.Forms.TextBox();
            this.anexoTB = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.mins = new System.Windows.Forms.TextBox();
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.graficaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.reporteToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.aperturaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.validatorToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.configuraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.ModDateCB = new System.Windows.Forms.CheckBox();
            this.NewDate = new System.Windows.Forms.TextBox();
            this.distribuciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.penetraciónToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.IncDGV)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).BeginInit();
            this.menuStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // IncDGV
            // 
            this.IncDGV.AllowUserToAddRows = false;
            this.IncDGV.AllowUserToDeleteRows = false;
            this.IncDGV.AllowUserToOrderColumns = true;
            resources.ApplyResources(this.IncDGV, "IncDGV");
            this.IncDGV.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.IncDGV.CellBorderStyle = System.Windows.Forms.DataGridViewCellBorderStyle.None;
            this.IncDGV.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single;
            this.IncDGV.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.IncDGV.Name = "IncDGV";
            this.IncDGV.ReadOnly = true;
            dataGridViewCellStyle1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(234)))), ((int)(((byte)(240)))), ((int)(((byte)(248)))));
            dataGridViewCellStyle1.NullValue = null;
            this.IncDGV.RowsDefaultCellStyle = dataGridViewCellStyle1;
            this.IncDGV.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.IncDGV_DataBindingComplete);
            this.IncDGV.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.IncDGV_RowPostPaint);
            // 
            // dataSet11
            // 
            this.dataSet11.DataSetName = "DataSet1";
            this.dataSet11.SchemaSerializationMode = System.Data.SchemaSerializationMode.IncludeSchema;
            // 
            // data
            // 
            resources.ApplyResources(this.data, "data");
            this.data.Name = "data";
            // 
            // report
            // 
            resources.ApplyResources(this.report, "report");
            this.report.Name = "report";
            this.report.UseVisualStyleBackColor = true;
            this.report.Click += new System.EventHandler(this.report_Click);
            // 
            // grafica
            // 
            resources.ApplyResources(this.grafica, "grafica");
            this.grafica.Name = "grafica";
            this.grafica.UseVisualStyleBackColor = true;
            this.grafica.Click += new System.EventHandler(this.grafica_Click);
            // 
            // titulo
            // 
            this.titulo.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.titulo, "titulo");
            this.titulo.Name = "titulo";
            this.titulo.TextChanged += new System.EventHandler(this.titulo_TextChanged);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // referencia
            // 
            this.referencia.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.referencia, "referencia");
            this.referencia.Name = "referencia";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // EncRep
            // 
            this.EncRep.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.EncRep, "EncRep");
            this.EncRep.Name = "EncRep";
            // 
            // label4
            // 
            resources.ApplyResources(this.label4, "label4");
            this.label4.Name = "label4";
            // 
            // anexoG
            // 
            this.anexoG.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.anexoG, "anexoG");
            this.anexoG.Name = "anexoG";
            // 
            // apertura
            // 
            resources.ApplyResources(this.apertura, "apertura");
            this.apertura.Name = "apertura";
            this.apertura.UseVisualStyleBackColor = true;
            this.apertura.Click += new System.EventHandler(this.apertura_Click);
            // 
            // SD
            // 
            resources.ApplyResources(this.SD, "SD");
            this.SD.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(76)))), ((int)(((byte)(36)))));
            this.SD.Name = "SD";
            // 
            // Date1TB
            // 
            this.Date1TB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.Date1TB, "Date1TB");
            this.Date1TB.Name = "Date1TB";
            // 
            // Date2TB
            // 
            this.Date2TB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.Date2TB, "Date2TB");
            this.Date2TB.Name = "Date2TB";
            // 
            // anexoTB
            // 
            this.anexoTB.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.anexoTB, "anexoTB");
            this.anexoTB.Name = "anexoTB";
            this.anexoTB.TextChanged += new System.EventHandler(this.anexoTB_TextChanged);
            // 
            // label5
            // 
            resources.ApplyResources(this.label5, "label5");
            this.label5.Name = "label5";
            // 
            // label6
            // 
            resources.ApplyResources(this.label6, "label6");
            this.label6.Name = "label6";
            // 
            // label7
            // 
            resources.ApplyResources(this.label7, "label7");
            this.label7.Name = "label7";
            // 
            // label8
            // 
            resources.ApplyResources(this.label8, "label8");
            this.label8.Name = "label8";
            // 
            // mins
            // 
            this.mins.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.mins, "mins");
            this.mins.Name = "mins";
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.graficaToolStripMenuItem,
            this.reporteToolStripMenuItem,
            this.aperturaToolStripMenuItem,
            this.validatorToolStripMenuItem,
            this.configuraciónToolStripMenuItem});
            resources.ApplyResources(this.menuStrip1, "menuStrip1");
            this.menuStrip1.Name = "menuStrip1";
            // 
            // graficaToolStripMenuItem
            // 
            this.graficaToolStripMenuItem.Name = "graficaToolStripMenuItem";
            resources.ApplyResources(this.graficaToolStripMenuItem, "graficaToolStripMenuItem");
            this.graficaToolStripMenuItem.Click += new System.EventHandler(this.grafica_Click);
            // 
            // reporteToolStripMenuItem
            // 
            this.reporteToolStripMenuItem.Name = "reporteToolStripMenuItem";
            resources.ApplyResources(this.reporteToolStripMenuItem, "reporteToolStripMenuItem");
            this.reporteToolStripMenuItem.Click += new System.EventHandler(this.report_Click);
            // 
            // aperturaToolStripMenuItem
            // 
            this.aperturaToolStripMenuItem.Name = "aperturaToolStripMenuItem";
            resources.ApplyResources(this.aperturaToolStripMenuItem, "aperturaToolStripMenuItem");
            this.aperturaToolStripMenuItem.Click += new System.EventHandler(this.apertura_Click);
            // 
            // validatorToolStripMenuItem
            // 
            this.validatorToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.distribuciónToolStripMenuItem,
            this.penetraciónToolStripMenuItem});
            this.validatorToolStripMenuItem.Name = "validatorToolStripMenuItem";
            resources.ApplyResources(this.validatorToolStripMenuItem, "validatorToolStripMenuItem");
            // 
            // configuraciónToolStripMenuItem
            // 
            this.configuraciónToolStripMenuItem.Name = "configuraciónToolStripMenuItem";
            resources.ApplyResources(this.configuraciónToolStripMenuItem, "configuraciónToolStripMenuItem");
            this.configuraciónToolStripMenuItem.Click += new System.EventHandler(this.configuraciónToolStripMenuItem_Click);
            // 
            // ModDateCB
            // 
            resources.ApplyResources(this.ModDateCB, "ModDateCB");
            this.ModDateCB.Name = "ModDateCB";
            this.ModDateCB.UseVisualStyleBackColor = true;
            this.ModDateCB.CheckedChanged += new System.EventHandler(this.ModDateCB_CheckedChanged);
            // 
            // NewDate
            // 
            resources.ApplyResources(this.NewDate, "NewDate");
            this.NewDate.Name = "NewDate";
            this.NewDate.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.NewDate_KeyPress);
            // 
            // distribuciónToolStripMenuItem
            // 
            this.distribuciónToolStripMenuItem.Name = "distribuciónToolStripMenuItem";
            resources.ApplyResources(this.distribuciónToolStripMenuItem, "distribuciónToolStripMenuItem");
            // 
            // penetraciónToolStripMenuItem
            // 
            this.penetraciónToolStripMenuItem.Name = "penetraciónToolStripMenuItem";
            resources.ApplyResources(this.penetraciónToolStripMenuItem, "penetraciónToolStripMenuItem");
            // 
            // ShowIncertidumbre
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(224)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.NewDate);
            this.Controls.Add(this.ModDateCB);
            this.Controls.Add(this.mins);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.anexoTB);
            this.Controls.Add(this.Date2TB);
            this.Controls.Add(this.Date1TB);
            this.Controls.Add(this.SD);
            this.Controls.Add(this.apertura);
            this.Controls.Add(this.anexoG);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.EncRep);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.referencia);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.titulo);
            this.Controls.Add(this.grafica);
            this.Controls.Add(this.report);
            this.Controls.Add(this.data);
            this.Controls.Add(this.IncDGV);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "ShowIncertidumbre";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ShowIncertidumbre_FormClosing);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.ShowIncertidumbre_FormClosed);
            ((System.ComponentModel.ISupportInitialize)(this.IncDGV)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataSet11)).EndInit();
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        public System.Windows.Forms.DataGridView IncDGV;
        private DataSet1 dataSet11;
        private System.Windows.Forms.Label data;
        private System.Windows.Forms.Button report;
        private System.Windows.Forms.Button grafica;
        private System.Windows.Forms.TextBox titulo;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox referencia;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox EncRep;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox anexoG;
        private System.Windows.Forms.Button apertura;
        private System.Windows.Forms.Label SD;
        private System.Windows.Forms.TextBox Date1TB;
        private System.Windows.Forms.TextBox Date2TB;
        private System.Windows.Forms.TextBox anexoTB;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.TextBox mins;
        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem graficaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem reporteToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem aperturaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem validatorToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem configuraciónToolStripMenuItem;
        private System.Windows.Forms.CheckBox ModDateCB;
        private System.Windows.Forms.TextBox NewDate;
        private System.Windows.Forms.ToolStripMenuItem distribuciónToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem penetraciónToolStripMenuItem;
    }
}