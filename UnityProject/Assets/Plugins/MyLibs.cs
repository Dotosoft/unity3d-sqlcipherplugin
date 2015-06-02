using UnityEngine;
using System.Collections;

public static class MyLibs {
	#if UNITY_ANDROID
	static AndroidJavaClass MyLib;

	private static AndroidJavaClass GetPluginClass() {
		if (MyLib == null) {
			MyLib = new AndroidJavaClass ("com.mycompany.mylibs.MyLibs");
		}
		return MyLib;
	}

	public static void InitializeSQLCipher() {
		GetPluginClass().CallStatic("InitializeSQLCipher");
	}

	public static void ToastMessage(string text) {
		GetPluginClass().CallStatic("ToastMessage", text);
	}
	#endif
}
