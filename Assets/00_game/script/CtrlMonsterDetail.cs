using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlMonsterDetail : MonoBehaviour {

	#region SerializeField
	[SerializeField]
	protected UISprite m_sprMonster;

	[SerializeField]
	protected UILabel m_lbName;
	[SerializeField]
	protected UILabel m_lbUriage;
	[SerializeField]
	protected UILabel m_lbExp;
	[SerializeField]
	protected UILabel m_lbCost;
	[SerializeField]
	protected UILabel m_lbRarity;

	[SerializeField]
	protected ButtonBase m_buttonBaseWait;

	[SerializeField]
	protected ButtonBase m_buttonBaseClose;

	[SerializeField]
	protected CtrlDispHungry m_dispHungry;

	[SerializeField]
	protected CtrlDispMonsterDetailStaff m_ctrlDispMonsterDetailStaff;

	#endregion

	public enum STEP
	{
		NONE			= 0,
		IDLE			,
		CHECK_OJISAN	,

		END				,
		MAX				,
	}
	public STEP m_eStep;
	private STEP m_eStepPre;

	protected DataMonster m_dataMonster;
	private CtrlOjisanCheck m_ojisanCheck;


	private bool m_bIsEnd;
	public bool IsEnd(){
		return m_bIsEnd;
	}

	public void Initialize( int _iSerial ){
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;
		m_bIsEnd = false;

		m_dataMonster = GameMain.dbMonster.Select (_iSerial);
		int iCleanLevel = 0;
		int iMealLevel = 0;
		m_dataMonster.GetConditions (ref iCleanLevel, ref iMealLevel);
		m_dispHungry.Set (iMealLevel);

		DataMonsterMaster master_data = GameMain.dbMonsterMaster.Select (m_dataMonster.monster_id);

		List<DataStaff> staff_list = GameMain.dbStaff.Select (string.Format (" item_serial = {0}", m_dataMonster.item_serial));

		m_ctrlDispMonsterDetailStaff.Initialize (staff_list);


		m_lbName.text = master_data.name;

		m_lbUriage.text = UtilString.GetSyuunyuu (master_data.revenew_coin, master_data.revenew_interval);
		//m_lbUriage.text = master_data.revenew_coin.ToString() + " / " + master_data.revenew_interval.ToString();
		m_lbExp.text = master_data.revenew_exp.ToString();
		m_lbCost.text = master_data.cost.ToString();

		string strRarity = "";
		for (int i = 0; i < master_data.rare; i++) {
			strRarity += "★";
		}
		m_lbRarity.text = strRarity;//master_data.rare.ToString();

		string strSpriteName = GetSpriteName (m_dataMonster.monster_id);
		UIAtlas atlas = AtlasManager.Instance.GetAtlas (strSpriteName);
		m_sprMonster.atlas = atlas;
		m_sprMonster.spriteName = strSpriteName;


	}

	protected string GetSpriteName( int _iMonsterId ){
		string strRet = "";
		strRet = "chara" + _iMonsterId.ToString ();
		return strRet;
	}

	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}
		switch (m_eStep) {
		case STEP.IDLE:
			if (bInit) {
				if (m_buttonBaseWait != null) {
					m_buttonBaseWait.TriggerClear ();
				}
				if (m_buttonBaseClose != null) {
					m_buttonBaseClose.TriggerClear ();
				}
			}
			if (m_buttonBaseClose.ButtonPushed) {
				m_eStep = STEP.END;
			} else if (m_buttonBaseWait != null && m_buttonBaseWait.ButtonPushed) {
				m_eStep = STEP.CHECK_OJISAN;
			} else {
			}
			break;

		case STEP.CHECK_OJISAN:
			if (bInit) {
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefOjisanCheck", gameObject);
				m_ojisanCheck = objOjisan.GetComponent<CtrlOjisanCheck> ();
				m_ojisanCheck.Initialize ("この動物を\n待機室へ\n移動させます。");
			}
			if (m_ojisanCheck.IsYes ()) {


				CtrlFieldItem fielditem = GameMain.ParkRoot.GetFieldItem (m_dataMonster.item_serial);
				fielditem.RemoveMonster (m_dataMonster.monster_serial);

				GameMain.dbMonster.Update (m_dataMonster.monster_serial, 0 );
				GameMain.ListRefresh = true;
				m_bIsEnd = true;
				GameMain.Instance.HeaderRefresh ();

				Destroy (m_ojisanCheck.gameObject);

				// これ、別のところでもやってます
				List<DataMonster> monster_list = GameMain.dbMonster.Select (" item_serial = " + fielditem.m_dataItem.item_serial.ToString ());
				int iUseCost = 0;
				foreach (DataMonster monster in monster_list) {
					DataMonsterMaster data_master = GameMain.dbMonsterMaster.Select (monster.monster_id);
					iUseCost += data_master.cost;
				}
				CsvItemDetailData detail_data = DataManager.GetItemDetail (fielditem.m_dataItem.item_id, fielditem.m_dataItem.level);
				GameMain.Instance.m_iCostMax = detail_data.cost;
				GameMain.Instance.m_iCostNow = iUseCost;
				GameMain.Instance.m_iCostNokori = detail_data.cost - iUseCost;




				m_eStep = STEP.END;

			} else if (m_ojisanCheck.IsNo ()) {
				Destroy (m_ojisanCheck.gameObject);
				m_eStep = STEP.IDLE;
			} else {
			}

			break;
		case STEP.END:
			if (bInit) {
				m_bIsEnd = true;
			}
			break;
		default:
			break;
		}

	
	}
}











