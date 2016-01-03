using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlItemDetailCage : CtrlItemDetailBase {
	#region SerializeField
	[SerializeField]
	private UILabel m_lbUriageNow;
	[SerializeField]
	private UILabel m_lbUriageMax;
	[SerializeField]
	private UILabel m_lbCostNow;
	[SerializeField]
	private UILabel m_lbCostMax;

	[SerializeField]
	private ButtonBase m_btnClean;
	[SerializeField]
	private ButtonBase m_btnMeal;

	[SerializeField]
	private AppearUpAnimation m_effClean;
	[SerializeField]
	private AppearUpAnimation m_effMeal;
	[SerializeField]
	private AppearUpAnimation m_effTutorialSetting;


	#endregion


	private bool m_bRemove;

	override protected void remove(){
		m_bRemove = true;
		return;
	}

	void dispRefresh(){
		//Debug.LogError ("dispRefresh");
		int iUriageMax = m_ctrlFieldItem.m_dataItem.GetUriagePerHour ();
		/*
		List<DataMonster > monster_list = GameMain.dbMonster.Select ( " item_serial = " + m_ctrlFieldItem.m_dataItem.item_serial.ToString() );
		int iUriageMax = 0;
		foreach (DataMonster monster in monster_list) {
			CsvMonsterData monster_csv = DataManager.GetMonster (monster.monster_id);

			int iCount = 3600 / monster_csv.revenew_interval;
			iUriageMax += monster_csv.revenew_coin * iCount;
		}
		*/

		int iCollectGold = 0;
		int iCollectExp= 0;

		m_lbUriageNow.text = string.Format( "{0}G" , m_ctrlFieldItem.m_dataItem.GetCollect (false , out iCollectGold , out iCollectExp ) );
		m_lbUriageMax.text = string.Format( "{0}G" , iUriageMax );
	}

	override protected void initialize(){

		List<DataMonster> monster_list = GameMain.dbMonster.Select (" item_serial = " + m_ctrlFieldItem.m_dataItem.item_serial.ToString ());
		int iUseCost = 0;
		foreach (DataMonster monster in monster_list) {
			DataMonsterMaster data_master = GameMain.dbMonsterMaster.Select (monster.monster_id);
			iUseCost += data_master.cost;
		}
		CsvItemDetailData detail_data = DataManager.GetItemDetail (m_dataItem.item_id, m_dataItem.level);
		GameMain.Instance.m_iCostMax = detail_data.cost;
		GameMain.Instance.m_iCostNow = iUseCost;
		GameMain.Instance.m_iCostNokori = detail_data.cost - iUseCost;

		// m_iMainItemSerial was set from parent Initialize

		m_bRemove = false;

		m_ctrlFieldItem.gameObject.transform.parent = gameObject.transform;

		float fScale = 1.0f / GameMain.ParkRoot.gameObject.transform.localScale.x;
		m_ctrlFieldItem.gameObject.transform.localScale = new Vector3 (fScale, fScale, fScale);
		m_ctrlFieldItem.gameObject.transform.localScale = Vector3.one;

		m_ctrlFieldItem.gameObject.transform.localPosition = new Vector3 (0.0f, -240.0f, 0.0f);

		if (6 <= m_ctrlFieldItem.m_dataItemMaster.size) {
			m_ctrlFieldItem.gameObject.transform.localPosition = new Vector3 (0.0f, -415.0f, 0.0f);
		}
		// こっちの画面であ後ろの方に移動しておいてください
		m_ctrlFieldItem.ItemSprite.depth = -10;

		m_lbCostNow.text = GameMain.Instance.m_iCostNow.ToString ();
		m_lbCostMax.text = GameMain.Instance.m_iCostMax.ToString ();

		m_btnClean.TriggerClear ();
		m_btnMeal.TriggerClear ();

		dispRefresh ();

		return;
	}



	override protected void close(){

		if (m_bRemove == false) {
			// 閉じる的な終了時

			m_ctrlFieldItem.gameObject.transform.parent = GameMain.ParkRoot.transform;
			m_ctrlFieldItem.gameObject.transform.localScale = Vector3.one;
			m_ctrlFieldItem.SetPos (m_dataItem.x, m_dataItem.y);
		} else {
			// 閉じる的な終了時
			Destroy (m_ctrlFieldItem.gameObject);
		}

		// 片付けルートは別にする必要あり

		return;
	}

	// Update is called once per frame
	void Update () {

		if (m_btnClean.ButtonPushed || 0 < GameMain.Instance.SwitchClean) {
			GameMain.Instance.SwitchClean = 0;
			m_btnClean.TriggerClear ();
			if (m_ctrlFieldItem.Clean ()) {
				SoundManager.Instance.PlaySE ("se_cleanup");
				m_effClean.Popup ();
			}
		}
		if (m_btnMeal.ButtonPushed || 0 < GameMain.Instance.SwitchFood) {
			GameMain.Instance.SwitchFood = 0;
			m_btnMeal.TriggerClear ();
			if (m_ctrlFieldItem.Meal ()) {
				SoundManager.Instance.PlaySE ("se_eat");
				m_effMeal.Popup ();
			}
		}
		if (0 < GameMain.Instance.SwitchSetting) {
			GameMain.Instance.SwitchSetting = 0;
			m_effTutorialSetting.Popup ();
		}
	
	}
}










