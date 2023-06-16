namespace ExcelMergeV7
{
    partial class Start
    {
        /// <summary>
        /// Variable del diseñador necesaria.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpiar los recursos que se estén usando.
        /// </summary>
        /// <param name="disposing">true si los recursos administrados se deben desechar; false en caso contrario.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        /// <summary>
        /// Método necesario para admitir el Diseñador. No se puede modificar
        /// el contenido de este método con el editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Start));
            this.carga1 = new System.Windows.Forms.ProgressBar();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.temperatura = new System.Windows.Forms.CheckBox();
            this.humedad = new System.Windows.Forms.CheckBox();
            this.usr = new System.Windows.Forms.TextBox();
            this.pass = new System.Windows.Forms.TextBox();
            this.iniciar = new System.Windows.Forms.Button();
            this.imageList1 = new System.Windows.Forms.ImageList(this.components);
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label1 = new System.Windows.Forms.Label();
            this.DirectToServer = new System.Windows.Forms.CheckBox();
            this.backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            this.SuspendLayout();
            // 
            // carga1
            // 
            resources.ApplyResources(this.carga1, "carga1");
            this.carga1.Name = "carga1";
            this.carga1.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // temperatura
            // 
            resources.ApplyResources(this.temperatura, "temperatura");
            this.temperatura.Name = "temperatura";
            this.temperatura.UseVisualStyleBackColor = true;
            this.temperatura.CheckedChanged += new System.EventHandler(this.temperatura_CheckedChanged);
            // 
            // humedad
            // 
            resources.ApplyResources(this.humedad, "humedad");
            this.humedad.Name = "humedad";
            this.humedad.UseVisualStyleBackColor = true;
            this.humedad.CheckedChanged += new System.EventHandler(this.humedad_CheckedChanged);
            // 
            // usr
            // 
            this.usr.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.usr, "usr");
            this.usr.Name = "usr";
            // 
            // pass
            // 
            this.pass.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            resources.ApplyResources(this.pass, "pass");
            this.pass.Name = "pass";
            this.pass.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.pass_KeyPress);
            // 
            // iniciar
            // 
            resources.ApplyResources(this.iniciar, "iniciar");
            this.iniciar.Name = "iniciar";
            this.iniciar.UseVisualStyleBackColor = true;
            this.iniciar.Click += new System.EventHandler(this.iniciar_Click);
            // 
            // imageList1
            // 
            this.imageList1.ColorDepth = System.Windows.Forms.ColorDepth.Depth8Bit;
            resources.ApplyResources(this.imageList1, "imageList1");
            this.imageList1.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // label2
            // 
            resources.ApplyResources(this.label2, "label2");
            this.label2.Name = "label2";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label1
            // 
            resources.ApplyResources(this.label1, "label1");
            this.label1.Name = "label1";
            // 
            // DirectToServer
            // 
            resources.ApplyResources(this.DirectToServer, "DirectToServer");
            this.DirectToServer.Name = "DirectToServer";
            this.DirectToServer.UseVisualStyleBackColor = true;
            // 
            // Start
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(224)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.DirectToServer);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.iniciar);
            this.Controls.Add(this.pass);
            this.Controls.Add(this.usr);
            this.Controls.Add(this.humedad);
            this.Controls.Add(this.temperatura);
            this.Controls.Add(this.carga1);
            this.MaximizeBox = false;
            this.Name = "Start";
            this.Activated += new System.EventHandler(this.Start_Activated);
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Start_FormClosing);
            this.Load += new System.EventHandler(this.Start_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        public System.Windows.Forms.ProgressBar carga1;
        private System.Windows.Forms.CheckBox temperatura;
        private System.Windows.Forms.CheckBox humedad;
        private System.Windows.Forms.TextBox usr;
        private System.Windows.Forms.TextBox pass;
        private System.Windows.Forms.Button iniciar;
        private System.Windows.Forms.ImageList imageList1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox DirectToServer;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
    }
}

