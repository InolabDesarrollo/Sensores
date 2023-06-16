namespace ExcelMergeV7
{
    partial class Apertura
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Apertura));
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.inicio = new System.Windows.Forms.TextBox();
            this.fin = new System.Windows.Forms.TextBox();
            this.clear = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.primero = new System.Windows.Forms.TextBox();
            this.ultimo = new System.Windows.Forms.TextBox();
            this.ver = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.enc = new System.Windows.Forms.TextBox();
            this.anexo = new System.Windows.Forms.TextBox();
            this.anexoG = new System.Windows.Forms.TextBox();
            this.encG = new System.Windows.Forms.TextBox();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.graph = new System.Windows.Forms.Button();
            this.StFs = new System.Windows.Forms.Label();
            this.EndLst = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Location = new System.Drawing.Point(0, 188);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.ReadOnly = true;
            this.dataGridView1.Size = new System.Drawing.Size(800, 262);
            this.dataGridView1.TabIndex = 0;
            this.dataGridView1.CellContentDoubleClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridView1_CellContentDoubleClick);
            this.dataGridView1.CellMouseDoubleClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.dataGridView1_CellMouseDoubleClick);
            this.dataGridView1.DataBindingComplete += new System.Windows.Forms.DataGridViewBindingCompleteEventHandler(this.dataGridView1_DataBindingComplete);
            this.dataGridView1.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.dataGridView1_RowPostPaint);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 13);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(32, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Inicio";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(16, 59);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(21, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Fin";
            // 
            // inicio
            // 
            this.inicio.Location = new System.Drawing.Point(51, 10);
            this.inicio.Name = "inicio";
            this.inicio.Size = new System.Drawing.Size(149, 20);
            this.inicio.TabIndex = 3;
            // 
            // fin
            // 
            this.fin.Location = new System.Drawing.Point(51, 56);
            this.fin.Name = "fin";
            this.fin.Size = new System.Drawing.Size(149, 20);
            this.fin.TabIndex = 4;
            // 
            // clear
            // 
            this.clear.Location = new System.Drawing.Point(75, 94);
            this.clear.Name = "clear";
            this.clear.Size = new System.Drawing.Size(75, 23);
            this.clear.TabIndex = 5;
            this.clear.Text = "Limpiar";
            this.clear.UseVisualStyleBackColor = true;
            this.clear.Click += new System.EventHandler(this.clear_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(241, 13);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(70, 13);
            this.label3.TabIndex = 6;
            this.label3.Text = "Primer sensor";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(241, 59);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(72, 13);
            this.label4.TabIndex = 7;
            this.label4.Text = "Último Sensor";
            // 
            // primero
            // 
            this.primero.Location = new System.Drawing.Point(326, 10);
            this.primero.Name = "primero";
            this.primero.Size = new System.Drawing.Size(149, 20);
            this.primero.TabIndex = 8;
            this.primero.TextChanged += new System.EventHandler(this.primero_TextChanged);
            // 
            // ultimo
            // 
            this.ultimo.Location = new System.Drawing.Point(326, 56);
            this.ultimo.Name = "ultimo";
            this.ultimo.Size = new System.Drawing.Size(149, 20);
            this.ultimo.TabIndex = 9;
            this.ultimo.TextChanged += new System.EventHandler(this.ultimo_TextChanged);
            // 
            // ver
            // 
            this.ver.Location = new System.Drawing.Point(713, 10);
            this.ver.Name = "ver";
            this.ver.Size = new System.Drawing.Size(75, 23);
            this.ver.TabIndex = 10;
            this.ver.Text = "Ver";
            this.ver.UseVisualStyleBackColor = true;
            this.ver.Click += new System.EventHandler(this.ver_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(508, 9);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 11;
            this.label5.Text = "Encabezado:";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(508, 55);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(43, 13);
            this.label6.TabIndex = 12;
            this.label6.Text = "Anexo: ";
            // 
            // enc
            // 
            this.enc.Location = new System.Drawing.Point(511, 25);
            this.enc.MaxLength = 200;
            this.enc.Name = "enc";
            this.enc.Size = new System.Drawing.Size(170, 20);
            this.enc.TabIndex = 13;
            this.enc.TextChanged += new System.EventHandler(this.enc_TextChanged);
            // 
            // anexo
            // 
            this.anexo.Location = new System.Drawing.Point(511, 71);
            this.anexo.MaxLength = 200;
            this.anexo.Name = "anexo";
            this.anexo.Size = new System.Drawing.Size(170, 20);
            this.anexo.TabIndex = 14;
            this.anexo.TextChanged += new System.EventHandler(this.anexo_TextChanged);
            // 
            // anexoG
            // 
            this.anexoG.Location = new System.Drawing.Point(511, 161);
            this.anexoG.MaxLength = 200;
            this.anexoG.Name = "anexoG";
            this.anexoG.Size = new System.Drawing.Size(170, 20);
            this.anexoG.TabIndex = 18;
            // 
            // encG
            // 
            this.encG.Location = new System.Drawing.Point(511, 115);
            this.encG.MaxLength = 200;
            this.encG.Name = "encG";
            this.encG.Size = new System.Drawing.Size(170, 20);
            this.encG.TabIndex = 17;
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(508, 145);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(80, 13);
            this.label7.TabIndex = 16;
            this.label7.Text = "Anexo Gráfica: ";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(508, 99);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(107, 13);
            this.label8.TabIndex = 15;
            this.label8.Text = "Encabezado Gráfica:";
            // 
            // graph
            // 
            this.graph.Location = new System.Drawing.Point(713, 115);
            this.graph.Name = "graph";
            this.graph.Size = new System.Drawing.Size(75, 23);
            this.graph.TabIndex = 19;
            this.graph.Text = "Gráfica";
            this.graph.UseVisualStyleBackColor = true;
            this.graph.Click += new System.EventHandler(this.graph_Click);
            // 
            // StFs
            // 
            this.StFs.AutoSize = true;
            this.StFs.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.StFs.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(76)))), ((int)(((byte)(36)))));
            this.StFs.Location = new System.Drawing.Point(13, 122);
            this.StFs.Name = "StFs";
            this.StFs.Size = new System.Drawing.Size(41, 13);
            this.StFs.TabIndex = 20;
            this.StFs.Text = "label9";
            this.StFs.Visible = false;
            // 
            // EndLst
            // 
            this.EndLst.AutoSize = true;
            this.EndLst.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.EndLst.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(4)))), ((int)(((byte)(76)))), ((int)(((byte)(36)))));
            this.EndLst.Location = new System.Drawing.Point(13, 161);
            this.EndLst.Name = "EndLst";
            this.EndLst.Size = new System.Drawing.Size(48, 13);
            this.EndLst.TabIndex = 21;
            this.EndLst.Text = "label10";
            this.EndLst.Visible = false;
            // 
            // Apertura
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(224)))), ((int)(((byte)(240)))));
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.EndLst);
            this.Controls.Add(this.StFs);
            this.Controls.Add(this.graph);
            this.Controls.Add(this.anexoG);
            this.Controls.Add(this.encG);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.anexo);
            this.Controls.Add(this.enc);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.ver);
            this.Controls.Add(this.ultimo);
            this.Controls.Add(this.primero);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.clear);
            this.Controls.Add(this.fin);
            this.Controls.Add(this.inicio);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.dataGridView1);
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.Name = "Apertura";
            this.Text = "Apertura";
            this.Load += new System.EventHandler(this.Apertura_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox inicio;
        private System.Windows.Forms.TextBox fin;
        private System.Windows.Forms.Button clear;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox primero;
        private System.Windows.Forms.TextBox ultimo;
        private System.Windows.Forms.Button ver;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox enc;
        private System.Windows.Forms.TextBox anexo;
        private System.Windows.Forms.TextBox anexoG;
        private System.Windows.Forms.TextBox encG;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Button graph;
        private System.Windows.Forms.Label StFs;
        private System.Windows.Forms.Label EndLst;
    }
}