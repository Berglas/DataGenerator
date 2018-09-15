using System.Collections.Generic;
using System.IO;

namespace DataGenerator.Class
{
    public class AnalyzeTable
    {
        public List<string[]> TableList { get; set; }

        public AnalyzeTable()
        {
            List<string[]> tempList = new List<string[]>();
            Parameter ParameterPool = new Parameter();

            // 取得TableSchema資料夾路徑
            string folderPath = ParameterPool.FilePath_TableSchema;

            // 取得資料夾內所有檔案
            foreach (string f in Directory.GetFiles(folderPath))
            {
                List<string> myList = new List<string>();
                //// 將檔名(即為資料表名)存入第一行
                //myList.Add(Path.GetFileNameWithoutExtension(f));
                string line;

                // 一次讀取一行
                System.IO.StreamReader file = new System.IO.StreamReader(f);
                while ((line = file.ReadLine()) != null)
                {
                    if (!(line.Trim() == string.Empty))
                    {
                        myList.Add(line.Trim());
                    }

                }

                file.Close();
                tempList.Add(myList.ToArray());
            }

            TableList = tempList;
        }

    }
}
