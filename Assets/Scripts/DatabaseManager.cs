using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;
using UnityEngine.UI;

public class DatabaseManager : MonoBehaviour {

	public static DatabaseManager instance = null;

	private string _constr; 
	private string usersTable = "USERS";
	private string flightsTable = "FLIGHTS";
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	private int something;

	void Awake(){

		// ensure singleton
		if (instance == null) {
			instance = this;
		} else if (instance != this) {
			Destroy (gameObject);
		}
		DontDestroyOnLoad (gameObject);

		// setup database
		SetupDB ();
	
	}


	void SetupDB ()
	{

		string databaseName = "v_air.db";

		// try to run local
		string editorFilePath = Application.dataPath + "/StreamingAssets/v_air.db";


		if (!File.Exists (editorFilePath)) {
		
			Debug.Log ("VAIR_ not in editor mode, look for db locally");

			string filepath = Application.persistentDataPath + "/" + databaseName;

			// check if file exists
			if (!File.Exists (filepath)) {

				// if it doesn't ->

				// open StreamingAssets directory and load the db ->

				WWW loadDB = new WWW ("jar:file://" + Application.dataPath + "!/assets/" + databaseName);  // this is the path to your StreamingAssets in android

				while (!loadDB.isDone) {
				}  // CAREFUL here, for safety reasons you shouldn't let this while loop unattended, place a timer and error check

				// then save to Application.persistentDataPath

				File.WriteAllBytes (filepath, loadDB.bytes);

				Debug.Log ("VAIR_ db saved to persistent data");

			} else {
			
				Debug.Log ("VAIR_ db found");
			
			}


			_constr = "URI=file:" + filepath;

		} else {
		
			Debug.Log ("VAIR_ editor mode: db found");

			_constr = "URI=file:" + editorFilePath;

		}
			


	}

	public bool SetupAndRead(int id, out VRUser user){
	
		// open database
		OpenDB ();

		bool read = ReadDB (id, out user);

		// close database
		CloseDB ();
	
		return read;
	}

	public bool ReadDB(int id, out VRUser user){

		string name;
		string sex;
		string flight;
		string flightTime;
		string locator;


		// find name
		name = SelectStringFromUserTable (id,"NAME");
		sex = SelectStringFromUserTable (id,"SEX");
		flight = SelectStringFromUserTable (id, "FLIGHT");
		locator = SelectStringFromUserTable (id,"LOCATOR");

		if (flight != "") {
			flightTime = SelectStringFromFlightsTable (flight, "BOARDING_TIME");
		} else {
			flightTime = "";
		}

		if (name != "") {
			user = new VRUser (id, name, sex, flight, flightTime,locator);
			Debug.Log ("VAIR_ Found info for id " + id + ". Name: " + user.name);
			return true;
		} else {
			user = null;
			return false;
		}

	}
		

	private string SelectStringFromUserTable (int id, string field)
	{

		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = "SELECT " + field + " FROM " + usersTable + " WHERE ID=" + id;
		_dbr = _dbcm.ExecuteReader ();

		string result = "";

		while (_dbr.Read ()) {
			string _field = _dbr.GetString (0);
			if (_field != "0") {
				result = _field;
				break;
			}
		}

		_dbr.Close ();
		_dbr = null;
		_dbcm.Dispose();
		_dbcm = null;


		Debug.Log ("Read on DB: " + result);

		return result;


	}


	private string SelectStringFromFlightsTable (string id, string field)
	{

		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = "SELECT " + field + " FROM " + flightsTable + " WHERE NUMBER='" + id + "'";
		_dbr = _dbcm.ExecuteReader ();

		string result = "";

		while (_dbr.Read ()) {
			string _field = _dbr.GetString (0);
			if (_field != "0") {
				result = _field;
				break;
			}
		}

		_dbr.Close ();
		_dbr = null;
		_dbcm.Dispose();
		_dbcm = null;

		Debug.Log ("Read on DB: " + result);
		return result;


	}


	string ConvertPath(string path){

		return path.Replace("\\","/");

	}

	void OnDisable(){

	}


	void OpenDB ()
	{
		//Open connection to the database.
		_dbc = (IDbConnection)new SqliteConnection (_constr);
		_dbc.Open ();
		Debug.Log ("VAIR_ Database Opened");
	}


	private void CloseDB(){

		_dbc.Close();
		_dbc = null;
		Debug.Log ("VAIR_ Database Closed");
	}

}

public class VRUser{

	public int id;
	public string name;
	public string sex;
	public string flight;
	public string flightTime;
	public string locator;
	public bool hasFlight{
		get{ 
			if (flight != "" && flight != null) {
				return true;
			} else {
				return false;
			}
		}
	}


	public VRUser(int _id, string _name, string _sex, string _flight, string _flightTime, string _locator){

		id = _id;
		name = _name;
		sex = _sex;
		flight = _flight;
		flightTime = _flightTime;
		locator = _locator;

	}

}
