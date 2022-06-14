/*
 * Copyright (c) SystemGear.co.,ltd. All rights reserved.
 */

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Batch.Models.Entity
{
    /// <summary>
    /// ログインユーザーマスタ DataEntityModel
    /// </summary>
    public class UserMasterEntity
    {
        /// <summary>
        /// ユーザーID
        /// </summary>
        public string? UserId { get; set; }

        /// <summary>
        /// 所属部署
        /// </summary>
        public string? Department { get; set; }

        /// <summary>
        /// ユーザー名
        /// </summary>
        public string? UserName { get; set; }

        /// <summary>
        /// パスワード
        /// </summary>
        public string? Password { get; set; }

        /// <summary>
        /// 削除フラグ
        /// </summary>
        public int? DelFlg { get; set; }
    }
}