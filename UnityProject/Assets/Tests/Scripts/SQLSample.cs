using UnityEngine;
using System;
using System.Collections;

public class SQLSample : MonoBehaviour {

	public string dbFilePath;
	public string dbDirectoryPath;
	public string dbPassword;

	private SqliteDatabase database;

	// Use this for initialization
	void Start () {
		#if UNITY_ANDROID
		MyLibs.InitializeSQLCipher ();
		#endif

		database = new SqliteDatabase(dbFilePath, dbDirectoryPath);
		database.Open();
		
		if(!string.IsNullOrEmpty(dbPassword)) 
			database.Key(dbPassword);

		Write("Open Database at " + database.pathDB);

		database.ExecuteNonQuery ("delete from UserData;");
		Write("Clear Table UserData");

		object[] paramVal = new object[]{1, "Joko", "Jakarta", 28};
		string query = "insert into UserData values ({0}, {1}, {2}, {3});";
		Write ("Execute: " + query, paramVal);
		database.ExecuteNonQuery (query, paramVal);

		QueryTable (1);

		paramVal = new object[]{"Joko Update", 1};
		query = "update UserData  set name = {0} where Id = {1}";
		Write ("Execute: " + query, paramVal);
		database.ExecuteNonQuery (query, paramVal);

		QueryTable (1);

		database.Close ();

		#if UNITY_ANDROID
		MyLibs.ToastMessage ("SQLCipher Done");
		#endif
	}

	void QueryTable(int id) {
		DataTable tableData = database.ExecuteQuery ("select * from UserData where Id = " + id);
		string data = "";
		foreach(DataRow row in tableData.Rows) {
			data += row.GetAsString("Id") + " : " + row.GetAsString("Name") + "\n";
		}
		Write ("Result :: " + data);
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
