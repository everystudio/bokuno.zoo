using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SODataMonsterMaster : SODataBase<DataMonsterMaster> {

	protected override void save ()
	{
		StreamWriter sw = Textreader.Open (string.Format ("{0}.csv", DBMonsterMaster.FILE_NAME));

		string strHead = string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
			"monster_id",
			"name",
			"description_cell",
			"cost",
			"fill",
			"dust",
			"coin",
			"ticket",
			"revenew_coin",
			"revenew_exp",
			"revenew_interval",
			"open_work_id",
			"description_book",
			"size",
			"rare",
			"status"
		);

		Textreader.Write (sw, strHead);
		foreach (DataMonsterMaster data in list) {
			string strData = string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15}",
				data.monster_id,
				data.name,
				data.description_cell,
				data.cost,
				data.fill,
				data.dust,
				data.coin,
				data.ticket,
				data.revenew_coin,
				data.revenew_exp,
				data.revenew_interval,
				data.open_work_id,
				data.description_book,
				data.size,
				data.rare,
				data.status
			);
			Textreader.Write (sw, strData);
			//Textreader.SaveWrite (string.Format ("{0}.csv", DBItem.FILE_NAME), strData);
		}
		Textreader.Close( sw );
		return;
	}

}
