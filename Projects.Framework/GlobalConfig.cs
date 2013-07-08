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

        /// <summary>
        /// 用户中心地址
        /// </summary>
        public readonly static string UserSiteUrl = GetAppSetting("Url.User");

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

        public readonly static string UploadFileRetrievePasswordVoucherPath = GetAppSetting("UploadFile.RetrievePassword");

        /// <summary>
        /// 上传文件映射目录下的一级目录（用于持久化地址和访问）
        /// </summary>
        public readonly static string UploadFolderPath = GetAppSetting("UploadFile.Folder");

        public readonly static string UploadCourseLogoPath = GetAppSetting("UploadFile.CourseLogo");

        public readonly static string UploadFileActivePath = GetAppSetting("UploadFile.ActivePath");

        /// <summary>
        /// 证书模板存放路径
        /// </summary>
        public readonly static string UploadMasterplatePath = GetAppSetting("UploadFile.Masterplate");

        /// <summary>
        /// 报名信息模板
        /// </summary>
        public readonly static string UploadEnrollTemplatePath = GetAppSetting("UploadFile.EnrollTemplate");

        /// <summary>
        /// 用户报名文件
        /// </summary>
        public readonly static string UploadUserEnrollFilePath = GetAppSetting("UploadFile.UserEnrollFile");

        /// <summary>
        /// 项目手机应用下载地址保存路径
        /// </summary>
        public readonly static string UploadProjectMobileAppPath = GetAppSetting("UploadFile.ProjectMobileApp");

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

        /// <summary>
        /// 允许上传文件的类型
        /// </summary>
        public readonly static string UploadAllowedFileTypes = GetAppSetting("UploadFile.Type");

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

        /// <summary>
        /// 所有课程id
        /// </summary>
        public readonly static string AllCourse = GetAppSetting("allCourse");

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
        /// 支付宝网关地址
        /// </summary>
        public readonly static string AlipayGatewayUrl = GetAppSetting("Alipay.gateway");
        /// <summary>
        /// 支付宝服务器异步通知页面地址
        /// </summary>
        public readonly static string AlipayNotifyUrl = GetAppSetting("Alipay.notifyUrl");
        /// <summary>
        /// 支付宝页面跳转同步通知页面地址
        /// </summary>
        public readonly static string AlipayReturnUrl = GetAppSetting("Alipay.returnUrl");

        /// <summary>
        /// cmsapi
        /// </summary>
        public readonly static string CmsApi = GetAppSetting("cms.api");

        /// <summary>
        /// 不支持264编码的视频格式，以“|”间隔
        /// </summary>
        public readonly static string VideoFomatsNonsupport264 = GetAppSetting("Video.FomatsNonsupport264");

        /// <summary>
        /// 视频上报间隔时间
        /// </summary>
        public readonly static int VideoStatIntervalSeconds = GetAppSettingForInt("Video.StatIntervalSeconds");

        /// <summary>
        /// 文档上报间隔时间
        /// </summary>
        public readonly static int DocumentStatIntervalSeconds = GetAppSettingForInt("Document.StatIntervalSeconds");

        /// <summary>
        /// 视频就绪时间戳
        /// </summary>
        public readonly static int VideoDistributionSeconds = GetAppSettingForInt("Video.DistributionSeconds");

        /// <summary>
        /// 转码中的视频重置间隔（小时）
        /// </summary>
        public readonly static int VideoResetHours = GetAppSettingForInt("Video.ResetHours");

        ///// <summary>
        ///// 临时
        ///// </summary>
        //public readonly static string VideoNDCDNAddress = GetAppSetting("Video.ND.CDN.Address");

        /// <summary>
        /// 文档就绪时间戳
        /// </summary>
        public readonly static int DocumentDistributionSeconds = GetAppSettingForInt("Document.DistributionSeconds");

        public readonly static string UploadFileEsotericaAttachFilePath = GetAppSetting("UploadFile.esotericaAttachFile");
        public readonly static string TyCloudAppendResourceUrl = GetAppSetting("TyCloud.appendResourceUrl");
        public readonly static string TyCloudEditResourceUrl = GetAppSetting("TyCloud.editResourceUrl");
        public readonly static string TyCloudGetContentUrl = GetAppSetting("TyCloud.getContentUrl");

        public readonly static string VideoTypeIds = GetAppSetting("Video.TypeIds");

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
