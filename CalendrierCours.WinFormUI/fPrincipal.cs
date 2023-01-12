using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CalendrierCours.WinFormUI
{
    public partial class fPrincipal : Form
    {
        public string MessageInformation { get; set; }

        public fPrincipal()
        {
            InitializeComponent();
        }

        private void tsmiQuitter_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void fPrincipal_Load(object sender, EventArgs e)
        {
            this.lInformation.Text = MessageInformation;
        }
    }
}
