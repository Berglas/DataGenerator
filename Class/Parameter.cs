using System.Xml;

namespace DataGenerator.Class
{
    public class Parameter
    {
        #region tag <DataBase> Parmeter Setting
        public string DataBase_ServerIP { get; set; }
        public string DataBase_ServerPort { get; set; }
        public string DataBase_DBName { get; set; }
        public string DataBase_UserID { get; set; }
        public string DataBase_Password { get; set; }
        #endregion

        #region tag <FilePath> Parmeter Setting
        public string FilePath_TableSchema { get; set; }
        public string FilePath_Log { get; set; }
        #endregion

        /// <summary>
        /// 建構參數檔
        /// </summary>
        public Parameter()
        {
            XmlDocument xmlConfig = new XmlDocument();
            xmlConfig.Load("../../Config.xml");//載入xml檔

            //建tag <DataBase>構參數檔
            DataBase_ServerIP = xmlConfig.SelectSingleNode("/Parameter/Database/ServerIP").Attributes["value"].Value;
            DataBase_ServerPort = xmlConfig.SelectSingleNode("/Parameter/Database/ServerPort").Attributes["value"].Value;
            DataBase_UserID = xmlConfig.SelectSingleNode("/Parameter/Database/UserID").Attributes["value"].Value;
            DataBase_Password = xmlConfig.SelectSingleNode("/Parameter/Database/Password").Attributes["value"].Value;
            DataBase_DBName = xmlConfig.SelectSingleNode("/Parameter/Database/DBName ").Attributes["value"].Value;

            //建tag <FilePath>構參數檔
            FilePath_TableSchema = xmlConfig.SelectSingleNode("/Parameter/FilePath/TableSchema").Attributes["value"].Value;
            FilePath_Log= xmlConfig.SelectSingleNode("/Parameter/FilePath/Log").Attributes["value"].Value;
        }
    }
}
