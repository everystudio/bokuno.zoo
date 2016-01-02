using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SODataWork : SODataBase<DataWork> {

	protected override void save ()
	{
		StreamWriter sw = Textreader.Open (string.Format ("{0}.csv", DBWork.FILE_NAME));

		string strHead = string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}",
			"work_id",
			"status",
			"title",
			"description",
			"type",
			"level",
			"appear_work_id",
			"exp",
			"difficulty",
			"prize_ticket",
			"prize_coin",
			"prize_monster",
			"mission_level",
			"mission_monster",
			"mission_staff",
			"mission_item",
			"mission_collect",
			"mission_tweet",
			"mission_login",
			"mission_num"
		);

		Textreader.Write (sw, strHead);
		foreach (DataWork data in list) {
			string strData = string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19}",
				data.work_id,
				data.status,
				data.title,
				data.description,
				data.type,
				data.level,
				data.appear_work_id,
				data.exp,
				data.difficulty,
				data.prize_ticket,
				data.prize_coin,
				data.prize_monster,
				data.mission_level,
				data.mission_monster,
				data.mission_staff,
				data.mission_item,
				data.mission_collect,
				data.mission_tweet,
				data.mission_login,
				data.mission_num
			);
			Textreader.Write (sw, strData);
			//Textreader.SaveWrite (string.Format ("{0}.csv", DBItem.FILE_NAME), strData);
		}
		Textreader.Close( sw );
		return;
	}

}
