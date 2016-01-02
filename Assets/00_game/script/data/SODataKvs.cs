using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SODataKvs : SODataBase<KVSData> {

	protected override void save ()
	{
		StreamWriter sw = Textreader.Open (string.Format ("{0}.csv", DBKvs.FILE_NAME));
		string strHead = string.Format ("{0},{1}",
			"key",
			"value"
		);

		Textreader.Write (sw, strHead);
		foreach (KVSData data in list) {
			string strData = string.Format ("{0},{1}",
				data.key,
				data.value
			);
			//Debug.LogError (strData);
			//Textreader.write (strData);
			Textreader.Write (sw, strData);
			//Textreader.SaveWrite (string.Format ("{0}.csv", DBItem.FILE_NAME), strData);
		}
		Textreader.Close( sw );
		return;
	}


}
