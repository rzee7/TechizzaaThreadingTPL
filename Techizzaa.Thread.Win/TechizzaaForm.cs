using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Techizzaa
{
    public partial class TechizzaaForm : Form
    {
        public TechizzaaForm()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Thread.Sleep(10000);

            //Task.Factory.StartNew(() =>
            //{
            //    Thread.Sleep(10000);
            //});
        }
    }
}
