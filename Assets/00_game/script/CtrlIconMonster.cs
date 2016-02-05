using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlIconMonster : CtrlIconBase {

	public int m_iMealLevel;
	public int m_iCleanLevel;
	public void Initialize( UISprite _sprite , DataMonster _dataMonster , int _iSize ){

		SetSize (_iSize);

		myTransform.localPosition = GetMovePos ();
		m_sprIcon = _sprite;
		m_dataMonster = _dataMonster;

		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

		AnimationIdol (true);
	}

	public GameObject m_goDust;
	//public GameObject m_goMeal;

	public void createDust (){
		if (m_goDust == null) {
			m_goDust = PrefabManager.Instance.MakeObject ("prefab/PrefDust", gameObject);
			m_goDust.transform.parent = gameObject.transform.parent;
			m_goDust.transform.localPosition = GetMovePos ();
			m_goDust.transform.localPosition *= 0.8f;

			m_goDust.transform.localPosition += 0.2f * (Define.CELL_X_DIR + Define.CELL_Y_DIR);

			m_goDust.GetComponent<UISprite> ().depth = m_sprIcon.depth-1;
		}
	}
	override public bool CleanDust(){
		bool bRet = false;
		int iCleanLevel = 0;
		int iMealLevel = 0;
		m_dataMonster.GetConditions (ref iCleanLevel, ref iMealLevel);
		if (iCleanLevel < 5) {
			bRet = true;
			Destroy (m_goDust);

			Dictionary< string , string > dict = new Dictionary< string , string > ();
			dict.Add ("clean_time", "\"" + TimeManager.StrGetTime () + "\"");
			GameMain.dbMonster.Update (m_dataMonster.monster_serial, dict);
			m_dataMonster = GameMain.dbMonster.Select (m_dataMonster.monster_serial);
			/*
			string strNow = TimeManager.StrNow ();
			Dictionary< string , string > dict = new Dictionary< string , string > ();
			dict.Add ("collect_time", "\"" + strNow + "\"");
			GameMain.dbMonster.Update (monster_serial, dict );
			*/


		}
		return bRet;
	}

	override public bool Meal(){
		bool bRet = false;
		int iCleanLevel = 0;
		int iMealLevel = 0;
		m_dataMonster.GetConditions (ref iCleanLevel, ref iMealLevel);

		if (iMealLevel < 5) {
			m_eStep = STEP.EAT;

			Dictionary< string , string > dict = new Dictionary< string , string > ();
			dict.Add ("meal_time", "\"" + TimeManager.StrGetTime () + "\"");
			GameMain.dbMonster.Update (m_dataMonster.monster_serial, dict);
			m_dataMonster = GameMain.dbMonster.Select (m_dataMonster.monster_serial);
			bRet = true;
		}
		return bRet;
	}

	public int m_iLocalCondition;

	override public void update_idle( bool _bInit ){
		if (_bInit) {
			// 空腹状態
			// 新しくする
			m_dataMonster = GameMain.dbMonster.Select (m_dataMonster.monster_serial);

			m_iLocalCondition = -1;

			int iCleanLevel = 0;
			int iMealLevel = 0;
			m_dataMonster.GetConditions (ref iCleanLevel, ref iMealLevel);
			m_iCleanLevel = iCleanLevel;
			m_iMealLevel = iMealLevel;

			/*
			double clean_time = TimeManager.Instance.GetDiffNow (m_dataMonster.clean_time).TotalSeconds * -1.0d;
			double meal_time = TimeManager.Instance.GetDiffNow (m_dataMonster.meal_time).TotalSeconds * -1.0d;

			foreach (CsvTimeData time_data in DataManager.csv_time) {
				if (time_data.type == 1) {
					if (clean_time < time_data.delta_time) {
						if (iCleanLevel < time_data.now) {
							iCleanLevel = time_data.now;
						}
					}

				} else if (time_data.type == 2) {
					if (meal_time < time_data.delta_time) {
						if (iMealLevel < time_data.now) {
							iMealLevel = time_data.now;
						}
					}
				} else {
				}
			}
			*/

			if (iCleanLevel < 1 && m_dataMonster.condition == (int)Define.Monster.CONDITION.FINE ) {
				Dictionary< string , string > dict = new Dictionary< string , string > ();
				dict.Add ("condition", ((int)(Define.Monster.CONDITION.SICK)).ToString ()); 
				GameMain.dbMonster.Update (m_dataMonster.monster_serial, dict);
				m_dataMonster = GameMain.dbMonster.Select (m_dataMonster.monster_serial);
			}

			//if (m_dataMonster.condition == (int)(Define.Monster.CONDITION.SICK) && iCleanLevel == 0) {
			if (m_dataMonster.condition == (int)(Define.Monster.CONDITION.SICK)) {
				m_fukidashi.SetStatus (CtrlIconFukidashi.STATUS.SICK2 ,m_sprIcon.depth);
			//} else if (m_dataMonster.condition == (int)(Define.Monster.CONDITION.SICK) && iCleanLevel == 1) {
			} else if (m_dataMonster.condition == (int)(Define.Monster.CONDITION.SICK) ) {
				m_fukidashi.SetStatus (CtrlIconFukidashi.STATUS.SICK1,m_sprIcon.depth);
			} else if (iCleanLevel < 3 ) {
				m_fukidashi.SetStatus (CtrlIconFukidashi.STATUS.DUST,m_sprIcon.depth);
			} else if (iMealLevel < 3 ) {
				m_fukidashi.SetStatus (CtrlIconFukidashi.STATUS.HUNGRY,m_sprIcon.depth);
			} else {
				m_fukidashi.SetStatus (CtrlIconFukidashi.STATUS.NONE,m_sprIcon.depth);
			}

			if (iCleanLevel != 5) {
				createDust ();
			}
		}
	}

	override public void AnimationIdol(bool _bInit){
		if (_bInit) {
			string strName = "chara" + m_dataMonster.monster_id.ToString () + "_eat1";
			m_sprIcon.atlas = AtlasManager.Instance.GetAtlas (strName);
			m_sprIcon.spriteName = strName;
		}
	}

	override public void AnimationMove(bool _bInit){
		if (_bInit) {
			m_fAnimationTime = 0.0f;
			m_iAnimationFrame = 0;
			m_fAnimationInterval = 0.2f;
		}

		m_fAnimationTime += Time.deltaTime;
		if (m_fAnimationInterval < m_fAnimationTime) {
			m_fAnimationTime -= m_fAnimationInterval;


			m_iAnimationFrame += 1;
			m_iAnimationFrame %= 2;		// トータル２フレーム

			int iDispFrame = m_iAnimationFrame + 1;

			m_sprIcon.spriteName = "chara" + m_dataMonster.monster_id.ToString () + "_move" + iDispFrame.ToString();
		}
		return;
	}

	private float m_fEatSoundTimer;
	override public void AnimationEat(bool _bInit , int _iMealId ){
		if (_bInit) {
			m_fAnimationTime = 0.0f;
			m_iAnimationFrame = 0;
			m_fAnimationInterval = 0.8f;
			m_fEatSoundTimer = 0.0f;
		}

		m_fEatSoundTimer += Time.deltaTime;
		if (1.0f < m_fEatSoundTimer) {
			m_fEatSoundTimer -= 1.0f;
			SoundManager.Instance.PlaySE ("se_eating");
		}

		m_fAnimationTime += Time.deltaTime;
		if (m_fAnimationInterval < m_fAnimationTime) {
			m_fAnimationTime -= m_fAnimationInterval;

			m_iAnimationFrame += 1;
			m_iAnimationFrame %= 2;		// トータル２フレーム

			int iDispFrame = m_iAnimationFrame + 1;

			m_sprIcon.spriteName = "chara" + m_dataMonster.monster_id.ToString () + "_eat" + iDispFrame.ToString();
		}
		return;
	}



}


















