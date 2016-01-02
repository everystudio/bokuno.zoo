using UnityEngine;
using System.Collections;

abstract public class CtrlIconBase : MonoBehaviourEx {

	public enum STEP
	{
		NONE		= 0,
		IDLE		,
		MOVE		,
		EAT			,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public float m_fTimer;
	public float m_fTimerIdleWait;

	public int m_iSize;
	public CtrlIconRoot m_iconRoot;
	public CtrlIconFukidashi m_fukidashi;		// 外部からの代入で利用

	public float m_fAnimationTime;
	public float m_fAnimationInterval = 0.032f;
	public int m_iAnimationFrame;

	public UISprite m_sprIcon;
	public DataMonster m_dataMonster;
	public DataStaff m_dataStaff;

	public GameObject m_goMeal;

	public void SetSize( int _iSize ){
		m_iSize = _iSize;
	}
	virtual public void SetDepth( int _iDepth ){
		m_sprIcon.depth = _iDepth + 2;
	}

	public Vector3 GetMovePos(){


		float fX = UtilRand.GetRange (1.0f);
		float fY = UtilRand.GetRange (1.0f);

		Vector3 v3X = Define.CELL_X_DIR * fX * m_iSize;
		Vector3 v3Y = Define.CELL_Y_DIR * fY * m_iSize;

		return v3X + v3Y;

	}

	abstract public void AnimationIdol(bool _bInit);
	abstract public void AnimationMove(bool _bInit);
	abstract public void AnimationEat(bool _bInit , int _iMealId );
	virtual public void createMeal(){
		Release (m_goMeal);
		m_goMeal = PrefabManager.Instance.MakeObject ("prefab/PrefMeal", gameObject);
		m_goMeal.transform.localScale = new Vector3 (0.5f, 0.5f, 0.5f);
	}

	abstract public void update_idle (bool _bInit);

	public void Update(){

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {
		case STEP.IDLE:
			if (bInit) {
				AnimationIdol (true);
				m_fTimer = 0.0f;

				m_fTimerIdleWait = UtilRand.GetRange (4.0f, 2.0f);

			}
			m_fTimer += Time.deltaTime;
			if (m_fTimerIdleWait < m_fTimer) {
				m_eStep = STEP.MOVE;
			}
			update_idle (bInit);
			break;

		case STEP.MOVE:
			if (bInit) {
				m_fTimer = 0.0f;

				m_bIsEndTween = false;
				Vector3 target = GetMovePos ();
				TweenPosition tp = TweenPosition.Begin (gameObject, 1.0f, target );
				EventDelegate.Set (tp.onFinished, EndTween);

				if ( myTransform.localPosition.x < target.x ) {
					m_sprIcon.transform.localScale = new Vector3( -1.0f , 1.0f , 1.0f );
				}
				else {
					m_sprIcon.transform.localScale = Vector3.one;
				}
			}
			AnimationMove (bInit);
			if (m_bIsEndTween) {
				m_eStep = STEP.IDLE;
			}
			m_fTimer += Time.deltaTime;
			if (3.0f < m_fTimer) {
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.EAT:
			if (bInit) {
				m_fTimer = 0.0f;
				createMeal ();
			}

			AnimationEat (bInit,0);
			m_fTimer += Time.deltaTime;
			if (3.0f < m_fTimer) {
				m_eStep = STEP.IDLE;

				Release (m_goMeal);
			}
			break;

		case STEP.MAX:
		default:
			break;
		}

	}

	virtual public bool CleanDust(){
		return false;
	}

	virtual public bool Meal(){
		return false;
	}

}
