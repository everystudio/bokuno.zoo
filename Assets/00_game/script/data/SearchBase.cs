using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;

public class SearchBase : SODataParam{

	public void Set(Dictionary<string , string > _dict){

		foreach (string key in _dict.Keys) {
			PropertyInfo propertyInfo = GetType ().GetProperty (key);
			if (propertyInfo.PropertyType == typeof(int)) {
				int iValue = int.Parse (_dict [key]);
				propertyInfo.SetValue (this, iValue, null);
			} else if (propertyInfo.PropertyType == typeof(string)) {
				propertyInfo.SetValue (this, _dict [key].Replace ("\"", ""), null);
			} else if (propertyInfo.PropertyType == typeof(double)) {
				propertyInfo.SetValue (this, double.Parse (_dict [key]), null);
			} else if (propertyInfo.PropertyType == typeof(float)) {
				propertyInfo.SetValue (this, float.Parse (_dict [key]), null);
			}
			else {
				Debug.LogError ("error type unknown");
			}
		}
	}


	public bool Equals( string _strWhere ){
		//Debug.Log (_strWhere);
		string[] test = _strWhere.Trim().Split (' ');
		int count = 0;
		foreach (string check in test) {
			//Debug.Log (string.Format ("{0}:{1}", count, check));
			count += 1;
		}

		bool bRet = true;

		for (int i = 0; i < test.Length; i+=4 ) {
			//Debug.Log (test [i]);
			PropertyInfo propertyInfo = GetType ().GetProperty (test [i]);
			if (propertyInfo.PropertyType == typeof(int)) {
				int intparam = (int)propertyInfo.GetValue (this, null);
				string strJudge = test [i + 1];
				int intcheck = int.Parse(test[i+2]);
				if (strJudge.Equals ("=")) {
					if (intparam != intcheck) {
						bRet = false;
					}
				} else if (strJudge.Equals ("!=")) {
					if (intparam == intcheck) {
						bRet = false;
					}
				} else {
				}
			}
		}
		return bRet;
	}}
