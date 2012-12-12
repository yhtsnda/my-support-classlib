using System;
using System.Collections.Specialized;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

namespace Warehouse.RecordProcessor
{
    /// <summary>
    /// 记录数据
    /// </summary>
    public class RecrodData
    {
        private string mRecorderFlag;
        private NameValueCollection mHeader;
        private NameValueCollection mBody;

        /// <summary>
        /// 记录者的标识
        /// </summary>
        public string RecorderFlag { get { return mRecorderFlag; } }

        /// <summary>
        /// HTTPRequest的Header信息
        /// </summary>
        public NameValueCollection Header { get { return mHeader; } }

        /// <summary>
        /// HTTPRequest的Body信息
        /// </summary>
        public NameValueCollection Body { get { return mBody; } }

        /// <summary>
        /// 记录数据
        /// </summary>
        public RecrodData(string flag, NameValueCollection header, NameValueCollection body)
        {
            this.mRecorderFlag = flag;
            this.mHeader = header;
            this.mBody = body;
        }

        /// <summary>
        /// 记录数据
        /// </summary>
        /// <param name="originalData">原始请求数据</param>
        internal RecrodData(string originalData)
        {
            var data = GetRecordData(originalData);
            this.mRecorderFlag = data.RecorderFlag;
            this.mHeader = data.Header;
            this.mBody = data.Body;
        }

        public virtual string Serialization()
        {
            var serializeData = JsonConvert.SerializeObject(this);
            return serializeData;
        }

        /// <summary>
        /// 将传入的数据转换为可记录的数据实例
        /// </summary>
        /// <param name="requestData"></param>
        /// <returns></returns>
        internal virtual RecrodData GetRecordData(string originalData)
        {
            try
            {
                var data = JsonConvert.DeserializeObject<RecrodData>(originalData);
                return data;    
            }
            catch (Exception)
            {
                throw new Exception("无法将原始数据反序列化为RecorderData,请检查格式");
            }
        }
    }
}
