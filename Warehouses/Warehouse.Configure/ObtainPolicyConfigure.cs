﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;

namespace Warehouse.Configure
{
    public class ObtainPolicyConfigure : PolicyConfigureBase
    {
        /// <summary>
        /// 是否使用缓存
        /// </summary>
        public bool UseCache { get; set; }

        /// <summary>
        /// 获取记录的方式
        /// </summary>
        public CommandType ObtainCommandType { get; set; }

        /// <summary>
        /// 读取获取数据的策略
        /// </summary>
        public override void Load()
        {
        }

        /// <summary>
        /// 保存存储数据的策略
        /// </summary>
        public override void Save()
        {

        }
    }
}
