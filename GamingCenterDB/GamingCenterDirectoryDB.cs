// ******************************************************************************************************************
//  Copyright(C) 2018  James LoForti
//  Contact Info: jamesloforti@gmail.com
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.If not, see<https://www.gnu.org/licenses/>.
//									     ____.           .____             _____  _______   
//									    |    |           |    |    ____   /  |  | \   _  \  
//									    |    |   ______  |    |   /  _ \ /   |  |_/  /_\  \ 
//									/\__|    |  /_____/  |    |__(  <_> )    ^   /\  \_/   \
//									\________|           |_______ \____/\____   |  \_____  /
//									                             \/          |__|        \/ 
//
// ******************************************************************************************************************
//
using System;
using System.Collections.Generic;
using System.Data.SqlClient;

namespace GamingCenterDB
{
    public class GamingCenterDirectoryDB: IDisposable // IDisposable interface
    {
        //Declare SqlConnection object to hold the connection
        private SqlConnection _conn;

        //The GamingCenterDirectoryDB Constructor
        //Purpose: To create SqlConnection object, initialize its string, and open the connection
        //Parameters: A string represented as _connectionString
        //Return: None
        public GamingCenterDirectoryDB(string _connectionString)
        {
            _conn = new SqlConnection();
            _conn.ConnectionString = _connectionString;
            _conn.Open();
        } // end constructor GamingCenterDirectoryDB()

        //The Dispose method
        //Purpose: To close the SqlConnection
        //Parameters: None
        //Return: None
        public void Dispose()
        {
            _conn.Close();
        } // end method Dispose

        //The ReadAllGamingCenter method
        //Purpose: To read all gamers from the GamingCenter table
        //Parameters: None
        //Return: A list of GamingCenter objects in the form of a list
        public List<GamingCenter> ReadAllGamingCenter()
        {
            //Create SQL command - SELECT all data members from GC table
            string cmdText = @"
                SELECT
                    Id, FirstName, LastName, UserID, GameType, ItemType, IsUVUStudent, Date
                FROM dbo.GamingCenter";

            //Create list of GamingCenter objects
            List<GamingCenter> list = new List<GamingCenter>();

            //Create new SqlCommand and execute query
            using (SqlCommand cmd = new SqlCommand(cmdText, _conn))
            {
                //Create new SqlDatReader and execute reader
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    //While data exists - READ
                    while (reader.Read())
                    {
                        //Create new GamingCenter object
                        GamingCenter gamer = new GamingCenter();

                        //Read values and save
                        gamer.Id = (int)reader["Id"];
                        gamer.FirstName = (string)reader["FirstName"];
                        gamer.LastName = (string)reader["LastName"];
                        gamer.UserID = (string)reader["UserID"];
                        gamer.GameType = (GameType)Enum.Parse(typeof(GameType), (string)reader["GameType"]);
                        gamer.ItemType = (ItemType)Enum.Parse(typeof(ItemType), (string)reader["ItemType"]);
                        gamer.IsUVUStudent = (bool)reader["IsUVUStudent"];
                        gamer.Date = (DateTime)reader["Date"];

                        //Add gamer to list
                        list.Add(gamer);
                    } // end while
                } // end using
            } // end using

            return list;
        } // end method ReadAllGamingCenter()

        //The ReadGamer method
        //Purpose: To read the specified gamer from the GamingCenter table
        //Parameters: An int represented as _id
        //Return: A gamer in the form of a GamingCenter object
        public GamingCenter ReadGamer(int _id)
        {
            //Create SQL command - SELECT all data members from GC table with given id
            string cmdText = @"
                SELECT
                    Id, FirstName, LastName, UserID, GameType, ItemType, IsUVUStudent, Date
                FROM dbo.GamingCenter
                WHERE Id = @id";

            //Create new GamingCenter object and initialize to null
            GamingCenter gamer = null;

            //Create new SqlCommand and execute query
            using (SqlCommand cmd = new SqlCommand(cmdText, _conn))
            {
                //Add id parameter to command object
                cmd.Parameters.AddWithValue("@id", _id);

                //Create new SqlDatReader and execute reader
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    //If data exists - READ
                    if (reader.Read())
                    {
                        //Create new GamingCenter object
                        gamer = new GamingCenter();

                        //Read values and save
                        gamer.Id = (int)reader["Id"];
                        gamer.FirstName = (string)reader["FirstName"];
                        gamer.LastName = (string)reader["LastName"];
                        gamer.UserID = (string)reader["UserID"];
                        gamer.GameType = (GameType)Enum.Parse(typeof(GameType), (string)reader["GameType"]);
                        gamer.ItemType = (ItemType)Enum.Parse(typeof(ItemType), (string)reader["ItemType"]);
                        gamer.IsUVUStudent = (bool)reader["IsUVUStudent"];
                        gamer.Date = (DateTime)reader["Date"];
                    } // end if
                } // end using
            } // end using

            return gamer;
        } // end method ReadGamer()

        //The CreateGamer method
        //Purpose: To create a GamingCenter entry
        //Parameters: A GamingCenter object represented as _gamer
        //Return: None
        public void CreateGamer(GamingCenter _gamer)
        {
            //Create SQL command - INSERT INTO GC table using given values
            string cmdText = @"
                INSERT INTO dbo.GamingCenter
                    (FirstName, LastName, UserID, GameType, ItemType, IsUVUStudent, Date)
                VALUES (@firstName, @lastName, @userID, @gameType, @itemType, @isUVUStudent, @date)
                
                --Get primary key of new entry
                SELECT Id = Scope_Identity()";

            //Create new SqlCommand and execute query
            using (SqlCommand cmd = new SqlCommand(cmdText, _conn))
            {
                //Add all parameters to command object
                cmd.Parameters.AddWithValue("@firstName", _gamer.FirstName);
                cmd.Parameters.AddWithValue("@lastName", _gamer.LastName);
                cmd.Parameters.AddWithValue("@userID", _gamer.UserID);
                cmd.Parameters.AddWithValue("@gameType", _gamer.GameType.ToString());
                cmd.Parameters.AddWithValue("@itemType", _gamer.ItemType.ToString());
                cmd.Parameters.AddWithValue("@isUVUStudent", _gamer.IsUVUStudent);
                cmd.Parameters.AddWithValue("@date", _gamer.Date);

                //Create new SqlDatReader and execute reader
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    //If data exists - READ
                    if (reader.Read())
                    {
                        //Get return value from table, convert to int, and save
                        _gamer.Id = Convert.ToInt32(reader["Id"]);
                    } // end if
                } // end using
            } // end using
        } // end method CreateGamer()

        //The UpdateGamer method
        //Purpose: To update a GamingCenter entry
        //Parameters: A GamingCenter object represented as _gamer
        //Return: None
        public void UpdateGamer(GamingCenter _gamer)
        {
            //Create SQL command - UPDATE GC table and SET given values, for gamer with specified Id
            string cmdText = @"
                UPDATE dbo.GamingCenter
                SET
                    FirstName = @firstName,
                    LastName = @lastName,
                    UserID = @userID,
                    GameType = @gameType,
                    ItemType = @itemType,
                    IsUVUStudent = @isUVUStudent,
                    Date = @date
                WHERE Id = @id";

            //Create new SqlCommand and execute query
            using (SqlCommand cmd = new SqlCommand(cmdText, _conn))
            {
                //Add all parameters to command object
                cmd.Parameters.AddWithValue("@firstName", _gamer.FirstName);
                cmd.Parameters.AddWithValue("@lastName", _gamer.LastName);
                cmd.Parameters.AddWithValue("@userID", _gamer.UserID);
                cmd.Parameters.AddWithValue("@gameType", _gamer.GameType.ToString());
                cmd.Parameters.AddWithValue("@itemType", _gamer.ItemType.ToString());
                cmd.Parameters.AddWithValue("@isUVUStudent", _gamer.IsUVUStudent);
                cmd.Parameters.AddWithValue("@date", _gamer.Date);
                cmd.Parameters.AddWithValue("@id", _gamer.Id);

                //Run the query and save rowCount
                int rowCount = cmd.ExecuteNonQuery();

                //If rowCount is less than 1
                if (rowCount < 1)
                {
                    //Throw exception
                    throw new Exception("No rows were updated in UpdateGamer()");
                } // end if
            } // end using
        } // end method UpdateGamer()

        //The DeleteGamer method
        //Purpose: To delete a GamingCenter entry
        //Parameters: An int represented as _id
        //Return: None
        public void DeleteGamer(int _id)
        {
            //Create SQL command - DELETE GC table entry with specified gamer Id
            string cmdText = @"
                DELETE FROM dbo.GamingCenter
                WHERE Id = @id";

            //Create new SqlCommand and execute query
            using (SqlCommand cmd = new SqlCommand(cmdText, _conn))
            {
                //Add Id parameter to command object
                cmd.Parameters.AddWithValue("@id", _id);

                //Run the query and save rowCount
                int rowCount = cmd.ExecuteNonQuery();

                //If rowCount is less than 1
                if (rowCount < 1)
                {
                    //Throw exception
                    throw new Exception("No rows were deleted in DeleteGamer()");
                } // end if
            } // end using
        } // end method DeleteGamer()

    } // end class GamingCenterDirectoryDB

} // end namespace GamingCenterDB
