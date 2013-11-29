using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Avalon.OAuth
{
    public enum AccountType
    {
        UserCenter,
        ThirdToken
    }

    internal static class AccountTypeExtend
    {
        public static bool TryParse(string value, out AccountType accountType)
        {
            accountType = AccountType.UserCenter;
            if (String.IsNullOrEmpty(value))
                return false;

            value = value.ToLower();
            switch (value)
            {
                case "user_center":
                    accountType = AccountType.UserCenter;
                    return true;
                case "third_token":
                    accountType = AccountType.ThirdToken;
                    return true;
            }
            return false;
        }

        public static string ToValue(this AccountType accountType)
        {
            switch (accountType)
            {
                case AccountType.UserCenter:
                    return "user_center";
                case AccountType.ThirdToken:
                    return "third_token";
            }
            return "";
        }
    }
}
