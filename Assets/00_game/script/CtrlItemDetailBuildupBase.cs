using UnityEngine;
using System.Collections;
using System.Collections.Generic;

abstract public class CtrlItemDetailBuildupBase : CtrlItemDetailBase {

	abstract protected string GetMainText ();

	#region SerializeField
	[SerializeField]
	private UISprite m_sprMain;
	[SerializeField]
	private UILabel m_lbMain;
	[SerializeField]
	private UILabel m_lbCostNow;
	[SerializeField]
	private UILabel m_lbCostAfter;

	[SerializeField]
	private UILabel m_lbBonusNow;
	[SerializeField]
	private UILabel m_lbBonusAfter;

	[SerializeField]
	private ButtonBase m_btnBuildup;

	[SerializeField]
	private GameObject m_goUnder;
	[SerializeField]
	private UILabel m_lbNeedGold;
	[SerializeField]
	private UILabel m_lbGetExp;

	#endregion

	public enum STEP {
		NONE			= 0,
		IDLE			,

		CHECK_BUILDUP	,
		GOLD_FUSOKU		,

		BUILDUP			,
		MAX				,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;
	public CtrlOjisanCheck m_ojisanCheck;

	public CsvItemDetailData m_dataNow;
	public CsvItemDetailData m_dataNext;

	virtual protected void dispUpdate( DataItem _data , ref CsvItemDetailData _next ){
		_next = null;

		m_lbNeedGold.text = "-";
		m_lbGetExp.text = "-";
		m_lbCostAfter.text = "-";
		m_lbBonusAfter.text = "-";

		foreach (CsvItemDetailData data in DataManager.csv_item_detail) {
			if (data.item_id == _data.item_id) {
				if (_data.level == data.level) {
					m_lbCostNow.text = data.cost.ToString ();
					m_lbBonusNow.text = string.Format( "{0}%" , data.revenue_rate);
					m_dataNow = data;

				} else if (_data.level + 1 == data.level) {
					m_lbCostAfter.text = data.cost.ToString ();
					m_lbBonusAfter.text = string.Format ("{0}%", data.revenue_rate);
					_next = data;

				} else {
					;// スルー
				}
			}
		}

		m_lbMain.transform.localPosition = new Vector3 (0.0f, 150.0f, 0.0f);
		if (_next != null) {
			//m_lbMain.text = GetMainText ();
			m_sprMain.spriteName = GetMainText ();
			m_goUnder.SetActive (true);
			m_lbNeedGold.text = string.Format ("{0}G", m_dataNext.coin);
			m_lbGetExp.text = m_dataNext.get_exp.ToString ();
			m_btnBuildup.gameObject.SetActive (true);
		} else {
			//m_lbMain.text = "増強☆完了";
			m_sprMain.spriteName = "empower_done";
			// やっぱ表示はする
			m_goUnder.SetActive (true);
			m_btnBuildup.gameObject.SetActive (false);
			m_lbNeedGold.text = "-";
			m_lbGetExp.text = "-";
		}
		return;
	}
	abstract protected void buildup ();

	override protected void initialize(){
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

		dispUpdate (m_dataItem , ref m_dataNext );
		return;
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
				Debug.Log (DataManager.user.m_iGold);
				m_btnBuildup.TriggerClear ();
			}
			if (m_btnBuildup.ButtonPushed || GameMain.Instance.TutorialBuildup) {
				m_btnBuildup.TriggerClear ();
				SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
				// お金が足りるなら確認

				if (m_dataNext.coin <= DataManager.user.m_iGold || GameMain.Instance.TutorialBuildup) {
					GameMain.Instance.TutorialBuildup = false;
					m_eStep = STEP.CHECK_BUILDUP;
				} else {
					m_eStep = STEP.GOLD_FUSOKU;
				}
			}
			break;

		case STEP.CHECK_BUILDUP:
			if (bInit) {
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefOjisanCheck", gameObject.transform.parent.parent.gameObject);
				m_ojisanCheck = objOjisan.GetComponent<CtrlOjisanCheck> ();
				string strDisp = "";
				strDisp = string.Format("本当に増強しますか\n\n{0}G → [FFD900]{1}[-]G\n" , DataManager.user.m_iGold , DataManager.user.m_iGold - m_dataNext.coin);
				m_ojisanCheck.Initialize (strDisp);
			}
			if (m_ojisanCheck.IsYes ()) {
				Destroy (m_ojisanCheck.gameObject);
				SoundManager.Instance.PlaySE (SoundName.BUILDUP);
				m_eStep = STEP.BUILDUP;
			} else if (m_ojisanCheck.IsNo ()) {
				Destroy (m_ojisanCheck.gameObject);
				SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
				m_eStep = STEP.IDLE;
			} else {
			}
			break;

		case STEP.GOLD_FUSOKU:
			if (bInit) {
				GameObject objOjisan = PrefabManager.Instance.MakeObject ("prefab/PrefOjisanCheck", gameObject.transform.parent.parent.gameObject);
				m_ojisanCheck = objOjisan.GetComponent<CtrlOjisanCheck> ();
				string strDisp = "";
				strDisp = string.Format("増強費用は\n[FFD900]{0}G[-]必要です\n\n[FFD900]{1}G[-]不足しています" , m_dataNext.coin , m_dataNext.coin - DataManager.user.m_iGold);
				m_ojisanCheck.Initialize (strDisp, true);
			}
			if (m_ojisanCheck.IsYes ()) {
				Destroy (m_ojisanCheck.gameObject);
				SoundManager.Instance.PlaySE (SoundName.BUTTON_PUSH);
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.BUILDUP:
			if( true ){

				// 増強費用
				DataManager.user.AddGold (m_dataNext.coin * -1);

				// 経験値獲得
				DataManager.user.AddExp (m_dataNext.get_exp);
				buildup ();
			}

			m_eStep = STEP.IDLE;
			break;
		case STEP.MAX:
		default:
			break;
		}

	}


}















