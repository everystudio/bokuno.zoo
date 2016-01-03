using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;


public enum ICON_TYPE{
	NONE		= 0,
	MONSTER		,
	STAFF		,
	MAX			,
}

public class CtrlIconRoot : MonoBehaviour {

	public enum STEP {
		NONE			= 0,
		IDLE			,
		MOVE			,
		EAT				,

		MAX				,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	#region SerializeField
	[SerializeField]
	public UISprite m_sprIcon;
	[SerializeField]
	public CtrlIconFukidashi m_fukidashi;
	#endregion

	public ICON_TYPE m_eIconType;
	public int m_iSize;
	protected CtrlIconBase m_ctrlIconBase;
	protected DataMonster m_dataMonster;
	protected DataStaff m_dataStaff;

	public bool Clean(){
		return m_ctrlIconBase.CleanDust ();
	}
	public bool Meal(){
		return m_ctrlIconBase.Meal ();
	}

	public bool EqualMonsterSerial( int _iSerial ){
		Debug.Log (_iSerial);
		Debug.Log (m_dataMonster);
		if (m_dataMonster != null) {
			Debug.Log (m_dataMonster.monster_serial);
		}
		if (m_dataMonster != null && m_dataMonster.monster_serial == _iSerial) {
			return true;
		}
		return false;
	}
	public bool EqualStaffSerial( int _iSerial ){
		if (m_dataStaff != null && m_dataStaff.staff_serial == _iSerial) {
			return true;
		}
		return false;
	}

	public void initializeRoot(){
		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;
	}

	public void Initialize( DataMonster _monster , CtrlFieldItem _fieldItem ){
		m_eIconType = ICON_TYPE.MONSTER;
		CtrlIconMonster script = gameObject.AddComponent<CtrlIconMonster> ();

		script.m_fukidashi = m_fukidashi;
		m_iSize = _fieldItem.m_dataItem.width;
		script.Initialize (m_sprIcon , _monster , m_iSize );
		m_ctrlIconBase = (CtrlIconBase)script;
		m_dataMonster = _monster;

	}
	public void Initialize( DataStaff _staff , CtrlFieldItem _fieldItem  ){
		m_eIconType = ICON_TYPE.STAFF;
		CtrlIconStaff script = gameObject.AddComponent<CtrlIconStaff> ();
		//Debug.LogError (m_iSize);
		script.m_fukidashi = m_fukidashi;
		m_iSize = _fieldItem.m_dataItem.width;
		script.Initialize (m_sprIcon , _staff , m_iSize );
		m_ctrlIconBase = (CtrlIconBase)script;
		m_dataStaff = _staff;
	}

	public void SetDepth( int _iDepth ){

		if( m_ctrlIconBase != null ){
			m_ctrlIconBase.SetDepth (_iDepth);
		//m_sprIcon.depth = _iDepth + 1;
		}
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
		case STEP.NONE:
			break;

		case STEP.IDLE:
			if (bInit) {
				m_ctrlIconBase.AnimationIdol (bInit);
			}
			break;

		case STEP.MOVE:
			m_ctrlIconBase.AnimationMove (bInit);
			break;
		case STEP.EAT:
			m_ctrlIconBase.AnimationEat (bInit,0);
			break;
		case STEP.MAX:
		default:
			break;
		}
	
	}
}

























