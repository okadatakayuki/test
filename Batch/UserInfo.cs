/*
 * Copyright (c) SystemGear.co.,ltd. All rights reserved.
 */

using NLog;
using NLog.Config;
using NLog.Targets;
using Batch.Class;
using Batch.Models.Entity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Batch
{
    /// <summary>
    /// ユーザー情報読込
    /// </summary>
    internal class UserInfo
    {
        // NLog設定
        private static Logger logger = LogManager.GetCurrentClassLogger();

        /// <summary>
        /// ユーザー情報読込 Main
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        [STAThread]
        public static void Main(string[] args)
        {
            InitializeLogger();

            // 処理日時取得
            DateTime dteSysDate = DateTime.Now;
            Console.WriteLine(dteSysDate.ToString());
            logger.Log(LogLevel.Info, dteSysDate.ToString());

            // コマンドライン引数(Excelファイルパス)のチェック
            UserInfoClass clsUserInfo = new UserInfoClass();
            bool blnReturn = clsUserInfo.chkCommandLine(args, logger);
            if (!blnReturn) 
            {
                return;
            }

            // コマンドライン引数よりExcelファイルパスを取得
            string strExcelPath = args[0];
            bool blnChkFlg = true;
            List<UserMasterEntity> entyUserMaster = new List<UserMasterEntity>();

            // Excel読込を行う
            Console.WriteLine("Excel読込を行う");
            logger.Log(LogLevel.Info, "Excel読込を行う");
            entyUserMaster = ExcelReadClass.Main(strExcelPath, ref blnChkFlg);
            if (!blnChkFlg) 
            {
                // ユーザにパラメータの入力を求めるメッセージを出力
                Console.WriteLine("入力ファイルの内容に誤りがあります。");
                logger.Log(LogLevel.Error, "入力ファイルの内容に誤りがあります。");
                //// プログラム実行後もコンソールを閉じずに表示し続ける
                //Console.Read();
                return;
            }

            // ユーザーマスタへの登録を行う
            Console.WriteLine("ユーザーマスタへの登録を行う");
            logger.Log(LogLevel.Info, "ユーザーマスタへの登録を行う");
            UserMasterRegistClass clsUserMasterRegist = new UserMasterRegistClass();
            bool blnRegist = clsUserMasterRegist.regUserMaster(entyUserMaster, dteSysDate, logger);
            if (!blnRegist)
            {
                // 異常終了
                Console.WriteLine("異常終了");
                logger.Log(LogLevel.Info, "異常終了");
            }
            else 
            {
                // 正常終了
                Console.WriteLine("正常終了：取込み完了しました。");
                logger.Log(LogLevel.Info, "正常終了：取込み完了しました。");
            }
        }

        /// <summary>
        /// NLog設定 Main
        /// </summary>
        private static void InitializeLogger()
        {
            var conf = new LoggingConfiguration();
            //ファイル出力定義
            var file = new FileTarget("file");
            file.Encoding = System.Text.Encoding.Unicode;
            //file.Layout = "${longdate} [${threadid:padding=2}] [${uppercase:${level:padding=-5}}] ${callsite}() - ${message}${exception:format=ToString}";
            file.Layout = "${longdate} [${uppercase:${level:padding=-5}}] ${callsite}() ${aspnet-user-identity} - ${message}${exception:format=ToString}";
            file.FileName = AppDomain.CurrentDomain.BaseDirectory + "logs/user_upload_{#}.log";
            file.ArchiveNumbering = ArchiveNumberingMode.Rolling;
            file.ArchiveFileName = AppDomain.CurrentDomain.BaseDirectory + "logs/user_upload.log.{#}";
            file.ArchiveEvery = FileArchivePeriod.None;
            file.MaxArchiveFiles = 10;
            conf.AddTarget(file);
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Fatal, file));
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, file));
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Warn, file));
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Info, file));
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Debug, file));
            conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Trace, file));

            ////イベントログ出力定義 ※ただし初回は管理者として実行しないとSourceの登録ができない
            //var eventlog = new EventLogTarget("eventlog");
            //eventlog.Layout = "${message}${newline}${exception:format=ToString}";
            //eventlog.Source = "NLogNoConfigSample";
            //eventlog.Log = "Application";
            //eventlog.EventId = "1001";
            //conf.AddTarget(eventlog);
            //conf.LoggingRules.Add(new LoggingRule("*", LogLevel.Error, eventlog));

            // 設定を反映する
            LogManager.Configuration = conf;
        }
    }
}