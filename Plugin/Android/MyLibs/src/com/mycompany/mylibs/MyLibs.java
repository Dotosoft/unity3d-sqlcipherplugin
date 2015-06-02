package com.mycompany.mylibs;

import android.content.Context;
import android.widget.Toast;

import com.unity3d.player.UnityPlayer;

public class MyLibs {

	public static void InitializeSQLCipher() {
		net.sqlcipher.database.SQLiteDatabase.loadLibs(UnityPlayer.currentActivity);
	}
	
	public static void ToastMessage(final String pMessage)
    {
    	UnityPlayer.currentActivity.runOnUiThread(new Runnable() {
			public void run() {
				Context context = UnityPlayer.currentActivity;
				CharSequence text = pMessage;
				int duration = Toast.LENGTH_SHORT;

				Toast toast = Toast.makeText(context, text, duration);
				toast.show();
			}
		});
    }
}
