using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Web;

namespace RMService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        string cString = ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"];
        string secret = "BkMKIxc9wMWZl6nZCLbs+VRousiwHt+w";
        string sid = "ms-app://s-1-15-2-2945773104-2861258875-584576698-1826756059-3853356979-917734085-2567172873";

        private void fixCORS()
        {
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Origin", "*");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Methods", "POST");
            WebOperationContext.Current.OutgoingResponse.Headers.Add("Access-Control-Allow-Headers", "Content-Type, Accept");
        }

        public List<Group> getAllGroups()
        {
            fixCORS();

            List<Group> toReturn = new List<Group>();

            SqlConnection connection = new SqlConnection(cString);
            string sqlString = "SELECT * FROM mGroup";
            SqlCommand cmd = new SqlCommand(sqlString, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Group g = new Group();
                    g.ID = Int32.Parse(reader[0].ToString());
                    g.key = reader[1].ToString();
                    g.title = reader[2].ToString();
                    g.subtitle = reader[3].ToString();
                    g.backgroundImage = reader[4].ToString();
                    g.description = reader[5].ToString();

                    toReturn.Add(g);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Group g = new Group();
                g.ID = -1;
                g.title = e.ToString();

                toReturn.Add(g);
                return toReturn;
            }
            finally
            {
                connection.Close();
            }
            return toReturn;
        }

        public Hashtable getAllSettings()
        {
            fixCORS();

            Hashtable toReturn = new Hashtable();

            SqlConnection connection = new SqlConnection(cString);
            string sqlString = "SELECT * FROM mGeneral";
            SqlCommand cmd = new SqlCommand(sqlString, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    String key = reader[1].ToString();
                    String value = reader[2].ToString();

                    toReturn.Add(key, value);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                toReturn.Add("null", e.ToString());
                return toReturn;
            }
            finally
            {
                connection.Close();
            }
            return toReturn;
        }

        public List<Item> getAllItems(string time)
        {
            fixCORS();

            List<Item> toReturn = new List<Item>();

            SqlConnection connection = new SqlConnection(cString);
            string sqlString = "SELECT * FROM mItem";
            SqlCommand cmd = new SqlCommand(sqlString, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Item i = new Item();
                    i.ID = Int32.Parse(reader[0].ToString());
                    i.group = getGroup(reader[1].ToString());
                    i.title = reader[2].ToString();
                    i.subtitle = reader[3].ToString();
                    i.description = reader[4].ToString();
                    i.content = reader[5].ToString();
                    i.backgroundImage = reader[6].ToString();

                    toReturn.Add(i);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Item i = new Item();
                i.ID = -1;
                i.title = e.ToString();

                toReturn.Add(i);
                return toReturn;
            }
            finally
            {
                connection.Close();
            }
            return toReturn;
        }

        public List<Item> getAllItemsByGroup(string group)
        {
            fixCORS();

            List<Item> toReturn = new List<Item>();

            SqlConnection connection = new SqlConnection(cString);
            string sqlString = "SELECT * FROM mItem WHERE [group] = @group";
            SqlCommand cmd = new SqlCommand(sqlString, connection);

            cmd.Parameters.AddWithValue("group", group);
            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    Item i = new Item();
                    i.ID = Int32.Parse(reader[0].ToString());
                    i.group = getGroup(reader[1].ToString());
                    i.title = reader[2].ToString();
                    i.subtitle = reader[3].ToString();
                    i.description = reader[4].ToString();
                    i.content = reader[5].ToString();
                    i.backgroundImage = reader[6].ToString();

                    toReturn.Add(i);
                }
                reader.Close();
            }
            catch (Exception e)
            {
                Item i = new Item();
                i.ID = -1;
                i.title = e.ToString();

                toReturn.Add(i);
                return toReturn;
            }
            finally
            {
                connection.Close();
            }
            return toReturn;
        }

        public String insertItem(Item item)
        {
            fixCORS();

            String result;

            SqlConnection connection = new SqlConnection(cString);
            string sqlString = "DECLARE @max int " +
                                "SET @max = (SELECT max(id) FROM mItem) " +

                                "INSERT INTO mItem " +
                                "VALUES (@max+1, " +
                                "@group, " +
                                "@title, " +
                                "@price, " +
                                "@description, " +
                                "@content, " +
                                "@backgroundImage)";
            SqlCommand cmd = new SqlCommand(sqlString, connection);

            cmd.Parameters.AddWithValue("group", item.group.key);
            cmd.Parameters.AddWithValue("title", item.title);
            cmd.Parameters.AddWithValue("price", item.subtitle);
            cmd.Parameters.AddWithValue("description", item.description);
            cmd.Parameters.AddWithValue("content", item.content);
            cmd.Parameters.AddWithValue("backgroundImage", item.backgroundImage);

            try
            {
                connection.Open();
                result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public String updateItem(Item item)
        {
            fixCORS();

            String result;

            SqlConnection connection = new SqlConnection(cString);
            string sqlString = "UPDATE mItem" +
                                "SET group = @group, " +
                                "title = @title, " +
                                "price = @price, " +
                                "description = @description, " +
                                "content = @content, " +
                                "backgroundImage = @backgroundImage" +
                                "WHERE id = @id";

            SqlCommand cmd = new SqlCommand(sqlString, connection);

            cmd.Parameters.AddWithValue("group", item.group.key);
            cmd.Parameters.AddWithValue("title", item.title);
            cmd.Parameters.AddWithValue("price", item.subtitle);
            cmd.Parameters.AddWithValue("description", item.description);
            cmd.Parameters.AddWithValue("content", item.content);
            cmd.Parameters.AddWithValue("backgroundImage", item.backgroundImage);
            cmd.Parameters.AddWithValue("id", item.ID);

            try
            {
                connection.Open();
                result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        private Group getGroup(String gKey)
        {
            Group g = new Group();

            SqlConnection connection = new SqlConnection(cString);
            string sqlString = "SELECT * FROM mGroup WHERE [key] = '" + gKey + "'";
            SqlCommand cmd = new SqlCommand(sqlString, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    g.ID = Int32.Parse(reader[0].ToString());
                    g.key = reader[1].ToString();
                    g.title = reader[2].ToString();
                    g.subtitle = reader[3].ToString();
                    g.backgroundImage = reader[4].ToString();
                    g.description = reader[5].ToString();
                }
                reader.Close();
            }
            catch (Exception e)
            {
                g.ID = -1;
                g.title = e.ToString();

                return g;
            }
            finally
            {
                connection.Close();
            }
            return g;
        }

        public String setLogo(String logo)
        {
            fixCORS();

            String result;

            SqlConnection connection = new SqlConnection(cString);

            string sqlString = "UPDATE mGeneral " +
                                "SET value = @logo " +
                                "WHERE [key] = 'logo'";

            SqlCommand cmd = new SqlCommand(sqlString, connection);

            cmd.Parameters.AddWithValue("logo", logo);

            try
            {
                connection.Open();
                result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public String setPassword(String password)
        {
            fixCORS();

            String result;

            SqlConnection connection = new SqlConnection(cString);

            string sqlString = "UPDATE mGeneral " +
                                "SET value = @password " +
                                "WHERE [key] = 'password'";

            SqlCommand cmd = new SqlCommand(sqlString, connection);

            cmd.Parameters.AddWithValue("password", password);

            try
            {
                connection.Open();
                result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public String setName(String name)
        {
            fixCORS();

            String result;

            SqlConnection connection = new SqlConnection(cString);

            string sqlString = "UPDATE mGeneral " +
                                "SET value = @name " +
                                "WHERE [key] = 'name'";

            SqlCommand cmd = new SqlCommand(sqlString, connection);

            cmd.Parameters.AddWithValue("name", name);

            try
            {
                connection.Open();
                result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public String setNotificationChannel(String notificationChannel)
        {
            fixCORS();

            String result;

            SqlConnection connection = new SqlConnection(cString);

            string sqlString = "UPDATE mGeneral " +
                                "SET value = @notificationChannel " +
                                "WHERE [key] = 'notificationChannel'";

            SqlCommand cmd = new SqlCommand(sqlString, connection);

            cmd.Parameters.AddWithValue("notificationChannel", notificationChannel);

            try
            {
                connection.Open();
                result = cmd.ExecuteNonQuery().ToString();
            }
            catch (Exception e)
            {
                return e.ToString();
            }
            finally
            {
                connection.Close();
            }

            return result;
        }

        public String sendPushNotification()
        {
            fixCORS();

            String uri = null;

            SqlConnection connection = new SqlConnection(cString);
            string sqlString = "SELECT * FROM mGeneral WHERE [key]='notificationChannel'";
            SqlCommand cmd = new SqlCommand(sqlString, connection);

            try
            {
                connection.Open();
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    uri = reader[2].ToString();
                }
                reader.Close();
            }
            catch (Exception e)
            {
                uri = null;
            }
            finally
            {
                connection.Close();
            }

            if (uri != null)
            {
                string xml = "<toast><visual><binding template='ToastText01'><text id='1'>bodyText</text></binding></visual></toast>";
                return PostToWns(secret, sid, uri, xml, "wns/toast", "text/xml");
            }
            return "1";
        }

        #region OAuthToken
        // Post to WNS
        public string PostToWns(string secret, string sid, string uri, string xml, string notificationType, string contentType)
        {
            try
            {
                // You should cache this access token.
                var accessToken = GetAccessToken(secret, sid);

                byte[] contentInBytes = Encoding.UTF8.GetBytes(xml);

                HttpWebRequest request = HttpWebRequest.Create(uri) as HttpWebRequest;
                request.Method = "POST";
                request.Headers.Add("X-WNS-Type", notificationType);
                request.ContentType = contentType;
                request.Headers.Add("Authorization", String.Format("Bearer {0}", accessToken.AccessToken));

                using (Stream requestStream = request.GetRequestStream())
                    requestStream.Write(contentInBytes, 0, contentInBytes.Length);

                using (HttpWebResponse webResponse = (HttpWebResponse)request.GetResponse())
                    return webResponse.StatusCode.ToString();
            }
            catch (WebException webException)
            {
                string exceptionDetails = webException.Response.Headers["WWW-Authenticate"];
                if (exceptionDetails.Contains("Token expired"))
                {
                    GetAccessToken(secret, sid);

                    // We suggest that you implement a maximum retry policy.
                    return PostToWns(uri, xml, secret, sid, notificationType, contentType);
                }
                else
                {
                    // Log the response
                    return "EXCEPTION: " + webException.Message;
                }
            }
            catch (Exception ex)
            {
                return "EXCEPTION: " + ex.Message;
            }
        }

        // Authorization
        [DataContract]
        public class OAuthToken
        {
            [DataMember(Name = "access_token")]
            public string AccessToken { get; set; }
            [DataMember(Name = "token_type")]
            public string TokenType { get; set; }
        }

        private OAuthToken GetOAuthTokenFromJson(string jsonString)
        {
            using (var ms = new MemoryStream(Encoding.Unicode.GetBytes(jsonString)))
            {
                var ser = new DataContractJsonSerializer(typeof(OAuthToken));
                var oAuthToken = (OAuthToken)ser.ReadObject(ms);
                return oAuthToken;
            }
        }

        protected OAuthToken GetAccessToken(string secret, string sid)
        {
            var urlEncodedSecret = HttpUtility.UrlEncode(secret);
            var urlEncodedSid = HttpUtility.UrlEncode(sid);

            var body = String.Format("grant_type=client_credentials&client_id={0}&client_secret={1}&scope=notify.windows.com",
                                     urlEncodedSid,
                                     urlEncodedSecret);

            string response;
            using (var client = new WebClient())
            {
                client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
                response = client.UploadString("https://login.live.com/accesstoken.srf", body);
            }
            return GetOAuthTokenFromJson(response);
        }

        #endregion
    }
}
