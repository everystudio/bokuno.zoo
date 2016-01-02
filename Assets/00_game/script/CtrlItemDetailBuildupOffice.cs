using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlItemDetailBuildupOffice : CtrlItemDetailBuildupBase {

	protected override string GetMainText ()
	{
		return "empower_office";
		//return "この施設を増強しますか";
	}

	override protected void buildup(){

		int iCostPre = m_dataItem.cost_max;
		int iCostAfter = m_dataItem.cost_max;

		int iNextLevel = m_dataItem.level + 1;

		Dictionary< string , string > dict = new Dictionary< string , string > ();
		dict.Add( "level" , iNextLevel.ToString() ); 

		/*
		CsvItemDetailData itemdetail;
		foreach (CsvItemDetailData csvData in DataManager.csv_item_detail) {
			if (m_dataItem.item_id == csvData.item_id && iNextLevel == csvData.level) {
				iCostAfter = csvData.cost;
				dict.Add ("cost_max", csvData.cost.ToString ()); 
				dict.Add ("revenue", csvData.revenue_rate.ToString ()); 
			}
		}
		*/

		// 増えた分おコストを計算
		GameMain.Instance.m_iCostNokori += (iCostAfter - iCostPre);

		GameMain.dbItem.Update( m_dataItem.item_serial , dict );

		m_dataItem = GameMain.dbItem.Select (m_dataItem.item_serial);

		dispUpdate (m_dataItem, ref m_dataNext);
	}


}
