using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.ServiceModel.Web;
using System.Text;
using System.Configuration;
using System.Data.SqlClient;

namespace RMService
{
	// NOTE: You can use the "Rename" command on the "Refactor" menu to change the class name "Service1" in code, svc and config file together.
	public class Service : IService
	{
		/*public string GetData()
		{
			//var uriString = ConfigurationManager.AppSettings["SQLSERVER_URI"];
			var uriString = "sqlserver://jussgvdzfzltpama:Es5VLb46QEH3YWvZ37rtjKm6t6Kpc22xt3dYrdKt4UaSppgxGH2LRjY4f8BNxM3X@0367a0d1-0c21-4942-ac84-a13800a67b43.sqlserver.sequelizer.com/db0367a0d10c214942ac84a13800a67b43";
			var uri = new Uri(uriString);
			var connectionString = new SqlConnectionStringBuilder
			{
				DataSource = uri.Host,
				InitialCatalog = uri.AbsolutePath.Trim('/'),
				UserID = uri.UserInfo.Split(':').First(),
				Password = uri.UserInfo.Split(':').Last(),
			}.ConnectionString;

			SqlConnection myConnection = new SqlConnection(connectionString);

			try
			{
				myConnection.Open();
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			String all = "";
			try
			{
				SqlDataReader myReader = null;
				SqlCommand myCommand = new SqlCommand("select * from Table_1",
														 myConnection);
				myReader = myCommand.ExecuteReader();
				while (myReader.Read())
				{
					String toReturn = myReader["ID"].ToString()+" "+myReader["name"].ToString();
					all += toReturn + "\n";
				}
			}
			catch (Exception e)
			{
				Console.WriteLine(e.ToString());
			}
			//return "TEST";
			return all;
		}

		public CompositeType GetDataUsingDataContract(CompositeType composite)
		{
			if (composite == null)
			{
				throw new ArgumentNullException("composite");
			}
			if (composite.BoolValue)
			{
				composite.StringValue += "Suffix";
			}
			return composite;
		}*/

        string cString = "Server=a1b4e5e1-6911-4f61-af49-a13a0102fc31.sqlserver.sequelizer.com;Database=dba1b4e5e169114f61af49a13a0102fc31;User ID=tzdygywlrfqyvwan;Password=smcHBa5kEmSmPBPxobowhVBAkWPwfQpz2nvoVSeUa5AcpQTnEoRXNLyDRJxNtGfX;";

        public string XMLData(string id)
        {
            return "You requested product " + id;
        }

        public string JSONData(string id)
        {
            return "You requested product " + id;
        }

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
