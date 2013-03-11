using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Configuration;
using Projects.Tool.Util;
using System.IO;

namespace Projects.Framework
{
    /// <summary>
    /// 全局配置
    /// </summary>
    public static class GlobalConfig
    {
        /*
         建议命名
         *  以Path 结尾命名的，代表程序可读写的本地路径
         *  以Url结尾的的路径，表示用户可读的Url地址
         */

        #region url 地址配置区
        /// <summary>
        /// 机构站地址
        /// </summary>
        public readonly static string OrgSiteUrl = GetAppSetting("Url.Org");

        /// <summary>
        /// 新平台的API地址
        /// </summary>
        public readonly static string OpenApiUrl = GetAppSetting("Url.Kc.Api");

        /// <summary>
        /// 主站地址
        /// </summary>
        public readonly static string SiteUrl = GetAppSetting("Url.Site");

        ///// <summary>
        ///// 旧有课程地址 
        ///// </summary>
        //public readonly static string SiteOldCourseUrl = GetAppSetting("Url.Old.Course");

        /// <summary>
        /// 在职教育api验证身份码
        /// </summary>
        public readonly static string SiteChaptcha = GetAppSetting("Site.Chaptcha");

        /// <summary>
        /// 站点标题
        /// </summary>
        public readonly static string SiteTitle = GetAppSetting("Site.Title");

        /// <summary>
        /// 站点是否开放
        /// </summary>
        public readonly static bool SiteOpen = bool.Parse(GetAppSetting("Site.Open"));

        /// <summary>
        /// 开放应用手机下载链接
        /// </summary>
        public readonly static string AppPhoneDownLoadUrl = GetAppSetting("App.Phone");

        /// <summary>
        /// PDF下载路径(该项目没用)
        /// </summary>
        public readonly static string PDFpath = "";

        /// <summary>
        /// 附件FTP地址
        /// </summary>
        public readonly static string AttachmentUrl = GetAppSetting("Resource.AttachmentUrl");

        ///// <summary>
        ///// SEO的stopWord
        ///// </summary>
        //public readonly static string SeoStopWord = GetAppSetting("SEO.StopWord");

        ///// <summary>
        ///// 模考赛图片
        ///// </summary>
        //public readonly static string ActiveUrl = AttachmentUrl + GetAppSetting("UploadFile.Active");

        ///// <summary>
        ///// 任务图片
        ///// </summary>
        //public readonly static string TaskImgUrl = AttachmentUrl + GetAppSetting("UploadFile.Task");

        ///// <summary>
        ///// 成就图片
        ///// </summary>
        //public readonly static string AchievementImgUrl = AttachmentUrl + GetAppSetting("UploadFile.Achievement");

        ///// <summary>
        ///// 充值地址
        ///// </summary>
        //public readonly static string PaySiteUrl = GetAppSetting("Url.Pay");


        /// <summary>
        /// 用户中心地址
        /// </summary>
        public readonly static string UserSiteUrl = GetAppSetting("Url.User");

        ///// <summary>
        ///// 聊天站点
        ///// </summary>
        //public readonly static string ChatSiteUrl = GetAppSetting("Url.Chat");

        ///// <summary>
        ///// 腾讯开放平台Api接口地址
        ///// </summary>
        //public readonly static string QQApiUrl = GetAppSetting("Url.QQApi");

        ///// <summary>
        ///// 腾讯开放平台Api测试接口地址,因为腾讯测试应用不允许用域名访问API
        ///// </summary>
        //public readonly static string QQDebugApiUrl = GetAppSetting("Url.Debug.QQApi");


        #endregion

        #region path 可写路径（本地）配置区

        /// <summary>
        /// 附件本地映射地址
        /// </summary>
        public readonly static string AttachmentPath = FileHelper.MapPath(GetAppSetting("Resource.AttachmentPath"));


        /// <summary>
        /// 文件上传地址
        /// </summary>
        public readonly static string UploadFilePath = GetAppSetting("UploadFile.Path");

        /// <summary>
        /// excel
        /// </summary>
        public readonly static string UploadFileExcelPath = GetAppSetting("UploadFile.Excel");

        /// <summary>
        /// UserLogo
        /// </summary>
        public readonly static string UploadFileUserLogoPath = GetAppSetting("UploadFile.UserLogo");

        /// <summary>
        /// ProjectLogo
        /// </summary>
        public readonly static string UploadFileProjectLogoPath = GetAppSetting("UploadFile.ProjectLogo");

        /// <summary>
        /// RemitVoucher
        /// </summary>
        public readonly static string UploadFileRemitVoucherPath = GetAppSetting("UploadFile.RemitVoucher");

        /// <summary>
        /// 上传文件映射目录下的一级目录（用于持久化地址和访问）
        /// </summary>
        public readonly static string UploadFolderPath = GetAppSetting("UploadFile.Folder");

        public readonly static string UploadFileActivePath = GetAppSetting("UploadFile.ActivePath");

        /// <summary>
        /// 证书模板存放路径
        /// </summary>
        public readonly static string UploadMasterplatePath = GetAppSetting("UploadFile.Masterplate");

        /// <summary>
        /// 上传文件映射目录（供前端用）
        /// </summary>
        public readonly static string UploadPath = AttachmentUrl + UploadFolderPath;

        public readonly static string UploadActiveFolderPath = GetAppSetting("UploadFile.Folder.Active");

        /// <summary>
        /// 应用平台监控文件地址
        /// </summary>
        public readonly static string ThirdAppMoniterFilePath = GetAppSetting("UploadFile.ThirdAppMoniter");

        #endregion

        ///// <summary>
        ///// 是否启用权限验证：没有该配置节默认为启用true
        ///// </summary>
        //public readonly static bool AuthEnable = GetAppSettingForBool("Auth.Enable", false);

        /// <summary>
        /// 允许上传文件的类型
        /// </summary>
        public readonly static string UploadAllowedFileTypes = GetAppSetting("UploadFile.Type");

        ///// <summary>
        ///// 后台模考赛发送短信授权码
        ///// </summary>
        //public readonly static string MessageCode = GetAppSetting("Message.Code");

        //#region EHR配置
        ///// <summary>
        ///// 91EHR考试开始时间
        ///// </summary>
        //public readonly static string EhrBeginTime = GetAppSetting("91EHR.BeginTime");

        ///// <summary>
        ///// 91EHR考试结束时间
        ///// </summary>
        //public readonly static string EhrEndTime = GetAppSetting("91EHR.EndTime");

        ///// <summary>
        ///// 91EHR密钥
        ///// </summary>
        //public readonly static string EhrDesKey = GetAppSetting("91EHR.Key");

        ///// <summary>
        ///// 91EHR注册前缀名
        ///// </summary>
        //public readonly static string EhrRegisterName = GetAppSetting("91EHR.RegisterName");

        ///// <summary>
        ///// 91EHR注册邮箱后缀
        ///// </summary>
        //public readonly static string EhrRegisterEmailName = GetAppSetting("91EHR.RegisterEmailName");

        ///// <summary>
        ///// 91EHR链接验证失败回调地址
        ///// </summary>
        //public readonly static string EhrFailUrl = GetAppSetting("91EHR.FailUrl");

        ///// <summary>
        ///// 91EHR配置UnitId
        ///// </summary>
        //public readonly static string EhrUnitId = GetAppSetting("91EHR.UnitId");
        //#endregion

        //#region 食品安全配置
        ///// <summary>
        ///// FSKQuiz开始时间
        ///// </summary>
        //public readonly static string FSKQuizBeginTime = GetAppSetting("FSKQuiz.BeginTime");

        ///// <summary>
        ///// FSKQuiz考试结束时间
        ///// </summary>
        //public readonly static string FSKQuizEndTime = GetAppSetting("FSKQuiz.EndTime");

        ///// <summary>
        ///// FSKQuiz密钥
        ///// </summary>
        //public readonly static string FSKQuizDesKey = GetAppSetting("FSKQuiz.Key");

        ///// <summary>
        ///// FSKQuiz注册前缀名
        ///// </summary>
        //public readonly static string FSKQuizRegisterName = GetAppSetting("FSKQuiz.RegisterName");

        ///// <summary>
        ///// FSKQuiz注册邮箱后缀
        ///// </summary>
        //public readonly static string FSKQuizRegisterEmailName = GetAppSetting("FSKQuiz.RegisterEmailName");

        ///// <summary>
        ///// FSKQuiz链接验证失败回调地址
        ///// </summary>
        //public readonly static string FSKQuizFailUrl = GetAppSetting("FSKQuiz.FailUrl");

        ///// <summary>
        ///// FSKQuiz配置UnitId
        ///// </summary>
        //public readonly static string FSKQuizUnitId = GetAppSetting("FSKQuiz.UnitId");
        //#endregion

        //#region 建议提交
        ///// <summary>
        ///// 建议提交入口
        ///// </summary>
        //public readonly static string SiteCrm = GetAppSetting("Url.Crm");

        //public readonly static string CrmKeyCode = GetAppSetting("Crm.KeyCode");

        //public readonly static string CrmProjCode = GetAppSetting("Crm.ProjCode");

        //public readonly static string CrmQT = GetAppSetting("Crm.QuestionTypeId");
        //#endregion

        //#region 竞赛相关配置
        //public readonly static int CriticalSeconds = GetAppSettingForInt("Race.CriticalSeconds");
        //#endregion

        ///// <summary>
        ///// 91天铭酒窝社区API接口URL
        ///// </summary>
        //public readonly static string TmApiUrl = GetAppSetting("91Tm.ApiUrl");

        ///// <summary>
        ///// 91天铭酒窝社区API接口WEB FLAG
        ///// </summary>
        //public readonly static string TmWebFlag = GetAppSetting("91Tm.WEB_FLAG");

        ///// <summary>
        ///// 91天铭酒窝社区API接口API KEY
        ///// </summary>
        //public readonly static string TmUapApiKey = GetAppSetting("91Tm.UAP_API_KEY");

        /// <summary>
        /// 默认平台代码
        /// </summary>
        public readonly static long PlatCode = 400110010000;

        /// <summary>
        /// API Url
        /// </summary>
        public readonly static string ApiUrl = GetAppSetting("UapApiUrl");

        /// <summary>
        /// API Post Url
        /// </summary>
        public readonly static string ApiPostUrl = GetAppSetting("UapApiPostUrl");

        /// <summary>
        /// API Key
        /// </summary>
        public readonly static string ApiKey = GetAppSetting("UapApiKey");

        ///// <summary>
        ///// 所有课程id
        ///// </summary>
        //public readonly static string AllCourse = GetAppSetting("allCourse");

        ///// <summary>
        ///// Lucene索引的存放路径
        ///// </summary>
        //public readonly static string LucenePath = GetAppSetting("LuceneIndexPath");

        ///// <summary>
        ///// PanGu文件地址
        ///// </summary>
        //public readonly static string PanGuFile = GetAppSetting("PanGuConfig");

        ///// <summary>
        ///// lucene 接口站
        ///// </summary>
        //public readonly static string SearchSiteUrl = GetAppSetting("Url.Search");

        ///// <summary>
        ///// 页面Keyword
        ///// </summary>
        //public readonly static string PageKeyword = GetAppSetting("Page.Keyword");

        ///// <summary>
        ///// 页面Description
        ///// </summary>
        //public readonly static string PageDescription = GetAppSetting("Page.Description");

        ///// <summary>
        ///// 系统消息的logoUrl
        ///// </summary>
        //public readonly static string SystemLogoUrl = GetAppSetting("SystemLogoUrl");

        ///// <summary>
        ///// 平台型产品开放性接口
        ///// </summary>
        //public readonly static string EPApi = GetAppSetting("EP.API");

        ///// <summary>
        ///// 短信SMS_Sign_Key
        ///// </summary>
        //public readonly static string SMSSignKey = GetAppSetting("SMS.Sign.Key");

        /// <summary>
        /// 资源标识密钥（前缀），完整密钥：SignKey + IP
        /// </summary>
        //public readonly static string SignKey = GetAppSetting("Video.Sign.Key");

        /// <summary>
        /// 不支持264编码的视频格式，以“|”间隔
        /// </summary>
        public readonly static string VideoFomatsNonsupport264 = GetAppSetting("Video.FomatsNonsupport264");

        public readonly static int VideoStatIntervalSeconds = GetAppSettingForInt("Video.StatIntervalSeconds");

        /// <summary>
        /// 视频就绪时间戳
        /// </summary>
        public readonly static int VideoDistributionSeconds = GetAppSettingForInt("Video.DistributionSeconds");

        /// <summary>
        /// 文档就绪时间戳
        /// </summary>
        public readonly static int DocumentDistributionSeconds = GetAppSettingForInt("Document.DistributionSeconds");

        /// <summary>
        /// 快钱的key
        /// </summary>
        public readonly static string BillKey = GetAppSetting("99bill.key");

        /// <summary>
        /// 快钱的bgUrl
        /// </summary>
        public readonly static string BillBgUrl = GetAppSetting("99bill.bgUrl");

        /// <summary>
        /// 快钱缴费结果的展示地址
        /// </summary>
        public readonly static string BillShowUrl = GetAppSetting("99bill.showUrl");

        /// <summary>
        ///快钱的接收款项的人民币账号
        /// </summary>
        public readonly static string BillMerchantAcctId = GetAppSetting("99bill.merchantAcctId");

        /// <summary>
        /// 网关提交地址
        /// </summary>
        public readonly static string BillGateUrl = GetAppSetting("99bill.gateUrl");

        /// <summary>
        /// 公钥证书路径
        /// </summary>
        public readonly static string BillPublicPath = GetAppSetting("99bill.publicPath");

        /// <summary>
        /// 私钥证书路径
        /// </summary>
        public readonly static string BillPrivatePath = GetAppSetting("99bill.privatePath");

        /// <summary>
        /// cmsapi
        /// </summary>
        public readonly static string CmsApi = GetAppSetting("cms.api");

        /// <summary>
        /// 获取设置
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private static string GetAppSetting(string name)
        {
            return ConfigurationManager.AppSettings[name] ?? string.Empty;
        }

        private static int GetAppSettingForInt(string name)
        {
            int v;
            Int32.TryParse(ConfigurationManager.AppSettings[name], out v);
            return v;
        }

        private static bool GetAppSettingForBool(string name, bool defaultVaule)
        {
            bool v;
            if (bool.TryParse(ConfigurationManager.AppSettings[name], out v))
                return v;
            else
                return defaultVaule;
        }
    }
}
