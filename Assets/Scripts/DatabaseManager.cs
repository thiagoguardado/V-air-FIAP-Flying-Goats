using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using Mono.Data.SqliteClient;
using System.IO;

public class DatabaseManager : MonoBehaviour {

	private string _constr; 
	private string usersTable = "USERS";
	private string flightsTable = "FLIGHTS";
	private IDbConnection _dbc;
	private IDbCommand _dbcm;
	private IDataReader _dbr;
	private int something;

	void Awake(){

		// setup database
		SetupDB ();
	
	}


	void SetupDB ()
	{
		_constr = "URI=file:" + ConvertPath (Directory.GetCurrentDirectory ()) + "/Assets/Databases/v_air.db";
	}


	public bool ReadDB(int id, out VRUser user){

		string name;
		string sex;
		string flight;

		_dbc=new SqliteConnection(_constr);
		_dbc.Open();

		// find name
		name = SelectStringFromUserTable (id,"NAME");
		sex = SelectStringFromUserTable (id, "SEX");
		flight = SelectStringFromUserTable (id, "FLIGHT");

		if (name != "") {
			user = new VRUser (id, name, sex, flight);
			Debug.Log ("Found info for id " + id + ". Name: " + user.name);
			return true;
		} else {
			user = null;
			return false;
		}

	}


	private string SelectStringFromUserTable (int id, string field)
	{

		_dbcm = _dbc.CreateCommand ();
		_dbcm.CommandText = "SELECT " + field + " FROM USERS WHERE ID=" + id;
		_dbr = _dbcm.ExecuteReader ();
		while (_dbr.Read ()) {
			string _field = _dbr.GetString (0);
			if (_field != "0") {
				return _field;
			}
		}

		return "";
	}

	string ConvertPath(string path){

		return path.Replace("\\","/");


	}

}

public class VRUser{

	public int id;
	public string name;
	public string sex;
	public string flight;
	public bool hasFlight{
		get{ 
			if (flight != "" && flight != null) {
				return true;
			} else {
				return false;
			}
		}
	}


	public VRUser(int _id, string _name, string _sex, string _flight){

		id = _id;
		name = _name;
		sex = _sex;
		flight = _flight;

	}

}
