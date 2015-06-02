using System;
using UnityEngine;
using System.IO;
using UnityORM;
using System.Collections.Generic;

public static class ORMSQLiteInit
{
	public static DBEvolution Evolution;
	public static string pathDB;
	
	static bool alreadyInited = false;
	
	public static void InitSqlite(string file, string directory, string password) {
		
		if(alreadyInited) return;
		alreadyInited = true;

		pathDB = Path.Combine (Application.persistentDataPath, directory);
		pathDB = Path.Combine (pathDB, file);

		Evolution = new DBEvolution(file, directory, password);
		
		Evolution.RecreateTableIfHashDiffers = true;// You shoud set false when you release application to avoid suddon drop table.
		
		// Init table
		Evolution.Evolute("SampleTable", // Traget table
			new List<string>(){ // These sql is executed at once.
			@"CREATE TABLE SampleTable(id INTEGER,nickname TEXT,lastLogin INTEGER, score FLOAT);",
			@"ALTER TABLE SampleTable ADD COLUMN age INTEGER;"
		});
		
		Evolution.Evolute("OtherTable",
			new List<string>(){
			@"CREATE TABLE OtherTable(key TEXT,val TEXT);",
			@"INSERT INTO OtherTable VALUES ('a','b')"
		});
		
		Evolution.Evolute("UserData",
			new List<string>(){
			//@"CREATE TABLE UserData(id INTEGER PRIMARY_KEY,name TEXT,hoge TEXT); "
			SQLMaker.GenerateCreateTableSQL<UserData>(ClassDescRepository.Instance.GetClassDesc<UserData>()) + " "
		});
	}
}