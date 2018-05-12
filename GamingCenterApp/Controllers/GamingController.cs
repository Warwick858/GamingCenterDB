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
using GamingCenterDB;
using GamingCenterApp.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace GamingCenterApp.Controllers
{
    public class GamingController : Controller
    {
        //Declare Static Variables
        static string _connectionString = null;
        static List<KeyValuePair<string, string>> _gameTypeOptions = null;
        static List<KeyValuePair<string, string>> _itemTypeOptions = null;

        //The ConnectionString property
        static string ConnectionString
        {
            get
            {
                //If connectionString has not been set, set it
                if (string.IsNullOrEmpty(_connectionString))
                    _connectionString = ConfigurationManager.ConnectionStrings["GamingCenterDBConnectionString"].ConnectionString;

                return _connectionString;
            } // end method get()
        } // end property ConnectionString

        //The GetGameTypeOptions method
        //Purpose: To create a list, populate it with key-value paris, and return the list
        //Parameters: None
        //Return: _gameTypeOptions in the form of a list of string key-value pairs
        static List<KeyValuePair<string, string>> GetGameTypeOptions()
        {
            //If _gameTypeOptions does NOT exist
            if (_gameTypeOptions == null)
            {
                //Create new key-value list
                _gameTypeOptions = new List<KeyValuePair<string, string>>();

                //Populate list
                _gameTypeOptions.Add(new KeyValuePair<string, string> ("Xbox", "Xbox"));
                _gameTypeOptions.Add(new KeyValuePair<string, string>("PS4", "PS4"));
                _gameTypeOptions.Add(new KeyValuePair<string, string>("Wii", "Wii"));
                _gameTypeOptions.Add(new KeyValuePair<string, string>("Pool", "Pool"));
                _gameTypeOptions.Add(new KeyValuePair<string, string>("Pong", "Pong"));
                _gameTypeOptions.Add(new KeyValuePair<string, string>("Hockey", "Hockey"));
                _gameTypeOptions.Add(new KeyValuePair<string, string>("Foos", "Foos"));
            } // end if

            return _gameTypeOptions;
        } // end method GetGameTypeOptions()

        //The GetItemTypeOptions method
        //Purpose: To create a list, populate it with key-value paris, and return the list
        //Parameters: None
        //Return: _itemTypeOptions in the form of a list of string key-value pairs
        static List<KeyValuePair<string, string>> GetItemTypeOptions()
        {
            //If _gameTypeOptions does NOT exist
            if (_itemTypeOptions == null)
            {
                //Create new key-value list
                _itemTypeOptions = new List<KeyValuePair<string, string>>();

                //Populate list
                _itemTypeOptions.Add(new KeyValuePair<string, string>("Generic", "Generic Controller"));
                _itemTypeOptions.Add(new KeyValuePair<string, string>("Pro", "Pro Controller"));
                _itemTypeOptions.Add(new KeyValuePair<string, string>("Remote", "Remote Controller"));
                _itemTypeOptions.Add(new KeyValuePair<string, string>("Cue", "Pool Cue"));
                _itemTypeOptions.Add(new KeyValuePair<string, string>("PongSet", "Pong Set"));
                _itemTypeOptions.Add(new KeyValuePair<string, string>("PuckSet", "Puck Set"));
                _itemTypeOptions.Add(new KeyValuePair<string, string>("Ball", "Foos Ball"));
            } // end if

            return _itemTypeOptions;
        } // end method GetGameTypeOptions()

        //The Index method
        //Purpose: To connect to the DAL, pull all records from the database and convert them to GamingModel objects
        //Parameters: None
        //Return: The gaming list in the form of a View, or a 500 error if connection or data doesn't exist
        // GET: Gaming
        public ActionResult Index()
        {
            //Create a list of GamingModel objects
            List<GamingModel> gamingList = new List<GamingModel>();

            try
            {
                //Create instance of DAL, and pass connection string by calling ConnectionString property
                using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(ConnectionString))
                {
                    //Foreach GamingCenter object in the database
                    foreach (GamingCenter dbGaming in db.ReadAllGamingCenter())
                    {
                        //Get the db data members, and save as GamingModel data members
                        GamingModel gamingModel = new GamingModel()
                        {
                            Id = dbGaming.Id.ToString(),
                            FirstName = dbGaming.FirstName,
                            LastName = dbGaming.LastName,
                            UserID = dbGaming.UserID,
                            GameType = dbGaming.GameType.ToString(),
                            ItemType = dbGaming.ItemType.ToString(),
                            IsUVUStudent = dbGaming.IsUVUStudent,
                            Date = dbGaming.Date,
                            GameTypeOptions = GetGameTypeOptions(),
                            ItemTypeOptions = GetItemTypeOptions()
                        }; // end initialize GamingModel

                        //Add this model to the list
                        gamingList.Add(gamingModel);
                    } // end foreach
                } // end using
            } // end try
            catch (Exception)
            {
                //Return a HttpStatusCodeResult object, and set status code as 500 (InternalServerError) and pass string to print
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Internal Server Error ");
            } // end catch

            return View(gamingList);
        } // end method Index()

        //The Details method
        //Purpose: To connect to the DAL and get gamer with specified id from database
        //Parameters: An int represented as _id - Value pulled from URI
        //Return: The gamingModel View in the form of an ActionResult, or status code "NOT FOUND" for no gamer w/ id, or 500 error for all other
        // GET: Gaming/Details/5
        public ActionResult Details(int id)
        {
            //Create GamingModel instance and initialize to null
            GamingModel gamingModel = null;

            try
            {
                //Create instance of DAL, and pass connection string by calling ConnectionString property
                using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(ConnectionString))
                {
                    //Get GamingCenter object with specified _id, and save
                    GamingCenter dbGamer = db.ReadGamer(id);

                    //If gamer cannot be found
                    if (dbGamer == null)
                    {
                        //Return 404 - Not found status code
                        return new HttpStatusCodeResult(
                            HttpStatusCode.NotFound,
                            string.Format("Gamer with id={0} unknown", id));
                    } // end if

                    //Save dbGamer data members as GamingModel object data members
                    gamingModel = new GamingModel()
                    {
                        Id = dbGamer.Id.ToString(),
                        FirstName = dbGamer.FirstName,
                        LastName = dbGamer.LastName,
                        UserID = dbGamer.UserID.ToString(),
                        GameType = dbGamer.GameType.ToString(),
                        ItemType = dbGamer.ItemType.ToString(),
                        IsUVUStudent = dbGamer.IsUVUStudent,
                        Date = dbGamer.Date,
                        GameTypeOptions = GetGameTypeOptions(),
                        ItemTypeOptions = GetItemTypeOptions()
                    }; // end initialize GamingModel
                } // end using
            } // end try
            catch (Exception)
            {
                //Return a HttpStatusCodeResult object, and set status code as 500 (InternalServerError) and pass string to print
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Internal Server Error ");
            } // end catch

            return View(gamingModel);
        } // end method Details()

        //The Create method - GET
        //Purpose: To submit a GET request to load the page
        //Parameters: None
        //Return: A View in the form of an ActionResult
        // GET: Gaming/Create
        public ActionResult Create()
        {
            //Create new instance of GamingModel
            GamingModel gamingModel = new GamingModel();

            //Populate GameTypeOptions
            gamingModel.GameTypeOptions = GetGameTypeOptions();

            //Populate ItemTypeOptions
            gamingModel.ItemTypeOptions = GetItemTypeOptions();

            return View(gamingModel);
        } // end method GET Create()

        //The Create method - POST
        //Purpose: To get values from the FormCollection object, and populate data members of new GamingCenter object
        //Parameters: A GamingModel object represented as _gamer
        //Return: A 302 redirect if creation of GamingCenter object is successful, or a Internal Server Error if not
        // POST: Gaming/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(GamingModel _gamer)
        {
            //If model is BAD - user validation failed
            if (AuditModel(_gamer) == -1)
            {
                //Return the model and List options
                _gamer.GameTypeOptions = _gameTypeOptions;
                _gamer.ItemTypeOptions = _itemTypeOptions;
                return View(_gamer);
            } // end if
            else // Model is GOOD
            {
                try
                {
                    //Create instance of DAL, and pass connection string by calling ConnectionString property
                    using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(ConnectionString))
                    {
                        //Create new GamingCenter object
                        GamingCenter dbGamer = new GamingCenter();

                        //Get each data member of the GamingModel object and save as the corresponding data member of the GamingCenter object
                        dbGamer.FirstName = _gamer.FirstName;
                        dbGamer.LastName = _gamer.LastName;
                        dbGamer.UserID = _gamer.UserID;
                        dbGamer.GameType = (GameType)Enum.Parse(typeof(GameType), _gamer.GameType);
                        dbGamer.ItemType = (ItemType)Enum.Parse(typeof(ItemType), _gamer.ItemType);
                        dbGamer.IsUVUStudent = _gamer.IsUVUStudent;
                        dbGamer.Date = _gamer.Date;

                        //Add Gamer to the db
                        db.CreateGamer(dbGamer);
                    } // end using

                    //Return a 302 redirect action to Index page
                    return RedirectToAction("Index");
                } // end try
                catch
                {
                    //Return a HttpStatusCodeResult object, and set status code as 500 (InternalServerError) and pass string to print
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Internal Server Error ");
                } // end catch
            } // end else
        } // end method POST Create()

        //The Edit method - GET
        //Purpose: To call the DAL's ReadGamer() and save as GamingModel object, then return the gamingModel View
        //Parameters: An int represented as _id
        //Return: The gamingModel View in the form of an ActionResult, or 404 for gamer not found, and 500 for all other
        // GET: Gaming/Edit/5
        public ActionResult Edit(int id)
        {
            //Create GamingModel instance and initialize to null
            GamingModel gamingModel = null;

            try
            {
                //Create instance of DAL, and pass connection string by calling ConnectionString property
                using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(ConnectionString))
                {
                    //Get GamingCenter object with specified _id, and save
                    GamingCenter dbGamer = db.ReadGamer(id);

                    //If gamer cannot be found
                    if (dbGamer == null)
                    {
                        //Return 404 - Not found status code
                        return new HttpStatusCodeResult(
                            HttpStatusCode.NotFound,
                            string.Format("Gamer with id={0} unknown", id));
                    } // end if

                    //Save dbGamer data members as GamingModel object data members
                    gamingModel = new GamingModel()
                    {
                        Id = dbGamer.Id.ToString(),
                        FirstName = dbGamer.FirstName,
                        LastName = dbGamer.LastName,
                        UserID = dbGamer.UserID.ToString(),
                        GameType = dbGamer.GameType.ToString(),
                        ItemType = dbGamer.ItemType.ToString(),
                        IsUVUStudent = dbGamer.IsUVUStudent,
                        Date = dbGamer.Date,
                        GameTypeOptions = GetGameTypeOptions(),
                        ItemTypeOptions = GetItemTypeOptions()
                    }; // end initialize GamingModel
                } // end using
            } // end try
            catch (Exception)
            {
                //Return a HttpStatusCodeResult object, and set status code as 500 (InternalServerError) and pass string to print
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Internal Server Error ");
            } // end catch

            return View(gamingModel);
        } // end method GET Edit()

        //The Edit method - POST
        //Purpose: To get data members from the collection and save into GamingCenter object, then update the database
        //Parameters: A int represented as id and a GamingModel object represented as _gamer
        //Return: A 302 redirect if successful, or 404 for gamer not found, and 500 for all other
        // POST: Gaming/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, GamingModel _gamer)
        {
            //If model is BAD - user validation failed
            if (AuditModel(_gamer) == -1)
            {
                //Return the model and List options
                _gamer.GameTypeOptions = _gameTypeOptions;
                _gamer.ItemTypeOptions = _itemTypeOptions;
                return View(_gamer);
            } // end if
            else // Model is GOOD
            {
                try
                {
                    //Create instance of DAL, and pass connection string by calling ConnectionString property
                    using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(ConnectionString))
                    {
                        //Create new GamingCenter object
                        GamingCenter dbGamer = new GamingCenter();

                        //Get each data member of the GamingModel object and save as the corresponding data member of the GamingCenter object
                        dbGamer.Id = id;
                        dbGamer.FirstName = _gamer.FirstName;
                        dbGamer.LastName = _gamer.LastName;
                        dbGamer.UserID = _gamer.UserID;
                        dbGamer.GameType = (GameType)Enum.Parse(typeof(GameType), _gamer.GameType);
                        dbGamer.ItemType = (ItemType)Enum.Parse(typeof(ItemType), _gamer.ItemType);
                        dbGamer.IsUVUStudent = _gamer.IsUVUStudent;
                        dbGamer.Date = _gamer.Date; // dbGamer.Date = DateTime.Parse(_gamer.Date)

                        //Update gamer in database
                        db.UpdateGamer(dbGamer);
                    } // end using

                    //Return a 302 redirect action to Index page
                    return RedirectToAction("Index");
                } // end try
                catch (KeyNotFoundException) // gamer _id was bad
                {
                    //Return 404 - Not found status code
                    return new HttpStatusCodeResult(
                        HttpStatusCode.NotFound,
                        string.Format("Gamer with id={0} unknown", id));
                } // end catch
                catch
                {
                    //Return a HttpStatusCodeResult object, and set status code as 500 (InternalServerError) and pass string to print
                    return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Internal Server Error ");
                } // end catch
            } // end else
        } // end method POST Edit()

        //The Delete method - GET
        //Purpose: To connect to the DAL and get gamer with specified id from database
        //Parameters: An int represented as _id - Value pulled from URI
        //Return: The gamingModel View in the form of an ActionResult, or status code "NOT FOUND" for no gamer w/ id, or 500 error for all other
        // GET: Gaming/Delete/5
        public ActionResult Delete(int id)
        {
            //Create GamingModel instance and initialize to null
            GamingModel gamingModel = null;

            try
            {
                //Create instance of DAL, and pass connection string by calling ConnectionString property
                using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(ConnectionString))
                {
                    //Get GamingCenter object with specified _id, and save
                    GamingCenter dbGamer = db.ReadGamer(id);

                    //If gamer cannot be found
                    if (dbGamer == null)
                    {
                        //Return 404 - Not found status code
                        return new HttpStatusCodeResult(
                            HttpStatusCode.NotFound,
                            string.Format("Gamer with id={0} unknown", id));
                    } // end if

                    //Save dbGamer data members as GamingModel object data members
                    gamingModel = new GamingModel()
                    {
                        Id = dbGamer.Id.ToString(),
                        FirstName = dbGamer.FirstName,
                        LastName = dbGamer.LastName,
                        UserID = dbGamer.UserID.ToString(),
                        GameType = dbGamer.GameType.ToString(),
                        ItemType = dbGamer.ItemType.ToString(),
                        IsUVUStudent = dbGamer.IsUVUStudent,
                        Date = dbGamer.Date, // ToString()
                        GameTypeOptions = GetGameTypeOptions(),
                        ItemTypeOptions = GetItemTypeOptions()
                    }; // end initialize GamingModel
                } // end using
            } // end try
            catch (Exception)
            {
                //Return a HttpStatusCodeResult object, and set status code as 500 (InternalServerError) and pass string to print
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Internal Server Error ");
            } // end catch

            return View(gamingModel);
        } // end method GET Delete()

        //The Delete method - POST
        //Purpose: To connect to the DAL and delete the gamer with the specified id
        //Parameters: A int represented as id and a GamingModel object represented as _gamer
        //Return: A 302 redirect if successful, or 404 for gamer not found, and 500 for all other
        // POST: Gaming/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, GamingModel _gamer)
        {
            try
            {
                //Create instance of DAL, and pass connection string by calling ConnectionString property
                using (GamingCenterDirectoryDB db = new GamingCenterDirectoryDB(ConnectionString))
                {
                    //Call DAL to delete gamer from database
                    db.DeleteGamer(id);
                } // end using

                //Return a 302 redirect action to Index page
                return RedirectToAction("Index");
            } // end try
            catch (KeyNotFoundException) // gamer _id was bad
            {
                //Return 404 - Not found status code
                return new HttpStatusCodeResult(
                    HttpStatusCode.NotFound,
                    string.Format("Gamer with id={0} unknown", id));
            } // end catch
            catch
            {
                //Return a HttpStatusCodeResult object, and set status code as 500 (InternalServerError) and pass string to print
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError, "Internal Server Error ");
            } // end catch
        } // end method POST Delete()

        //The AuditModel method
        //Purpose: To validate the firstName, lastName, and User ID data members of the given GamingModel object
        //Parameters: A GamingModel object represented as _gamer
        //Return: An int: 0 = Model Valid, -1 = Model NOT Valid
        private int AuditModel(GamingModel _gamer)
        {
            //Declare helper vars:
            DateTime date;
            bool boolean;

            //Create string of special characters
            string specChars = @"^!#$%^&*()[]{}<>";
            //Split string into array
            char[] specCharsArray = specChars.ToCharArray();

            //Ensure first name is NOT empty or white space
            if (string.IsNullOrWhiteSpace(_gamer.FirstName))
                ModelState.AddModelError("FirstName", "First name is required. ");

            //Ensure first name is NOT null
            if (_gamer.FirstName != null)
            {
                //Ensure first name does NOT exceed max length
                if (_gamer.FirstName.Length > 30)
                    ModelState.AddModelError("FirstName", "First name cannot exceed 30 characters. ");
            
                //Ensure first name does NOT include prohibited special chars
                if (_gamer.FirstName.IndexOfAny(specCharsArray) != -1)
                    ModelState.AddModelError("FirstName", "First name can contain only 1 to 30 letters, hyphens, or spaces. ");
            } // end if

            //Ensure last name is NOT empty or white space
            if (string.IsNullOrWhiteSpace(_gamer.LastName))
                ModelState.AddModelError("LastName", "Last name is required. ");

            //Ensure last name is NOT null
            if (_gamer.LastName != null)
            {
                //Ensure last name does NOT exceed max length
                if (_gamer.LastName.Length > 30)
                    ModelState.AddModelError("LastName", "Last name cannot exceed 30 characters. ");

                //Ensure last name does NOT include prohibited special chars
                if (_gamer.LastName.IndexOfAny(specCharsArray) != -1)
                    ModelState.AddModelError("LastName", "Last name can contain only 1 to 30 letters, hyphens, or spaces. ");
            } // end if

            //Ensure user ID is NOT empty or white space
            if (string.IsNullOrWhiteSpace(_gamer.UserID))
                ModelState.AddModelError("UserID", "User ID is required. ");

            //Ensure user ID is NOT null
            if (_gamer.UserID != null)
            {
                //Ensure User ID does NOT include prohibited special chars
                if (_gamer.UserID.IndexOfAny(specCharsArray) != -1)
                    ModelState.AddModelError("UserID", "User ID can contain only 1 to 30 letters, numbers, hyphens, or spaces. ");
            } // end if

            //Ensure game type is NOT null
            if (_gamer.GameType == null)
            {
                ModelState.AddModelError("GameType", "Game type is required. ");
            } // end if

            //Ensure item type is NOT null
            if (_gamer.ItemType == null)
            {
                ModelState.AddModelError("ItemType", "Item type is required. ");
            } // end if

            //Ensure isUVUStudent is a boolean
            if (!bool.TryParse(_gamer.IsUVUStudent.ToString(), out boolean))
            {
                ModelState.AddModelError("IsUVUStudent", "UVU student indication is required, check box for 'yes'. ");
            } // end if

            //Ensure date is within SQL DateTime bounds
            if (_gamer.Date.Year < 1753 || _gamer.Date.Year > 2999)
            {
                ModelState.AddModelError("Date", "Date year must be between 1753 and 2999. ");
            } // end if

            //Ensure date can be parsed
            if (!DateTime.TryParse(_gamer.Date.ToString(), out date))
            {
                ModelState.AddModelError("Date", "Invalid date format. ");
            } // end if

            //If the model is BAD
            if (!ModelState.IsValid)
            {
                return -1;
            } // end if

            return 0;
        } // end method AuditModel()

    } // end class GamingController
} // end namespace GamingCenterApp.Controllers
