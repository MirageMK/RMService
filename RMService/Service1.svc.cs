using System;
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

        public List<Item> getAllItems()
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
    }
}
