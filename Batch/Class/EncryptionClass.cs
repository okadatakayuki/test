/*
 * Copyright (c) SystemGear.co.,ltd. All rights reserved.
 */

using System;
using System.Security.Cryptography;
using System.Text;

namespace Batch.Class
{
    /// <summary>
    /// 暗号化 Class
    /// </summary>
    public class EncryptionClass
    {
        // SHA256暗号化定義
        static readonly HashAlgorithm hashAlgorithm = SHA256.Create();

        /// <summary>
        /// 暗号化 Main
        /// </summary>
        /// <param name="strSourceData">暗号化対象の文字列</param>
        /// <param name="cache">Redis server</param>
        /// <returns>暗号化した文字列</returns>
        public static string Main(string strSourceData) 
        {
            byte[] bteSource;
            byte[] bteHash;

            // 暗号化対象の文字列をbyte変換
            bteSource = ASCIIEncoding.ASCII.GetBytes(strSourceData);

            // byte変換した暗号化対象の文字列をhash変換
            bteHash = hashAlgorithm.ComputeHash(bteSource);

            // 暗号化した文字を16進の文字列に変換
            string strEncryption = chgByteArrayToString(bteHash);

            return strEncryption;
        }

        /// <summary>
        /// 暗号化した文字を16進の文字列に変換
        /// </summary>
        /// <param name="arrInput">暗号化した文字列</param>
        /// <returns>16進に変換した文字列</returns>
        private static string chgByteArrayToString(byte[] arrInput) 
        {
            int intCnt;
            StringBuilder sOutput = new StringBuilder(arrInput.Length);

            // 暗号化した文字を16進の文字列に変換
            for (intCnt = 0; intCnt < arrInput.Length - intCnt; intCnt++) 
            {
                sOutput.Append(arrInput[intCnt].ToString("X2"));
            }
            return sOutput.ToString();
        }
    }
}