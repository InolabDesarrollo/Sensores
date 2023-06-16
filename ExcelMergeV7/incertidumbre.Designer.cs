namespace ExcelMergeV7
{
    partial class incertidumbre
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(incertidumbre));
            this.Aplicar = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.prop = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.off = new System.Windows.Forms.TextBox();
            this.SuspendLayout();
            // 
            // Aplicar
            // 
            resources.ApplyResources(this.Aplicar, "Aplicar");
            this.Aplicar.Name = "Aplicar";
            this.Aplicar.UseVisualStyleBackColor = true;
            this.Aplicar.Click += new System.EventHandler(this.Aplicar_Click);
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
            // prop
            // 
            resources.ApplyResources(this.prop, "prop");
            this.prop.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.prop.Name = "prop";
            // 
            // label3
            // 
            resources.ApplyResources(this.label3, "label3");
            this.label3.Name = "label3";
            // 
            // off
            // 
            resources.ApplyResources(this.off, "off");
            this.off.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.off.Name = "off";
            // 
            // incertidumbre
            // 
            resources.ApplyResources(this, "$this");
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(196)))), ((int)(((byte)(224)))), ((int)(((byte)(240)))));
            this.Controls.Add(this.off);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.prop);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Aplicar);
            this.Name = "incertidumbre";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.incertidumbre_FormClosing);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button Aplicar;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox prop;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox off;
    }
}