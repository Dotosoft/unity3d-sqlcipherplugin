using System.Collections;
using System;
using UnityORM;

[Serializable()]
public class UserData{
	public long ID;
	
	public string Name;
	
	public int Age;
	
	[Ignore]
	public string Hoge;
	
	[MetaInfoAttirbute(NameInJSON = "address_data")]
	public string AddressData;
	
	public DateTime LastUpdated {get;set;}
	
	public Nested NestedClass = new Nested();
	
	public override string ToString ()
	{
		return "ID:" + ID + " Name:" + Name + " Hoge:" + Hoge + " Age:" + Age + " Address:" + AddressData +
			" LastUpdated:" + LastUpdated + " NestedClass:" + NestedClass;
	}
}

[Serializable()]
public class Nested {
	public int Hoge;
	public string Fuga;
	public override string ToString ()
	{
		return "Hoge : " + Hoge + " Fuga:" + Fuga;
	}
}