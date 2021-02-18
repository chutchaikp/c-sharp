using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace c_sharp_thread
{

    public partial class Form1 : Form
    {
        private static readonly log4net.ILog log = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);


        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            log.Debug( DateTime.Now.ToString("o") );
            var users = FakeData(10);

            foreach (User user in users)
            {
                log.Debug(user.ToJSON());
            }
        }

        static List<User> FakeData(int nb)
        {
            FakeData fake = new FakeData();
            return fake.Data(nb);
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }




    }
}
