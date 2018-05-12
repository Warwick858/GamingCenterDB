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
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GamingCenterDB;
using System.Collections.Generic;
using System.Configuration;

namespace GamingCenterDBTest
{
    [TestClass] // TestClass attribute
    public class GamingCenterDBTests
    {
        //The GamingCenterDBConstructor method
        //Purpose: To create a GamingCenter object and initialize to given values
        //Parameters: None
        //Return: None
        [TestMethod] // TestMethod attribute
        public void GamingCenterDBConstructor()
        {
            //Call constructor for GamingCenter class & init test values
            GamingCenter gamingCenter = new GamingCenter()
            {
                FirstName = "Jimmy",
                LastName = "LoForti",
                UserID = "10675175",
                GameType = GameType.Xbox,
                ItemType = ItemType.Generic,
                IsUVUStudent = true,
                Date = new DateTime(2016, 1, 23),
            }; // end constructor()
        } // end method GamingCenterDBConstructor()

        //The ReadAllGamingCenter method
        //Purpose: To call ReadAllGamingCenter() and evaluate returned list: gamers exist and FirstName not null
        //Parameters: None
        //Return: None
        [TestMethod] // TestMethod attribute
        public void ReadAllGamingCenter()
        {
            //Call ConfigMgr to get connection string, and save it
            string connectionString =
                ConfigurationManager.ConnectionStrings["GamingCenterDBConnectionString"].ConnectionString;

            //Create new GCDB object using connString, and open connection
            using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(connectionString))
            {
                //Call ReadAllGamers() and save returned list
                List<GamingCenter> list = db.ReadAllGamingCenter();

                //Verify list is NOT empty
                Assert.IsTrue(list.Count > 0);

                //Foreach gamer
                foreach (GamingCenter gamer in list)
                {
                    //Verify gamer's FirstName is NOT null
                    Assert.IsFalse(string.IsNullOrEmpty(gamer.FirstName));

                    //Write First & LastName to console
                    Console.WriteLine("{0} {1}", gamer.FirstName, gamer.LastName);
                } // end foreach
            } // end using
        } // end method ReadAllGamingCenter()

        //The ReadGamer method
        //Purpose: To call ReadGamer() and evaluate gamer object: gamer not null and FirstName not null
        //Parameters: None
        //Return: None
        [TestMethod] // TestMethod attribute
        public void ReadGamer()
        {
            //Call ConfigMgr to get connection string, and save it
            string connectionString =
                ConfigurationManager.ConnectionStrings["GamingCenterDBConnectionString"].ConnectionString;

            //Create new GCDB object using connString, and open connection
            using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(connectionString))
            {
                //Call ReadGamer with id of 1, and save returned gamer object
                GamingCenter gamer = db.ReadGamer(1);

                //Verify gamer EXISTS
                Assert.IsNotNull(gamer);

                //Verify gamer's FirstName is NOT null
                Assert.IsFalse(string.IsNullOrEmpty(gamer.FirstName));

                //Write First & LastName to console
                Console.WriteLine("{0} {1}", gamer.FirstName, gamer.LastName);
            } // end using
        } // end method ReadGamer()

        //The CreateGamer method
        //Purpose: To create GC object, call CreateGamer() and evaluate: gamer id is not negative
        //Parameters: None
        //Return: None
        [TestMethod] // TestMethod attribute
        public void CreateGamer()
        {
            //Call ConfigMgr to get connection string, and save it
            string connectionString =
                ConfigurationManager.ConnectionStrings["GamingCenterDBConnectionString"].ConnectionString;

            //Create new GCDB object using connString, and open connection
            using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(connectionString))
            {
                //Create new GamingCenter object and initialize given values
                GamingCenter gamer = new GamingCenter()
                {
                    Id = -1, // database will re-populate id
                    FirstName = "Jane",
                    LastName = "Doe",
                    UserID = "0000000",
                    GameType = GameType.Xbox,
                    ItemType = ItemType.Generic,
                    IsUVUStudent = true,
                    Date = new DateTime(2016, 1, 1)
                }; // end constructor

                //Create gamer entry
                db.CreateGamer(gamer);

                //Verify Id is NOT negative
                Assert.IsTrue(gamer.Id > 0);
            } // end using
        } // end method createGamer()

        //The UpdateGamer method
        //Purpose: Create a new gamer using given values, then modify gamer's values, and verify: gamer values were changed
        //Parameters: None
        //Return: None
        [TestMethod] // TestMethod attribute
        public void UpdateGamer()
        {
            //Call ConfigMgr to get connection string, and save it
            string connectionString =
                ConfigurationManager.ConnectionStrings["GamingCenterDBConnectionString"].ConnectionString;

            //Create new GCDB object using connString, and open connection
            using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(connectionString))
            {
                //Create new GamingCenter object and initialize given values
                GamingCenter gamer = new GamingCenter()
                {
                    Id = -1, // database will re-populate id
                    FirstName = "Monkey",
                    LastName = "McTester",
                    UserID = "9999999",
                    GameType = GameType.Foos,
                    ItemType = ItemType.Ball,
                    IsUVUStudent = true,
                    Date = new DateTime(2015, 2, 2)
                }; // end constructor

                //Create gamer entry
                db.CreateGamer(gamer);

                //Verify Id is NOT negative
                Assert.IsTrue(gamer.Id > 0);

                //Change all properties to different values
                gamer.FirstName = "Champ";
                gamer.LastName = "Awesometon";
                gamer.UserID = "1010101";
                gamer.GameType = GameType.PS4;
                gamer.ItemType = ItemType.Generic;
                gamer.IsUVUStudent = false;
                gamer.Date = new DateTime(2000, 10, 10);

                //Update the entry
                db.UpdateGamer(gamer);

                //Read gamer back in and verify changes were made
                GamingCenter gamerCopy = db.ReadGamer(gamer.Id);
                Assert.AreEqual(gamer.FirstName, gamerCopy.FirstName);
                Assert.AreEqual(gamer.LastName, gamerCopy.LastName);
                Assert.AreEqual(gamer.UserID, gamerCopy.UserID);
                Assert.AreEqual(gamer.GameType, gamerCopy.GameType);
                Assert.AreEqual(gamer.ItemType, gamerCopy.ItemType);
                Assert.AreEqual(gamer.IsUVUStudent, gamerCopy.IsUVUStudent);
                Assert.AreEqual(gamer.Date, gamerCopy.Date);
            } // end using
        } // end method UpdateGamer()

        //The DeleteGamer method
        //Purpose: Create a gamer object, get back its ID, delete the gamer and verify: gamer is now null
        //Parameters: None
        //Return: None
        [TestMethod] // TestMethod attribute
        public void DeleteGamer()
        {
            //Call ConfigMgr to get connection string, and save it
            string connectionString =
                ConfigurationManager.ConnectionStrings["GamingCenterDBConnectionString"].ConnectionString;

            //Create new GCDB object using connString, and open connection
            using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(connectionString))
            {
                //Create new GamingCenter object and initialize given values
                GamingCenter gamer = new GamingCenter()
                {
                    Id = -1, // database will re-populate id
                    FirstName = "Ender",
                    LastName = "Gameless",
                    UserID = "1111111",
                    GameType = GameType.Pong,
                    ItemType = ItemType.PongSet,
                    IsUVUStudent = false,
                    Date = new DateTime(2009, 4, 11)
                }; // end constructor

                //Create gamer entry
                db.CreateGamer(gamer);

                //Verify Id is NOT negative
                Assert.IsTrue(gamer.Id > 0);

                //Delete the entry
                db.DeleteGamer(gamer.Id);

                //Get the recently deleted gamer 
                GamingCenter gamerOld = db.ReadGamer(gamer.Id);

                //Verify the entry is null
                Assert.IsTrue(gamerOld == null);
            } // end using
        } // end method DeleteGamer()

    } // end class GamingCenterDBTests

} // end namespace GamingCenterDBTest
