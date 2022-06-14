/*
 * Copyright (c) SystemGear.co.,ltd. All rights reserved.
 */

using Batch.Class;
using Batch.Models.Entity;
using NPOI.SS.UserModel;
using System;
using System.Text;
using System.Runtime.InteropServices;

namespace Batch.Class
{
    /// <summary>
    /// Excel読込 Class
    /// </summary>
    public class ExcelReadClass
    {
        /// <summary>
        /// Excel読込 Main
        /// </summary>
        /// <param name="strExcelPath">暗号化した文字列</param>
        /// <param name="blnChkFlg">エラーチェックフラグ</param>
        /// <returns>ログインユーザーマスタ DataEntityModel</returns>
        public static List<UserMasterEntity> Main(string strExcelPath, ref bool blnChkFlg) 
        {
            List<UserMasterEntity> entyUserMaster = new List<UserMasterEntity>();

            // （WorkbookFactory.Create()を使ってinterfaceで受け取れば、xls, xlsxの両方に対応できます）
            // WorkBookの取得
            IWorkbook workbook = WorkbookFactory.Create(strExcelPath);

            // WorkSheetの取得
            ISheet worksheet = workbook.GetSheetAt(0);

            // WorkSheet最終行の設定
            int intlastRow = worksheet.LastRowNum;

            UserInfoClass clsUserInfo = new UserInfoClass();
            blnChkFlg = true;

            // WorkSheetの読込
            for (int intCnt = 1; intCnt <= intlastRow; intCnt++)
            {
                UserMasterEntity recdUserMaster = new UserMasterEntity();

                // WorkSheetの行を取得
                IRow row = worksheet.GetRow(intCnt);

                // WorkSheetの行項目を取得
                // ユーザーID
                ICell cell = row.GetCell(0);
                string? strUserId = cell?.StringCellValue;
                blnChkFlg = clsUserInfo.chkChar(strUserId, true, 254);
                if (blnChkFlg)
                {
                    blnChkFlg = clsUserInfo.chkCharCustomUserId(strUserId);
                }
                if (!blnChkFlg) 
                {
                    break;
                }
                recdUserMaster.UserId = strUserId;

                // 所属部署
                cell = row.GetCell(1);
                string? strDepartment = cell?.StringCellValue;
                blnChkFlg = clsUserInfo.chkChar(strDepartment, true, 128);
                if (!blnChkFlg)
                {
                    break;
                }
                recdUserMaster.Department = strDepartment;

                // ユーザー名
                cell = row.GetCell(2);
                string? strUserName = cell?.StringCellValue;
                blnChkFlg = clsUserInfo.chkChar(strUserName, true, 128);
                if (!blnChkFlg)
                {
                    break;
                }
                recdUserMaster.UserName = strUserName;

                // パスワード
                cell = row.GetCell(3);
                string? strPassword = cell?.StringCellValue;
                blnChkFlg = clsUserInfo.chkChar(strPassword, true, 32, 8);
                if (blnChkFlg)
                {
                    blnChkFlg = clsUserInfo.chkCharCustomPassword(strPassword);
                }
                if (!blnChkFlg)
                {
                    break;
                }
                recdUserMaster.Password = strPassword;

                // 削除フラグ
                cell = row.GetCell(4);
                string? strDelFlg = cell?.StringCellValue;
                blnChkFlg = clsUserInfo.chkChar(strDelFlg, false, 1);
                if (!blnChkFlg)
                {
                    break;
                }
                if ((strDelFlg != null) && (strDelFlg.Length > 0))
                {
                    int intDelFlg = 0;
                    if (int.TryParse(strDelFlg, out intDelFlg))
                    {
                        recdUserMaster.DelFlg = intDelFlg;
                    }
                    else
                    {
                        break;
                    }
                }

                entyUserMaster.Add(recdUserMaster);
            }

            return entyUserMaster;
        }
    }
}