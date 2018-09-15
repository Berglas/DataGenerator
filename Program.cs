using DataGenerator.Class;
using System;
using System.Windows.Forms;

namespace DataGenerator
{
    static class Program
    {
        /// <summary>
        /// 應用程式的主要進入點。
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            try
            {
                //判斷是否要執行之依據
                bool isExecute = true;

                #region 連接DB
                if (!ConnectDB.ConnectForPostgres._ConnectDB())
                {
                    MessageBox.Show("連接資料庫失敗!");
                    isExecute = false;
                    return;
                }
                #endregion                

                //執行主程式
                if (isExecute)
                {
                    Application.Run(new Main());
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Data.ToString());
            }
        }
    }
}
