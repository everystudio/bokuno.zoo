using UnityEngine;
using System.Collections;

public class CtrlIconStaff : CtrlIconBase {

	public void Initialize( UISprite _sprite , DataStaff _dataStaff , int _iSize ){
		SetSize (_iSize);
		myTransform.localPosition = GetMovePos ();
		m_sprIcon = _sprite;
		m_dataStaff = _dataStaff;
		Debug.LogError (m_sprIcon.depth);
		m_sprIcon.depth += 1;
		Debug.LogError (m_sprIcon.depth);
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

		// しょっぱなには掃除とかをする
		m_iCheckCount = 100;
		AnimationIdol (true);

		m_csvStaffData = DataManager.GetStaff (m_dataStaff.staff_id);
		m_fieldItem = GameMain.ParkRoot.GetFieldItem (m_dataStaff.item_serial);

	}
	override public void SetDepth( int _iDepth ){
		m_sprIcon.depth = _iDepth + 2;
	}

	public int m_iCheckCount;

	public CsvStaffData m_csvStaffData;
	public CtrlFieldItem m_fieldItem;

	public override void update_idle (bool _bInit)
	{
		if (_bInit) {
			m_iCheckCount += 1;

			if (30 < m_iCheckCount) {
				m_iCheckCount = 0;
				if (m_csvStaffData.effect_param == (int)Define.Staff.EFFECT_PARAM.CLEAN || m_csvStaffData.effect_param == (int)Define.Staff.EFFECT_PARAM.MEAL_CLEAN) {
					m_fieldItem.Clean ();
				}
				if (m_csvStaffData.effect_param == (int)Define.Staff.EFFECT_PARAM.MEAL || m_csvStaffData.effect_param == (int)Define.Staff.EFFECT_PARAM.MEAL_CLEAN) {
					m_fieldItem.Meal ();
				}
			}
		}
		return;
	}

	override public void AnimationIdol(bool _bInit){
		if (_bInit) {
			string strName = "staff_icon" + m_dataStaff.staff_id.ToString ();
			m_sprIcon.atlas = AtlasManager.Instance.GetAtlas (strName);
			m_sprIcon.spriteName = strName;
		}
	}

	override public void AnimationMove(bool _bInit){
		if (_bInit) {
			m_fAnimationTime = 0.0f;
			m_fAnimationInterval = 0.2f;
			m_iAnimationFrame = 0;
		}

		m_fAnimationTime += Time.deltaTime;
		if (m_fAnimationInterval < m_fAnimationTime) {
			m_fAnimationTime -= m_fAnimationInterval;

			m_iAnimationFrame += 1;
			m_iAnimationFrame %= 2;		// トータル２フレーム

			string strName = "staff_icon" + m_dataStaff.staff_id.ToString ();

			if (m_iAnimationFrame == 1) {
				strName += "_2";
			}
			m_sprIcon.spriteName = strName;
		}
		return;
	}

	override public void AnimationEat(bool _bInit , int _iMealId ){
	}



}
