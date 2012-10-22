
namespace Projects.Tool.WeblogProvider
{
    /// <summary>
    /// 应用实体类
    /// </summary>
    internal class AppEntity
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 应用标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 应用地址
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// 应用描述
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// 需要记录
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// 是否开启通知
        /// </summary>
        public bool IsNotified { get; set; }

        /// <summary>
        /// 通知邮件
        /// </summary>
        public string PhoneNumbers { get; set; }

        /// <summary>
        /// 通知邮件
        /// </summary>
        public string Emails { get; set; }
    }
}
