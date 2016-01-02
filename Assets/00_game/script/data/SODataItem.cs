using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SODataItem : SODataBase<DataItem> {

	protected override void save ()
	{
		StreamWriter sw = Textreader.Open (string.Format ("{0}.csv", DBItem.FILE_NAME));

		string strHead = string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}",
			"item_serial",
			"item_id",
			"category",
			"level",
			"status",
			"cost",
			"cost_max",
			"revenue_rate",
			"x",
			"y",
			"width",
			"height",
			"collect_time",
			"create_time"
		);

		Textreader.Write (sw, strHead);

		foreach (DataItem data in list) {
			string strData = string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13}",
				                 data.item_serial,
				                 data.item_id,
				                 data.category,
				                 data.level,
				                 data.status,
				                 data.cost,
				                 data.cost_max,
				                 data.revenue_rate,
				                 data.x,
				                 data.y,
				                 data.width,
				                 data.height,
				                 data.collect_time,
				                 data.create_time
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
