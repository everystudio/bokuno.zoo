using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using System.Collections;
using System.Collections.Generic;
using System.IO;

public class SODataParam{

	public void Load(Dictionary<string, string> param)
	{
		foreach (string key in param.Keys) {
			SetField (key.Replace( "\"" , "" ), param [key]);
		}
	}

	private void SetField(string key, string value)
	{
		//Debug.Log (key.ToString() + ":" + value.ToString());
		PropertyInfo propertyInfo = this.GetType().GetProperty(key, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

		if(propertyInfo.PropertyType == typeof(int))         propertyInfo.SetValue(this, int.Parse(value), null);
		else if(propertyInfo.PropertyType == typeof(string)) propertyInfo.SetValue(this, value.Replace( "\"" , "" ), null);
		else if(propertyInfo.PropertyType == typeof(double)) propertyInfo.SetValue(this, double.Parse(value), null);
		// 他の型にも対応させたいときには適当にここに。enumとかもどうにかなりそう。
	}
}


public abstract class SODataBase<T> : MonoBehaviour where T : SODataParam, new() {

	// 作る必要はないんだけど一応ね
	public List<T> list = new List<T>();
	virtual public void Load(string _strFilename)
	{
		list = new List<T> ();

		string file = string.Format ("{0}.csv", _strFilename);
		string pathDB = System.IO.Path.Combine (Application.persistentDataPath, file);
		FileInfo fi = new FileInfo(pathDB);
		StreamReader sr = new StreamReader(fi.OpenRead());

		int iLoop = 0;
		string strFirst = sr.ReadLine ();
		var headerElements = strFirst.Split (',');
		/*
		foreach (string str_key in headerElements) {
			Debug.LogError (str_key);
		}
		*/

		while( sr.Peek() != -1 ){
			string strLine = sr.ReadLine ();
			ParseLine (strLine, headerElements);
		}
		sr.Close();
	}

	private void ParseLine(string line, string[] headerElements)
	{
		var elements = line.Split(',');
		if(elements.Length == 1) return;
		if(elements.Length != headerElements.Length)
		{
			Debug.LogWarning(string.Format("can't load: {0}", line));
			return;
		}

		var param = new Dictionary<string, string>();
		for (int i = 0; i < elements.Length; ++i) {
			param.Add (headerElements [i], elements [i]);
		}
		var master = new T();
		master.Load(param);
		list.Add(master);
	}
	public void Save(){
		save ();
	}
	abstract protected void save();

}
