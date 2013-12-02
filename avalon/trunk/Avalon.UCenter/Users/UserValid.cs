using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Avalon.UCenter
{
    public static class UserValid
    {
        static Regex regUserName = new Regex(@"^[A-Za-z0-9\-\@\._]+$", RegexOptions.Compiled);//只允许数字，字母，-、.、@、_等字符
        static Regex regUserNameNum = new Regex(@"^[0-9]+$", RegexOptions.Compiled);//不能全是数字
        static Regex regEmail = new Regex(@"^([\w-\.]+)@((\[[0-9]{1,3}\.[0-9]{1,3}\.[0-9]{1,3}\.)|(([\w-]+\.)+))([a-zA-Z]{2,4}|[0-9]{1,3})(\]?)$", RegexOptions.Compiled);
        static Regex regPassword = new Regex(@"^[A-Za-z0-9]+$", RegexOptions.Compiled);

        /// <summary>
        /// 验证用户名
        /// </summary>
        /// <remarks>
        /// 规则
        /// 1、长度在5-50
        /// 2、仅能 A-Za-z0-9\-\@\._ 字符内
        /// 3、不能全部为数字（预留给手机使用）
        /// 4、不能重复
        /// </remarks>
        public static RegisterResultCode ValidUserName(string userName)
        {
            if (String.IsNullOrEmpty(userName))
                return RegisterResultCode.EmptyUserName;

            if (userName.Length < 5 || userName.Length > 50)
                return RegisterResultCode.InvalidUserNameLength;

            if (!regUserName.IsMatch(userName) || regUserNameNum.IsMatch(userName))
                return RegisterResultCode.InvalidUserName;

            return RegisterResultCode.Success;
        }

        public static bool ValidUserName(string userName, out RegisterResultCode code)
        {
            code = ValidUserName(userName);
            return code == RegisterResultCode.Success;
        }

        /// <summary>
        /// 验证昵称
        /// </summary>
        /// <remarks>
        /// 规则
        /// 1、长度在3-20
        /// 2、不能重复
        /// </remarks>
        public static RegisterResultCode ValidNickName(string nickname)
        {
            if (String.IsNullOrEmpty(nickname))
                return RegisterResultCode.EmptyNickName;

            var nickNameLength = GetChineseLength(nickname);
            if (nickname.Contains("third_") || nickname.Contains("@591up.com"))
            {
                if (nickNameLength < 3 || nickNameLength > 50)
                    return RegisterResultCode.InvalidNickNameLength;
            }
            else
            {
                if (nickNameLength < 3 || nickNameLength > 20)
                    return RegisterResultCode.InvalidNickNameLength;
            }

            return RegisterResultCode.Success;
        }

        public static bool ValidNickName(string nickname, out RegisterResultCode code)
        {
            code = ValidNickName(nickname);
            return code == RegisterResultCode.Success;
        }

        /// <summary>
        /// 验证密码（md5）
        /// </summary>
        /// <remarks>
        /// 规则
        /// 1、长度在7-12
        /// 2、仅能 A-Za-z0-9 字符内
        /// </remarks>
        public static RegisterResultCode ValidPassword(string password)
        {
            if (String.IsNullOrEmpty(password))
                return RegisterResultCode.EmptyPassword;

            if (password.Length < 7 || password.Length > 12)
                return RegisterResultCode.InvalidPasswordLength;

            if (!regPassword.IsMatch(password))
                return RegisterResultCode.InvalidPassword;

            return RegisterResultCode.Success;
        }

        public static bool ValidPassword(string password, out RegisterResultCode code)
        {
            code = ValidPassword(password);
            return code == RegisterResultCode.Success;
        }

        public static bool IsEmail(string email)
        {
            return !String.IsNullOrEmpty(email) && regEmail.IsMatch(email);
        }

        public static RegisterResultCode ValidEmail(string email)
        {
            if (!String.IsNullOrEmpty(email) && !regEmail.IsMatch(email))
                return RegisterResultCode.InvalidEmail;

            return RegisterResultCode.Success;
        }

        public static bool ValidEmail(string email, out RegisterResultCode code)
        {
            code = ValidEmail(email);
            return code == RegisterResultCode.Success;
        }

        static int GetChineseLength(string str)
        {
            int length = 0;
            char[] q = str.ToCharArray();
            for (int i = 0; i < q.Length; i++)
            {
                if ((int)q[i] >= 0x4E00 && (int)q[i] <= 0x9FA5)//是否汉字,汉字算两个字符
                    length += 2;
                else
                    length += 1;
            }
            return length;
        }
    }
}
