using UnityEngine;
using System.Collections;
using UnityORM;
using System;

public class ORMSample : MonoBehaviour {

	public string dbFile;
	public string dbDirectory;
	public string password;

	public UserData[] dataObjects;

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID
		MyLibs.InitializeSQLCipher ();
		#endif

		if (dataObjects.Length < 1) {
			Write("No data to write database!");
			return;
		}

		ORMSQLiteInit.InitSqlite(dbFile, dbDirectory, password);
		Write("Open Database at " + ORMSQLiteInit.pathDB);

		FieldLister lister = new FieldLister();
		
		Write(dataObjects[0].ToString());
		var info = lister.ListUp<UserData>();
		
		Write(info.ToString());
		string insert = SQLMaker.GenerateInsertSQL(info, dataObjects[0]);
		string update = SQLMaker.GenerateUpdateSQL(info, dataObjects[0]);
		
		Write("Insert = {0}",insert);
		Write("Update = {0}", update);
		
		DBMapper mapper = new DBMapper(ORMSQLiteInit.Evolution.Database);
		mapper.UpdateOrInsertAll(dataObjects);
		
		UserData[] fromDb = mapper.Read<UserData>("SELECT * FROM UserData;");
		Write(fromDb[0].ToString());
		Write(fromDb[1].ToString());

		JSONMapper jsonMapper = new JSONMapper();
		string json = jsonMapper.Write<UserData>(fromDb);
		UserData[] fromJson = jsonMapper.Read<UserData>(json);
		
		Write("Json = {0}", json);
		Write(fromJson[0].ToString());
		Write(fromJson[1].ToString());

		#if UNITY_ANDROID
		MyLibs.ToastMessage ("SQLCipher Done");
		#endif
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up, 5);
	}

	string text = "";

	void Write(string fmt, params object[] pars)
	{
		lock (text) {
			text += string.Format("\n{0}\t{1}\n", DateTime.Now, string.Format(fmt, pars));
		}
	}

	void OnGUI()
	{
		lock (text) {
			GUI.TextArea(new Rect(10, 10, Screen.width - 20, Screen.height - 20), text);
		}
	}
}