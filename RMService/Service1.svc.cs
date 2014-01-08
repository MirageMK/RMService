using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;

namespace RMService
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
    // NOTE: In order to launch WCF Test Client for testing this service, please select Service1.svc or Service1.svc.cs at the Solution Explorer and start debugging.
    public class Service1 : IService1
    {
        string cString = ConfigurationManager.AppSettings["SQLSERVER_CONNECTION_STRING"];

        public List<Group> getAllGroups()
        {
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
            WebOperationContext.Current.OutgoingResponse.Headers.Add(
  "Access-Control-Allow-Origin", "*"); WebOperationContext.Current.OutgoingResponse.Headers.Add(
  "Access-Control-Allow-Methods", "POST"); WebOperationContext.Current.OutgoingResponse.Headers.Add(
  "Access-Control-Allow-Headers", "Content-Type, Accept"); 

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

        public Group getGroup(String gKey)
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
    }
}
