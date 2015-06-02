using System;
using System.Runtime.InteropServices;
using UnityEngine;

public class SqliteException : Exception
{
	int errorCode;
	public int ErrorCode{get{return errorCode;}}
	public SqliteException(int errorCode ,string message) : base(message + "(ErrorCode:" + errorCode + ")") {
		this.errorCode = errorCode;
	}
}

/// <summary>
/// Sqlite database.
/// Get from http://gamesforsoul.com/2012/03/sqlite-unity-and-ios-a-rocky-relationship/
/// and modified.
/// </summary>
/// <exception cref='SqliteException'>
/// Is thrown when the sqlite exception.
/// </exception>
public class SqliteDatabase
{
	public static bool IsGoodCode(int resultCode){
		return resultCode == SQLITE_OK || resultCode == SQLITE_DONE;
	}
	public const int SQLITE_OK = 0;
	const int SQLITE_ROW = 100;
	public const int SQLITE_DONE = 101;
	const int SQLITE_INTEGER = 1;
	const int SQLITE_FLOAT = 2;
	const int SQLITE_TEXT = 3;
	const int SQLITE_BLOB = 4;
	const int SQLITE_NULL = 5;
	public const int SQLITE_ERROR_ALREADY_OPENED = -2;
	public const int SQLITE_ERROR_NOT_OPENED = -1;
	
	#if UNITY_EDITOR
	const string dllimport = "sqlite3";
	#elif UNITY_IOS
	const string dllimport = "__Internal";
	#elif UNITY_ANDROID
	const string dllimport = "sqlcipher_android";
	#endif
	
	#if UNITY_EDITOR || UNITY_ANDROID
	[DllImport(dllimport, EntryPoint = "sqlite3_open")]
	private static extern int _sqlite3_open(string filename, out IntPtr db);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_close")]
	private static extern int _sqlite3_close(IntPtr db);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_prepare_v2")]
	private static extern int _sqlite3_prepare_v2(IntPtr db, string zSql, int nByte, out IntPtr ppStmpt, IntPtr pzTail);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_step")]
	private static extern int _sqlite3_step(IntPtr stmHandle);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_errcode")]
	private static extern int _sqlite3_errcode(IntPtr db);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_extended_errcode")]
	private static extern int _sqlite3_extended_errcode(IntPtr db);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_changes")]
	private static extern int _sqlite3_changes(IntPtr db);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_finalize")]
	internal static extern int _sqlite3_finalize(IntPtr stmHandle);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_errmsg")]
	private static extern IntPtr _sqlite3_errmsg(IntPtr db);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_column_count")]
	private static extern int _sqlite3_column_count(IntPtr stmHandle);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_column_name")]
	private static extern IntPtr _sqlite3_column_name(IntPtr stmHandle, int iCol);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_column_type")]
	private static extern int _sqlite3_column_type(IntPtr stmHandle, int iCol);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_column_int")]
	private static extern int _sqlite3_column_int(IntPtr stmHandle, int iCol);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_column_int64")]
	private static extern long _sqlite3_column_int64(IntPtr stmHandle, int iCol);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_column_text")]
	private static extern IntPtr _sqlite3_column_text(IntPtr stmHandle, int iCol);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_column_double")]
	private static extern double _sqlite3_column_double(IntPtr stmHandle, int iCol);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_column_blob")]
	private static extern IntPtr _sqlite3_column_blob (IntPtr stmHandle, int iCol);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_column_bytes")]
	private static extern int _sqlite3_column_bytes (IntPtr stmHandle, int iCol);
	
	[DllImport(dllimport, EntryPoint = "sqlite3_key")]
	private static extern int _sqlite3_key(IntPtr stmHandle, string key, int len);
	
	#elif UNITY_IOS
	[DllImport("__Internal")]
	private static extern int _sqlite3_open(string filename, out IntPtr db);
	
	[DllImport("__Internal")]
	private static extern int _sqlite3_close(IntPtr db);
	
	[DllImport("__Internal")]
	private static extern int _sqlite3_prepare_v2(IntPtr db, string zSql, int nByte, out IntPtr ppStmpt, IntPtr pzTail);
	
	[DllImport("__Internal")]
	private static extern int _sqlite3_step(IntPtr stmHandle);
	
	[DllImport("__Internal")]
	private static extern int _sqlite3_errcode(IntPtr db);
	
	[DllImport("__Internal")]
	private static extern int _sqlite3_extended_errcode(IntPtr db);
	
	[DllImport("__Internal")]
	private static extern int _sqlite3_changes(IntPtr db);
	
	[DllImport("__Internal")]
	internal static extern int _sqlite3_finalize(IntPtr stmHandle);
	
	[DllImport("__Internal")]
	private static extern IntPtr _sqlite3_errmsg(IntPtr db);
	
	[DllImport("__Internal")]
	private static extern int _sqlite3_column_count(IntPtr stmHandle);
	
	[DllImport("__Internal")]
	private static extern IntPtr _sqlite3_column_name(IntPtr stmHandle, int iCol);
	
	[DllImport("__Internal")]
	private static extern int _sqlite3_column_type(IntPtr stmHandle, int iCol);
	
	[DllImport("__Internal")]
	private static extern int _sqlite3_column_int(IntPtr stmHandle, int iCol);
	
	[DllImport("__Internal")]
	private static extern long _sqlite3_column_int64(IntPtr stmHandle, int iCol);
	
	[DllImport("__Internal")]
	private static extern IntPtr _sqlite3_column_text(IntPtr stmHandle, int iCol);
	
	[DllImport("__Internal")]
	private static extern double _sqlite3_column_double(IntPtr stmHandle, int iCol);
	
	[DllImport("__Internal")]
	private static extern IntPtr _sqlite3_column_blob (IntPtr stmHandle, int iCol);
	
	[DllImport("__Internal")]
	private static extern int _sqlite3_column_bytes (IntPtr stmHandle, int iCol);
	
	[DllImport("__Internal")]
	private static extern int _sqlite3_key(IntPtr stmHandle, string key, int len);
	#endif
	
	private IntPtr _connection;
	private bool IsConnectionOpen { get; set; }
	
	private bool CanExQuery = true;
	private string directoryPath;
	public string pathDB { private set; get;}
	
	public bool isOpen {
		get {
			return this.IsConnectionOpen;
		}
	}
	
	#region Public Methods
	
	public bool isDBExist() {
		return CanExQuery;
	}
	
	/// <summary>
	/// Initializes a new instance of the <see cref="SqliteDatabase"/> class.
	/// </summary>
	/// <param name='dbName'> 
	/// Data Base name. (the file needs exist in the streamingAssets folder)
	/// </param>
	public SqliteDatabase (string dbFile, string relativeDirectoryPath = "")
	{
		directoryPath = System.IO.Path.Combine (Application.persistentDataPath, relativeDirectoryPath);
		pathDB = System.IO.Path.Combine (directoryPath, dbFile);
		
		//original path
		string sourcePath = System.IO.Path.Combine (Application.streamingAssetsPath, relativeDirectoryPath + "/" + dbFile);
		
		//if DB does not exist in persistent data folder (folder "Documents" on iOS) or source DB is newer then copy it
		if (!System.IO.File.Exists (pathDB)) {
			if(!System.IO.Directory.Exists(pathDB)) {
				System.IO.Directory.CreateDirectory(directoryPath);
			}
			
			if (sourcePath.Contains ("://")) {
				// Android	
				WWW www = new WWW (sourcePath);
				// Wait for download to complete - not pretty at all but easy hack for now 
				// and it would not take long since the data is on the local device.
				while (!www.isDone) {;}
				
				if (String.IsNullOrEmpty(www.error)) { 					
					System.IO.File.WriteAllBytes(pathDB, www.bytes);
				} else {
					CanExQuery = false;										
				}	
				
			} else {
				// Mac, Windows, Iphone
				//validate the existens of the DB in the original folder (folder "streamingAssets")
				if (System.IO.File.Exists (sourcePath)) {
					
					//copy file - alle systems except Android
					System.IO.File.Copy (sourcePath, pathDB, true);
					
				} else {
					CanExQuery = false;
					Debug.Log ("ERROR: the file DB named " + dbFile + " doesn't exist in the StreamingAssets Folder, please copy it there.");
				}	
				
			}
		}
	}
	
	public void Open()
	{
		if (IsConnectionOpen)
		{
			throw new SqliteException(SQLITE_ERROR_ALREADY_OPENED,"There is already an open connection");
		}
		int openResult = _sqlite3_open(pathDB, out _connection);
		if (openResult != SQLITE_OK)
		{
			throw new SqliteException(openResult,"Could not open database file: " + pathDB);
		}
		IsConnectionOpen = true;
	}
	
	public void Close()
	{
		if(IsConnectionOpen)
		{
			_sqlite3_close(_connection);
		}
		
		IsConnectionOpen = false;
	}

	public void ExecuteNonQuery(string query)
	{
		if (!IsConnectionOpen)
		{
			throw new SqliteException(SQLITE_ERROR_NOT_OPENED,"SQLite database is not open.");
		}
		
		IntPtr stmHandle = Prepare(query);
		
		int result = _sqlite3_step(stmHandle);
		if (result != SQLITE_DONE)
		{
			IntPtr errorMsg = _sqlite3_errmsg(_connection);
			throw new SqliteException(result, Marshal.PtrToStringAnsi(errorMsg) + " FullQuery=|" + query + "|");
		}
		Finalize(stmHandle);
	}
	
	public void DeleteDB() {
		if (IsConnectionOpen) {
			Close();
		}
		System.IO.File.Delete (pathDB);
	}
	
	public DataTable ExecuteQuery(string query)
	{
		if (!IsConnectionOpen)
		{
			throw new SqliteException(SQLITE_ERROR_NOT_OPENED,"SQLite database is not open.");
		}
		
		IntPtr stmHandle = Prepare(query);
		
		int columnCount = _sqlite3_column_count(stmHandle);
		
		var dataTable = new DataTable();
		for (int i = 0; i < columnCount; i++)
		{
			string columnName = Marshal.PtrToStringAnsi(_sqlite3_column_name(stmHandle, i));
			dataTable.Columns.Add(columnName);
		}
		
		//populate datatable
		while (_sqlite3_step(stmHandle) == SQLITE_ROW)
		{
			object[] row = new object[columnCount];
			for (int i = 0; i < columnCount; i++)
			{
				switch (_sqlite3_column_type(stmHandle, i))
				{
				case SQLITE_INTEGER:
					row[i] = _sqlite3_column_int(stmHandle, i);
					break;
					
				case SQLITE_TEXT:
					IntPtr text = _sqlite3_column_text(stmHandle, i);
					row[i] = Marshal.PtrToStringAnsi(text);
					break;
					
				case SQLITE_FLOAT:
					row[i] = (float) _sqlite3_column_double(stmHandle, i);
					break;
					
				case SQLITE_BLOB:
					IntPtr blob = _sqlite3_column_blob (stmHandle, i);
					int size = _sqlite3_column_bytes (stmHandle, i);
					byte[] data = new byte[size];
					Marshal.Copy (blob, data, 0, size);
					row [i] = data;
					break;
					
				case SQLITE_NULL:
					row[i] = null;
					break;
				}
			}
			
			dataTable.AddRow(row);
		}
		
		Finalize(stmHandle);
		
		return dataTable;
	}
	
	public void ExecuteNonQuery(string query, object[] param) {
		ExecuteNonQuery(reformatQuery(query, param));
	}
	
	public DataTable ExecuteQuery (string query, object[] param)
	{
		return ExecuteQuery(reformatQuery(query, param));
	}
	
	string reformatQuery (string query, object[] param) {
		
		query = query.Replace("{", "'{");
		query = query.Replace("}", "}'");
		
		for(int i = 0;i<param.Length;i++) {
			if(param[i] is String) {
				param[i] = ((string)param[i]).Replace("'", "''");
			}
		}

		string queryResult = string.Format(query, param);
		return queryResult;
	}
	
	public void ExecuteScript(string script)
	{
		string[] statements = script.Split(';');
		
		foreach (string statement in statements)
		{
			if (!string.IsNullOrEmpty(statement.Trim ()))
			{
				ExecuteNonQuery(statement);
			}
		}
	}
	
	public void Key(string key){
		
		if (Application.platform == RuntimePlatform.IPhonePlayer || Application.platform == RuntimePlatform.Android) {
			_sqlite3_key(_connection, key, key.Length);
		}
		
	}
	
	#endregion
	
	#region Private Methods
	
	private IntPtr Prepare(string query)
	{
		IntPtr stmHandle;
		byte[] queryBytes = System.Text.Encoding.UTF8.GetBytes(query);
		int trueSize = queryBytes.Length;
		int resultCode = _sqlite3_prepare_v2(_connection, query, trueSize, out stmHandle, IntPtr.Zero);
		if (resultCode != SQLITE_OK)
		{
			IntPtr errorMsg = _sqlite3_errmsg(_connection);
			throw new SqliteException(resultCode,Marshal.PtrToStringAnsi(errorMsg) + " FullQuery=|" + query + "|");
		}
		
		return stmHandle;
	}
	
	private void Finalize(IntPtr stmHandle)
	{
		int resultCode = _sqlite3_finalize(stmHandle);
		if (resultCode != SQLITE_OK)
		{
			throw new SqliteException(resultCode,"Could not finalize SQL statement.");
		}
	}
	
	#endregion
}