using System;
using Npgsql;

namespace DataGenerator.Class
{
    public class ConnectDB
    {
        public class ConnectForPostgres
        {
            public static NpgsqlConnection conn;

            /// <summary>
            /// 連接資料庫(For PostgreSQL)
            /// </summary>
            /// <returns></returns>
            public static bool _ConnectDB()
            {
                try
                {
                    Parameter ParameterPool = new Parameter();
                    string connString = string.Format("Host={0};Port={1};Username={2};Password={3};Database={4}", 
                        ParameterPool.DataBase_ServerIP, 
                        ParameterPool.DataBase_ServerPort,
                        ParameterPool.DataBase_UserID,
                        ParameterPool.DataBase_Password,
                        ParameterPool.DataBase_DBName);

                    conn = new NpgsqlConnection(connString);
                    conn.Open();
                }
                catch (Exception ex)
                {
                    return false;
                }
                finally
                {
                    conn.Close();
                }
                return true;
            }

            /// <summary>
            /// 執行指令(For PostgreSQL)
            /// </summary>
            /// <param name="SQL">SQL語法</param>
            /// <returns></returns>
            public static bool _ExecuteCommand(string SQL)
            {
                NpgsqlTransaction transaction = null;

                try
                {
                    conn.Open();
                    transaction = conn.BeginTransaction();
                    NpgsqlCommand command = new NpgsqlCommand();
                    command.Connection = conn;
                    command.CommandText = SQL;
                    command.Transaction = transaction;
                    command.ExecuteNonQuery();
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    conn.Close();
                    return false;
                }
                finally
                {
                    conn.Close();
                }
                return true;
            }

        }

        public class ConnectForMySQL
        {
            //連接資料庫(For MySQL) 未完成
            public static bool _ConnectDB()
            {
                try
                {

                }
                catch (Exception ex)
                {
                    return false;
                }
                return true;
            }
        }

    }
}
