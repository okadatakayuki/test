/*
 * Copyright (c) SystemGear.co.,ltd. All rights reserved.
 */

using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Batch.Class
{
    /// <summary>
    /// ユーザー情報読込 Class
    /// </summary>
    internal class UserInfoClass
    {
        /// <summary>
        /// コマンドラインのチェック
        /// </summary>
        /// <param name="args">コマンドライン引数</param>
        /// <param name="logger">NLog logger</param>
        /// <returns>true:OK/false:NG</returns>
        public bool chkCommandLine(string[] args, Logger logger) 
        {
            // 引数が1つ以上存在する場合
            if (args.Length > 0)
            {
                if (args.Length == 1)
                {
                    // プログラム実行後もコンソールを閉じずに表示し続ける
                    //Console.Read();
                    return true;
                }
                else
                {
                    // ユーザにパラメータの入力を求めるメッセージを出力
                    Console.WriteLine("引数に誤りがあります。");
                    logger.Log(LogLevel.Error, "引数に誤りがあります。");
                    // プログラム実行後もコンソールを閉じずに表示し続ける
                    //Console.Read();
                    return false;
                }
            }
            // 引数が存在しない場合
            else
            {
                // ユーザにパラメータの入力を求めるメッセージを出力
                Console.WriteLine("引数に誤りがあります。");
                logger.Log(LogLevel.Error, "引数に誤りがあります。");
                // プログラム実行後もコンソールを閉じずに表示し続ける
                Console.Read();
                return false;
            }
        }

        /// <summary>
        /// ユーザーマスタの文字数チェック
        /// </summary>
        /// <param name="strInput">チェック対象の文字列</param>
        /// <param name="blnNullChk">Nullチェック有無</param>
        /// <param name="intByteMax">入力上限桁数</param>
        /// <param name="intByteMin">入力下限桁数</param>
        /// <returns>true:OK/false:NG</returns>
        public bool chkChar(string? strInput, bool blnNullChk, int intByteMax, int intByteMin = 0)
        {
            if (blnNullChk)
            {
                // 入力された文字がNull値、又は長さが0の場合
                if ((strInput == null) || (strInput.Length == 0))
                {
                    return false;
                }
            }
            else 
            {
                // 入力された文字がNull値、又は長さが0の場合
                if ((strInput == null) || (strInput.Length == 0))
                {
                    return true;
                }
            }

            //// ASCII エンコード
            //byte[] bteData = System.Text.Encoding.ASCII.GetBytes(strInput);
            // データがutf-8の場合
            byte[] bteData = System.Text.Encoding.UTF8.GetBytes(strInput);

            // 入力された文字が指定されたByte数以上の場合
            if (bteData.Length > intByteMax)
            {
                return false;
            }

            // 入力された文字が指定されたByte数以下の場合
            if (bteData.Length < intByteMin)
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// ユーザーマスタ.パスワード項目の入力文字チェック
        /// </summary>
        /// <param name="strInput">チェック対象の文字列</param>
        /// <returns>true:OK/false:NG</returns>
        public bool chkCharCustomPassword(string? strInput)
        {
            // 入力された文字がNull値、又は長さが0の場合
            if ((strInput == null) || (strInput.Length == 0))
            {
                return false;
            }

            // 正規表現パターンを指定してRegexオブジェクトを作成
            Regex re = new Regex(@"[\u0021-\u007e]");

            // 正規表現パターンに含まれていない文字が存在する場合はエラー
            for (int intCnt = 0; intCnt < strInput.Length; intCnt++) 
            {
                string strChar = strInput.Substring(intCnt, 1);
                if (!re.IsMatch(strChar))
                {
                    return false;
                }
            }

            return true;
        }

        /// <summary>
        /// ユーザーマスタ.ユーザーID項目の入力文字チェック
        /// </summary>
        /// <param name="strInput">チェック対象の文字列</param>
        /// <returns>true:OK/false:NG</returns>
        public bool chkCharCustomUserId(string? strInput)
        {
            // 入力された文字がNull値、又は長さが0の場合
            if ((strInput == null) || (strInput.Length == 0))
            {
                return false;
            }

            // 正規表現パターンを指定してメールアドレスっぽいか調べる
            if (!System.Text.RegularExpressions.Regex.IsMatch(
                strInput,
                @"\A[A-Z0-9._%+-]+@[A-Z0-9.-]+\.[A-Z]{2,4}\z",
                System.Text.RegularExpressions.RegexOptions.IgnoreCase))
            {
                return false;
            }

            return true;
        }
    }
}
