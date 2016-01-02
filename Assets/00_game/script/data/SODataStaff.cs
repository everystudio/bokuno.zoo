using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SODataStaff : SODataBase<DataStaff> {

	protected override void save ()
	{
		StreamWriter sw = Textreader.Open (string.Format ("{0}.csv", DBStaff.FILE_NAME));

		string strHead = string.Format ("{0},{1},{2},{3},{4},{5}",
			"staff_serial",
			"office_serial",
			"staff_id",
			"item_serial",
			"setting_time",
			"create_time"
		);

		Textreader.Write (sw, strHead);
		foreach (DataStaff data in list) {
			string strData = string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}",
				data.staff_serial,
				data.office_serial,
				data.staff_id,
				data.item_serial,
				data.setting_time,
				data.create_time
			);
			Textreader.Write (sw, strData);
			//Textreader.SaveWrite (string.Format ("{0}.csv", DBItem.FILE_NAME), strData);
		}
		Textreader.Close( sw );
		return;
	}
}
