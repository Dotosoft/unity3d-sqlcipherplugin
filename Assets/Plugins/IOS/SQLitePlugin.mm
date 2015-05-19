#import <sqlite3.h>
// #import <sqlite3ext.h>

extern "C" {
    int _sqlite3_open(const char *filename, sqlite3 **database) {
        int result = sqlite3_open(filename, database);
        return result;
    }
    
    int _sqlite3_close(sqlite3 *database) {
        return sqlite3_close(database);
    }
    
    int _sqlite3_prepare_v2(void **database, const char *queryString, int nByte, sqlite3_stmt **selectstmt,  const char *pzTail) {
        sqlite3* db = (sqlite3 *)database;
        return sqlite3_prepare_v2(db, queryString, nByte, selectstmt, nil);
    }
    
    int _sqlite3_step(void **stmHandleParam) {
        sqlite3_stmt* stmHandle = (sqlite3_stmt *)stmHandleParam;
        return sqlite3_step(stmHandle);
    }
    
    int _sqlite3_errcode(sqlite3 *db) {
        return sqlite3_errcode(db);
    }
    
    int _sqlite3_extended_errcode(sqlite3 *db) {
        return sqlite3_extended_errcode(db);
    }
    
    int _sqlite3_changes(sqlite3 *db) {
        return sqlite3_changes(db);
    }
    
    int _sqlite3_finalize(sqlite3_stmt *stmHandle) {
        return sqlite3_finalize(stmHandle);
    }
    
    const char *_sqlite3_errmsg(sqlite3 *db) {
        return sqlite3_errmsg(db);
    }
    
    int _sqlite3_column_count(sqlite3_stmt *stmHandle) {
        return sqlite3_column_count(stmHandle);
    }
    
    const char *_sqlite3_column_name(sqlite3_stmt* stmHandle, int N) {
        return sqlite3_column_name(stmHandle, N);
    }
    
    int _sqlite3_column_type(sqlite3_stmt* stmHandle, int iCol) {
        return sqlite3_column_type(stmHandle, iCol);
    }
    
    int _sqlite3_column_int(sqlite3_stmt* stmHandle, int iCol) {
        return sqlite3_column_int(stmHandle, iCol);
    }
    
    long _sqlite3_column_int64(sqlite3_stmt* stmHandle, int iCol) {
        return sqlite3_column_int64(stmHandle, iCol);
    }
    
    const unsigned char *_sqlite3_column_text(sqlite3_stmt* stmHandle, int iCol) {
        return sqlite3_column_text(stmHandle, iCol);
    }
    
    double _sqlite3_column_double(sqlite3_stmt* stmHandle, int iCol) {
        return sqlite3_column_double(stmHandle, iCol);
    }
    
    const void *_sqlite3_column_blob (sqlite3_stmt* stmHandle, int iCol) {
        return sqlite3_column_blob(stmHandle, iCol);
    }
    
    int _sqlite3_column_bytes (sqlite3_stmt* stmHandle, int iCol) {
        return sqlite3_column_bytes(stmHandle, iCol);
    }
    
    int _sqlite3_key(void **database, const char *key, int len) {
        sqlite3* db = (sqlite3 *)database;
        int result = sqlite3_key(db, key, len);
        
        return result;
    }
}