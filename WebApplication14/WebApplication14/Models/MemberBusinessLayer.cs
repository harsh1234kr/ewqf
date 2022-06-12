using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Web;

namespace WebApplication14.Models
{
    /// <summary>
    /// the class deals with all the connection Member to the system
    /// </summary>
    public class MemberBusinessLayer
    {
        // The conncection to the server db that in the azure platform.
        public string connectionString = "Data Source = devrevise.database.windows.net; Initial Catalog = ReviseDatabase; Integrated Security = False; User ID = revise; Password=P1m2n3r$;Connect Timeout = 60; Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        public UserStatus GetUserValidity(UserDetails u)
        {
            bool isAdmin = false;
            // open connection to the DataBase using the connectionString.
            SqlConnection connection = new SqlConnection(connectionString);
                connection.Open();
            SqlCommand command = new SqlCommand("", connection);
            
            command.CommandText = "Select isAdmin From Members Where userName='" + u.UserName + "' And password='" + u.Password + "'";

                SqlDataReader reader = command.ExecuteReader();
            // if the query returned  any value then...
            if (reader.Read())
                { 
                    
                        isAdmin = (bool)reader["isAdmin"];
                    // if the value boolean is true and is admin then...
                    if(isAdmin)
                    {
                        return UserStatus.AuthenticatedAdmin;
                    }
                    // else he Member in the system
                    else
                        return UserStatus.AuthentucatedUser;


                 }
            // else he not a registered in the system.
            else
                return UserStatus.NonAuthenticatedUser;
            reader.Close();
            connection.Close();
        }
    }
}