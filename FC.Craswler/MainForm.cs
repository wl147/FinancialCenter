using FC.BLL.Bussiness;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using FC.Base.OperationBase;

namespace FC.Craswler
{
    public partial class MainForm : Form
    {
        public MainForm()
        {
            InitializeComponent();
        }

        private void btn_LeagueInfo_Click(object sender, EventArgs e)
        {
            LeagueMatchesBusiness bll = new LeagueMatchesBusiness();
            bll.CreateLeagueMatch("111", "巴萨","www.baidu.com", 20);
        }
    }
}
