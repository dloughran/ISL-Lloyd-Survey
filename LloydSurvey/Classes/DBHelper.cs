using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Helpers;
using System.Web.Script.Serialization;
using System.Data;
using System.Data.SqlClient;
using System.Xml;
using System.Configuration;


namespace LloydSurvey.Classes
{
    public class DBHelper
    {
        public string ConnectionString { get; set; }
        public string LastDBError { get; set; }

        public DBHelper()
        {
            string activeConnectionStringName = ConfigurationManager.AppSettings["ConnectionStringName"];
            this.ConnectionString = ConfigurationManager.ConnectionStrings[activeConnectionStringName].ConnectionString;
            this.LastDBError = "No DB Errors";
        }

        public string writeSurveyResponse(QuestionPageOneModel p1Model, MapModel mapModel, QuestionPageTwoModel p2Model)
        {
            //Create general response information
            string sessionid = HttpContext.Current.Session.SessionID;
            string useragent = HttpContext.Current.Request.UserAgent;
            string ipv4 = HttpContext.Current.Request.UserHostAddress;
            DateTime responseDateTime = DateTime.Now;

            XmlDocument SurveyXml = XMLHelper.GetSurveyXml(p1Model, mapModel, p2Model);
            
            //Open DB Connection
            SqlConnection conn = new SqlConnection(this.ConnectionString);
            string sql = "INSERT INTO response (sessionid, clientipv4address, clientuseragent, submissiondatetime, responsexml) VALUES (@sessionid, @ipaddress, @useragent, @submissiondatetime, @responsexml)";
            SqlCommand cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@sessionid", sessionid);
            cmd.Parameters.AddWithValue("@ipaddress", ipv4);
            cmd.Parameters.AddWithValue("@useragent", useragent);
            cmd.Parameters.AddWithValue("@submissiondatetime", responseDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            cmd.Parameters.AddWithValue("@responsexml", SurveyXml.InnerXml);
            //cmd.Parameters.AddWithValue("@sessionid", "sid");
            //cmd.Parameters.AddWithValue("@ipaddress", "100.100.100.100");
            //cmd.Parameters.AddWithValue("@useragent", "useragent");
            //cmd.Parameters.AddWithValue("@submissiondatetime", "2015-06-15 07:07:07");
            //cmd.Parameters.AddWithValue("@responsexml", null);
            try
            {
                conn.Open();
                cmd.ExecuteNonQuery();
            }
            catch (System.Data.SqlClient.SqlException ex)
            {
                // TODO: Log Errors
                this.LastDBError = String.Format("Exception:{0}{1} -- InnerException:{2}", ex.ErrorCode, ex.Message, ex.InnerException.Message);
            }
            finally
            {
                conn.Close();
            }
            return LastDBError;
        }
    }
}