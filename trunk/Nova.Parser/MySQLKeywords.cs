using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Nova.Parser
{
    internal class MySQLKeywords
    {
        private static MySQLKeywords instance;
        private Dictionary<string, MySQLToken> keywords = new Dictionary<string, MySQLToken>();

        public static MySQLKeywords Instance
        {
            get
            {
                if (instance == null)
                    instance = new MySQLKeywords();
                return instance;
            }
        }

        private MySQLKeywords()
        {
            foreach (string word in Enum.GetNames(typeof(MySQLToken)))
            {
                if (word.StartsWith("KW_"))
                {
                    string kw = word.Substring("KW_".Length);
                    keywords.Add(kw, (MySQLToken)Enum.Parse(typeof(MySQLToken), word));
                }
            }

            keywords.Add("NULL", MySQLToken.LITERAL_NULL);
            keywords.Add("FALSE", MySQLToken.LITERAL_BOOL_FALSE);
            keywords.Add("TRUE", MySQLToken.LITERAL_BOOL_TRUE);
        }

        public MySQLToken GetKeyword(string keyUpperCase)
        {
            MySQLToken token;
            if (keywords.TryGetValue(keyUpperCase, out token))
                return token;
            return MySQLToken.NONE;
        }
    }
}
