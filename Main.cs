using System.Windows.Forms;
using DataGenerator.Class;
using System.Threading;
using System;

namespace DataGenerator
{
    public partial class Main : Form
    {

        public Main()
        {
            InitializeComponent();

            //建立一個執行緒，並且傳入一個委派物件ThreadStart，並且指向TimerSetting.TimerInitialize方法。          
            Thread oThreadA = new Thread(new ThreadStart(TimerSetting.TimerInitialize));
            oThreadA.Name = "A Thread";

            //啟動執行緒物件
            oThreadA.Start();
        }

        private void Main_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Dispose();
            GC.Collect();
        }
    }
}
