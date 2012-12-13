using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.IO;
using System.Configuration;

using MySql.Data.MySqlClient;

namespace AnalysisReporter
{
    public class ReporterModule : IHttpModule
    {
        static readonly string mConnectString = String.Empty;

        static ReporterModule()
        {
            mConnectString = ConfigurationManager.ConnectionStrings["test_log"].ConnectionString;
        }

        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.PreRequestHandlerExecute += PreRequestHandlerExecute;
        }

        void PreRequestHandlerExecute(object sender, EventArgs e)
        {
            HttpApplication application = sender as HttpApplication;
            string flag = application.Request.Headers.Get("analysis");
            if (!String.IsNullOrEmpty(flag))
            {
                string requestType = application.Request.RequestType.ToUpper();
                var dicForm = new Dictionary<string, object>();
                if (requestType == "POST")
                {
                    foreach (string key in application.Request.Form.AllKeys)
                    {
                        if (!dicForm.ContainsKey(key))
                            dicForm.Add(key, application.Request.Form[key]);
                    }
                }
                else if (requestType == "GET")
                {
                    foreach (string key in application.Request.QueryString.AllKeys)
                    {
                        if (!dicForm.ContainsKey(key))
                            dicForm.Add(key, application.Request.QueryString[key]);
                    }
                }
                dicForm.Add("RawUrl", application.Request.RawUrl);
                dicForm.Add("Browser", application.Request.Browser.Browser);
                WriteLogData(dicForm);
            }
        }

        void WriteLogData(Dictionary<string ,object> data)
        {
            MySqlConnection connect = new MySqlConnection(mConnectString);
            MySqlCommand command = new MySqlCommand();

            try
            {
                connect.Open();
                command.Connection = connect;
                command.CommandText = String.Format("Insert Into test_long(Url, Browser) Values('{0}','{1}')",
                    data["RawUrl"], data["Browser"]);
                command.ExecuteNonQuery();
            }
            catch
            {
            }
            finally
            {
                if (connect.State != System.Data.ConnectionState.Closed)
                    connect.Clone();
                connect.Dispose();
                command.Dispose();
            }
        }
    }
}
