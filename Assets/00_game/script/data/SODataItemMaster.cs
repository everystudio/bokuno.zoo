using UnityEngine;
using System.IO;
using System.Collections;
using System.Collections.Generic;

public class SODataItemMaster : SODataBase<DataItemMaster> {

	protected override void save ()
	{
		StreamWriter sw = Textreader.Open (string.Format ("{0}.csv", DBItemMaster.FILE_NAME));

		string strHead = string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21}",
			"item_id",
			"status",
			"name",
			"category",
			"type",
			"cell_type",
			"description",
			"need_coin",
			"need_ticket",
			"need_money",
			"size",
			"cost",
			"area",
			"revenue",
			"revenue_interval",
			"revenue_up",
			"production_time",
			"setting_limit",
			"sub_parts_id",
			"open_item_id",
			"revenue_up2",
			"add_coin"
		);

		Textreader.Write (sw, strHead);
		foreach (DataItemMaster data in list) {
			string strData = string.Format ("{0},{1},{2},{3},{4},{5},{6},{7},{8},{9},{10},{11},{12},{13},{14},{15},{16},{17},{18},{19},{20},{21}",
				data.item_id,
				data.status,
				data.name,
				data.category,
				data.type,
				data.cell_type,
				data.description,
				data.need_coin,
				data.need_ticket,
				data.need_money,
				data.size,
				data.cost,
				data.area,
				data.revenue,
				data.revenue_interval,
				data.revenue_up,
				data.production_time,
				data.setting_limit,
				data.sub_parts_id,
				data.open_item_id,
				data.revenue_up2,
				data.add_coin
			);
			Textreader.Write (sw, strData);
			//Textreader.SaveWrite (string.Format ("{0}.csv", DBItem.FILE_NAME), strData);
		}
		Textreader.Close( sw );
		return;
	}

}
