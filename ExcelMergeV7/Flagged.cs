using System;
using System.Collections.Generic;
using System.Windows.Forms;

namespace ExcelMergeV7
{
    public partial class Flagged : Form
    {
        public Flagged()
        {
            InitializeComponent();
        }

        private Functions ft = new Functions();

        private void printF(List<string> flags)
        {
            foreach (string date in flags)
            {
                Console.WriteLine(date);
            }
        }

        public Flagged(List<string> flags)
        {
            InitializeComponent();
            label1.Text = flags.Count + " Fechas sin datos: ";
            //printF(flags);
            try
            {
                ft.WriteDGV2(flags, dataGridView1);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        private void Flagged_Load(object sender, EventArgs e)
        {

        }
    }
}