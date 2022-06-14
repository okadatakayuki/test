/*
 * Copyright (c) SystemGear.co.,ltd. All rights reserved.
 */

using Batch.Models.Dto;
using Batch.Models.Entity;
using NLog;
using Npgsql;
using NpgsqlTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batch.Class
{
    /// <summary>
    /// ユーザーマスタ登録 Class
    /// </summary>
    internal class UserMasterRegistClass
    {
        /// <summary>
        /// ユーザーマスタ登録 Main
        /// </summary>
        /// <param name="entyUserMaster">ログインユーザーマスタ DataEntityModel</param>
        /// <param name="dteSysDate">システム日付</param>
        /// <param name="logger">NLog logger</param>
        /// <returns>true:OK/false:NG</returns>
        public bool regUserMaster(List<UserMasterEntity> entyUserMaster, DateTime dteSysDate, Logger logger) 
        {
            bool blnReturn = false;
            bool blnRoleBack = false;
            int intDel = 0;
            int intIns = 0;

            try
            {
                // 接続文字列
                var connectionString = "Server=localhost;Port=5432;User Id=itsu;Password=stgr;Database=postgres";

                // DB操作に必要なインスタンスを生成
                using NpgsqlConnection connection = new NpgsqlConnection(connectionString);

                // 接続開始
                connection.Open();

                // トランザクションの開始
                NpgsqlTransaction transaction = connection.BeginTransaction();

                foreach (var rec in entyUserMaster)
                {
                    UserMasterDto datoUserMaster = new UserMasterDto();
                    datoUserMaster.UserId = rec.UserId;
                    datoUserMaster.UserName = rec.UserName;
                    datoUserMaster.Department = rec.Department;
                    string? strPassword = rec.Password;
                    string? strEncryption = null;
                    if (strPassword != null)
                    {
                        strEncryption = EncryptionClass.Main(strPassword);
                    }
                    datoUserMaster.Password = strEncryption;
                    datoUserMaster.DataTimestamp = dteSysDate;

                    try
                    {
                        // ユーザーマスタ削除処理
                        intDel += delUserMaster(connection, datoUserMaster.UserId);

                        // 削除フラグが1以外の場合は、ユーザーマスタ登録処理
                        if (rec.DelFlg != 1)
                        {
                            intIns += insUserMaster(connection, datoUserMaster);
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.Write(ex.Message.ToString());
                        logger.Log(LogLevel.Error, ex.Message.ToString());
                        blnRoleBack = true;
                    }
                }

                if (blnRoleBack)
                {
                    // ロールバックする場合
                    transaction.Rollback();
                    // 接続終了
                    connection.Close();
                    connection.Dispose();
                    intDel = 0;
                    intIns = 0;
                }
                else
                {
                    // コミット
                    transaction.Commit();
                    // 接続終了
                    connection.Close();
                    connection.Dispose();
                    blnReturn = true;
                }
            }
            catch (Exception ex)
            {
                Console.Write(ex.Message.ToString());
                logger.Log(LogLevel.Error, ex.Message.ToString());
            }
            finally 
            {
                logger.Log(LogLevel.Info, "削除: " + intDel.ToString() + "件");
                logger.Log(LogLevel.Info, "登録: " + intIns.ToString() + "件");
            }

            return blnReturn;
        }

        /// <summary>
        /// ユーザーマスタ削除
        /// </summary>
        /// <param name="connection">NpgSql connection</param>
        /// <param name="strUserId">システム日付</param>
        /// <returns>削除件数</returns>
        private int delUserMaster(NpgsqlConnection connection, string strUserId)
        {
            int intResult = 0;

            // Sql作成
            StringBuilder sb = new StringBuilder();
            sb.Append("DELETE ");
            sb.Append("FROM public.user_master ");
            sb.Append("WHERE user_id = :userid");

            try 
            {
                // Sqlセット
                using var cmd = new NpgsqlCommand(sb.ToString(), connection);

                // パラメーターの追加
                cmd.Parameters.Add(new NpgsqlParameter("userid", NpgsqlDbType.Varchar));
                // パラメーターに値をセット
                cmd.Parameters[0].Value = strUserId;

                // Sql実行
                intResult = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }

            return intResult;
        }

        /// <summary>
        /// ユーザーマスタ登録
        /// </summary>
        /// <param name="connection">NpgSql connection</param>
        /// <param name="datoUserMaster">ログインユーザーマスタ DataDtoModel</param>
        /// <returns>登録件数</returns>
        private int insUserMaster(NpgsqlConnection connection, UserMasterDto datoUserMaster)
        {
            int intResult = 0;

            // Sql作成
            StringBuilder sb = new StringBuilder();
            sb.Append("INSERT INTO public.user_master VALUES ");
            sb.Append("( ");
            sb.Append(":userid, ");
            sb.Append(":username, ");
            sb.Append(":department, ");
            sb.Append(":password, ");
            sb.Append(":datatimestamp ");
            sb.Append(") ");

            try
            {
                // Sqlセット
                using var cmd = new NpgsqlCommand(sb.ToString(), connection);

                // パラメーターの追加
                cmd.Parameters.Add(new NpgsqlParameter("userid", NpgsqlDbType.Varchar));
                cmd.Parameters.Add(new NpgsqlParameter("username", NpgsqlDbType.Varchar));
                cmd.Parameters.Add(new NpgsqlParameter("department", NpgsqlDbType.Varchar));
                cmd.Parameters.Add(new NpgsqlParameter("password", NpgsqlDbType.Varchar));
                cmd.Parameters.Add(new NpgsqlParameter("datatimestamp", NpgsqlDbType.Timestamp));
                // パラメーターに値をセット
                cmd.Parameters[0].Value = datoUserMaster.UserId;
                cmd.Parameters[1].Value = datoUserMaster.UserName;
                cmd.Parameters[2].Value = datoUserMaster.Department;
                cmd.Parameters[3].Value = datoUserMaster.Password;
                cmd.Parameters[4].Value = datoUserMaster.DataTimestamp;

                // Sql実行
                intResult = cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                throw;
            }

            return intResult;
        }
    }
}