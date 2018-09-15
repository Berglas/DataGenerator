using System;
using System.Threading;

namespace DataGenerator.Class
{
    public class TimerSetting
    {
        /// <summary>
        /// 設定要定期執行的Timer
        /// </summary>
        public static void TimerInitialize()
        {
            AnalyzeTable AnalyzeResult = new AnalyzeTable();
            bool Run = true;

            while (Run)
            {
                DateTime nowDate = DateTime.Now;

                try
                {
                    for (int i = 0; i < AnalyzeResult.TableList.Count; i++)
                    {
                        if (TimeDifference(DateTime.Parse(AnalyzeResult.TableList[i][2]), nowDate) > Int32.Parse(AnalyzeResult.TableList[i][1]))
                        {
                            string SQL = ConvertToSQL(AnalyzeResult.TableList[i]);
                            ConnectDB.ConnectForPostgres._ExecuteCommand(SQL);
                            AnalyzeResult.TableList[i][2] = nowDate.ToString("yyyy-MM-dd HH:mm:ss.fff");
                            for (int j = 0; j < AnalyzeResult.TableList[i].Length; j++)
                            {
                                string[] column = AnalyzeResult.TableList[i][j].Split(new[] { "-->>" }, StringSplitOptions.None);
                                if (column.Length > 1)
                                {
                                    //針對IntRangeCount此屬性做運算
                                    if (column[1].Split(new[] { "::" }, StringSplitOptions.None)[0] == "IntRangeCount")
                                    {
                                        AnalyzeResult.TableList[i][j] = IntRangeCount(column[0], column[1]);
                                    }
                                }
                            }
                        }
                    }

                }
                catch (Exception ex)
                {
                    Run = false;
                }

                Thread.Sleep(300);
            }
        }

        /// <summary>
        /// 計算時間差異(單位:秒)
        /// </summary>
        /// <param name="previousTime">上次時間</param>
        /// <param name="thisTime">此次時間</param>
        /// <returns></returns>
        static double TimeDifference(DateTime previousTime, DateTime thisTime)
        {
            double ReturnVal = -1;
            TimeSpan ts = thisTime - previousTime;
            ReturnVal = ts.TotalMilliseconds;

            return ReturnVal;
        }

        /// <summary>
        /// 將分析過後的資料表轉成要寫入SQL
        /// </summary>
        /// <param name="TableArray"></param>
        /// <returns></returns>
        static string ConvertToSQL(string[] TableArray)
        {
            string column = "";
            string value = "";
            string updateSQL = "";
            try
            {
                for (int i = 3; i < TableArray.Length; i++)
                {
                    string tempC = TableArray[i].Split(new[] { "-->>" }, StringSplitOptions.None)[0];
                    string tempV = TableArray[i].Split(new[] { "-->>" }, StringSplitOptions.None)[1];

                    //判斷是否為最後一筆。(True:不加逗號、Flase:加逗號)
                    if (i == TableArray.Length - 1)
                    {
                        column += "\"" + tempC + "\"";
                        value += AnalyzeValue(tempV);
                        updateSQL+= "\"" + tempC + "\" = "+ AnalyzeValue(tempV);
                    }
                    else
                    {
                        column += "\"" + tempC + "\",";
                        value += AnalyzeValue(tempV) + ",";
                        updateSQL += "\"" + tempC + "\" = " + AnalyzeValue(tempV) + ",";
                    }

                }
                string resultSQL = string.Format("INSERT INTO \"{0}\" ({1}) VALUES({2})" +
                                                 "ON CONFLICT ON CONSTRAINT \"{0}_pkey\" DO UPDATE SET {3}", TableArray[0], column, value, updateSQL);

                return resultSQL;
            }
            catch (Exception ex)
            {
                return "";
            }
        }

        /// <summary>
        /// 分析且轉換模擬資料內容
        /// </summary>
        /// <param name="Value"></param>
        /// <returns></returns>
        static string AnalyzeValue(string Value)
        {
            try
            {
                string type = Value.Split(new[] { "::" }, StringSplitOptions.None)[0];
                string content = "";
                if (Value.Split(new[] { "::" }, StringSplitOptions.None).Length > 1)
                {
                    content = Value.Split(new[] { "::" }, StringSplitOptions.None)[1];
                }

                string result = "";
                Random myRandom = new Random();
                int index = 0;
                int min = 0;
                int max = 0;

                switch (type)
                {
                    case "NULL":
                        result = "NULL";
                        break;

                    case "NowDate":
                        result = "'" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss") + "'";
                        break;

                    case "IntRange":
                        if (content.Split('~').Length > 1)
                        {
                            min = Int32.Parse(content.Split('~')[0]);
                            max = Int32.Parse(content.Split('~')[1]);
                        }
                        else
                        {
                            min = Int32.Parse(content.Split('~')[0]);
                            max = Int32.Parse(content.Split('~')[0]);
                        }
                        result = "'" + myRandom.Next(min, max + 1).ToString() + "'";
                        break;

                    case "StringSelect":
                        index = myRandom.Next(0, 10000) % content.Split(new[] { "^^" }, StringSplitOptions.None).Length;
                        //因要使亂數種子有效地(不重複)產生亂數，故要延遲100毫秒
                        Thread.Sleep(100);
                        result = "'" + content.Split(new[] { "^^" }, StringSplitOptions.None)[index] + "'";
                        break;

                    case "FixedString":
                        result = "'" + content + "'";
                        break;

                    case "IntRangeCount":
                        result = "'" + content.Split(',')[0] + "'";
                        break;

                    default:
                        result = "NULL";
                        break;
                }

                return result;
            }
            catch (Exception ex)
            {
                return "";
            }

        }

        /// <summary>
        /// 針對IntRangeCount此屬性做運算
        /// </summary>
        /// <param name="_Itme"></param>
        /// <returns></returns>
        static string IntRangeCount(string columnName, string columnValue)
        {
            columnValue=columnValue.Replace("IntRangeCount::", "");
            int index = Int32.Parse(columnValue.Split(',')[0]);
            int min = Int32.Parse(columnValue.Split(',')[1].Split('~')[0]);
            int max = Int32.Parse(columnValue.Split(',')[1].Split('~')[1]);
            if (index < max)
            {
                index++;
            }
            else if (index == max)
            {
                index = min;
            }
            string x = columnName + "-->>IntRangeCount::" + index.ToString() + "," + min.ToString() + "~" + max.ToString();
            return columnName + "-->>IntRangeCount::" + index.ToString() + "," + min.ToString() + "~" + max.ToString();
            
        }
    }
}
