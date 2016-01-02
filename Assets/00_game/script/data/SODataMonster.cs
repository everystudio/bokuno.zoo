using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SODataMonster : SODataBase<DataMonster> {

	protected override void save ()
	{
		StreamWriter sw = Textreader.Open (string.Format ("{0}.csv", DBMonster.FILE_NAME));

		string strHead = string.Format ("{0},{1},{2},{3},{4},{5},{6}",
			"monster_serial",
			"monster_id",
			"item_serial",
			"condition",
			"collect_time",
			"meal_time",
			"clean_time"
		);

		Textreader.Write (sw, strHead);
		foreach (DataMonster data in list) {
			string strData = string.Format ("{0},{1},{2},{3},{4},{5},{6}",
				data.monster_serial,
				data.monster_id,
				data.item_serial,
				data.condition,
				data.collect_time,
				data.meal_time,
				data.clean_time
			);
			Textreader.Write (sw, strData);
			//Textreader.SaveWrite (string.Format ("{0}.csv", DBItem.FILE_NAME), strData);
		}
		Textreader.Close( sw );
		return;
	}
}
