using AnTest.Models;
using AnTest.Services;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnTest
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btnProcess_Click(object sender, EventArgs e)
        {
            var email = new EmailModel
            {
                To = tbTo.Text,
                Copy = tbCopy.Text
            };

            try
            {
                var processedCopy = EmailProcessing.ProcessEmailCopy(email);
                lbResult.Text = $"To: {email.To} \nCopy: {processedCopy}";
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка обработки email: {ex.Message}", "Ошибка",
                    MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}
