using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlItemDetailBuildupCage : CtrlItemDetailBuildupBase {

	protected override string GetMainText ()
	{
		return "empower";
		//return "この檻を増強しますか";
	}

	override protected void buildup(){
		CsvItemDetailData detail_pre_data = DataManager.GetItemDetail (m_dataItem.item_id, m_dataItem.level);

		int iCostPre = detail_pre_data.cost;
		int iCostAfter = detail_pre_data.cost;

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
		CsvItemDetailData detail_data = DataManager.GetItemDetail (m_dataItem.item_id, iNextLevel);
		iCostAfter = detail_data.cost;

		int iAddCost = iCostAfter - iCostPre;
		if (0 < iAddCost) {
			GameMain.Instance.m_iCostNokori += iAddCost;
		}
		//Debug.Log (string.Format ("cost pre={0} after={1} nokori={2}", iCostPre, iCostAfter, GameMain.Instance.m_iCostNokori));
		GameMain.Instance.m_iCostMax = detail_data.cost;


		GameMain.dbItem.Update( m_dataItem.item_serial , dict );

		m_dataItem = GameMain.dbItem.Select (m_dataItem.item_serial);

		dispUpdate (m_dataItem, ref m_dataNext);
	}

}



















