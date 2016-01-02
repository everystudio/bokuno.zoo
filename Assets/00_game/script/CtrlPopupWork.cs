using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlPopupWork : MonoBehaviourEx{

	[SerializeField]
	private UILabel m_lbTitle;
	[SerializeField]
	private UILabel m_lbTitle2;
	[SerializeField]
	private UILabel m_lbMessage;
	[SerializeField]
	private UILabel m_lbPrize;
	[SerializeField]
	private UILabel m_lbExp;

	public enum STEP {
		NONE		= 0,
		INITIALIZE	,
		IDLE		,

		OPEN		,
		WAIT		,
		CLOSE		,

		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;
	public bool m_bInitialize;
	public float m_fTimer;
	public float m_fDispWait;

	public UISprite m_sprWhite;
	public UtilSwitchSprite m_switchSprite;
	public UISprite m_sprWorkIcon;

	public GameObject m_goHit;
	public ButtonBase m_btnClose;

	public int m_iLoopIndex;

	public int m_iPopupWorkId;


	private  Queue<int> m_WorkIdQueue = new Queue<int>(){};

	public void AddCleardWorkId( int _iWorkId ){
		m_WorkIdQueue.Enqueue ( _iWorkId );
	}

	void Start(){
		Initialize ();
	}

	public void Initialize(){

		if (m_bInitialize == false) {
			m_bInitialize = true;

			m_eStep = STEP.INITIALIZE;
			m_eStepPre = STEP.MAX;

			m_goHit.SetActive (false);
			m_sprWhite.gameObject.SetActive (false);
			m_btnClose.gameObject.SetActive (false);

			m_switchSprite.gameObject.transform.localScale = Vector3.zero;
			m_lbMessage.text = "-------------";

			m_WorkIdQueue.Clear ();

			m_fDispWait = 0.0f;
		}

	}


	// Update is called once per frame
	void Update () {

		bool bInit = false;
		if (m_eStepPre != m_eStep) {
			m_eStepPre  = m_eStep;
			bInit = true;
		}

		switch (m_eStep) {

		case STEP.INITIALIZE:
			if (bInit) {
				m_switchSprite.SetSprite ("work_clear");
			}

			//Debug.LogError (SpriteManager.Instance.IsIdle ());
			if (SpriteManager.Instance.IsIdle ()) {
				m_eStep = STEP.IDLE;
			}
			break;

		case STEP.IDLE:
			if (bInit) {
				m_goHit.SetActive (false);
				m_sprWhite.gameObject.SetActive (false);
				m_btnClose.gameObject.SetActive (false);

			}

			if (0 < m_WorkIdQueue.Count && TutorialManager.Instance.IsTutorial() == false ) {
				m_iPopupWorkId = m_WorkIdQueue.Dequeue ();
				m_eStep = STEP.OPEN;
			}
			break;

		case STEP.OPEN:
			if (bInit) {

				GameMain.Instance.TutorialInputLock = true;
				SoundManager.Instance.PlaySE ("se_work_clear");

				m_goHit.SetActive (true);
				m_sprWhite.gameObject.SetActive (true);
				m_btnClose.gameObject.SetActive (true);

				m_sprWorkIcon.spriteName = BannerWork.GetSpriteName (m_iPopupWorkId);
				m_sprWorkIcon.atlas = AtlasManager.Instance.GetAtlas (m_sprWorkIcon.spriteName);
				CsvWorkData work_data = DataManager.GetWork (m_iPopupWorkId);

				m_lbTitle.text = work_data.title;
				m_lbTitle2.text = work_data.title;
				m_lbMessage.text = work_data.description;
				m_lbExp.text = work_data.exp.ToString ();

				string strPrize = "";
				if (0 < work_data.prize_coin) {
					strPrize = string.Format ("{0}G", work_data.prize_coin);
				} else if (0 < work_data.prize_monster) {
					CsvMonsterData monster_data = DataManager.GetMonster (work_data.prize_monster);
					strPrize = string.Format ("{0}", monster_data.name);
				} else if (0 < work_data.prize_ticket) {
					strPrize = string.Format ("チケット{0}枚", work_data.prize_ticket);
				} else {
					Debug.LogError ("unknown prize");
				}
				m_lbPrize.text = strPrize;


				TweenScale ts = TweenScale.Begin (m_switchSprite.gameObject, 0.2f, Vector3.one);
				EventDelegate.Set (ts.onFinished, EndTween);
			}
			if (m_bIsEndTween) {
				m_eStep = STEP.WAIT;
			}
			break;
		case STEP.WAIT:
			if (bInit) {
				m_btnClose.TriggerClear ();
			}
			if (m_btnClose.ButtonPushed) {
				m_eStep = STEP.CLOSE;
			}
			break;

		case STEP.CLOSE:
			if (bInit) {
				m_bIsEndTween = false;
				TweenScale ts = TweenScale.Begin (m_switchSprite.gameObject, 0.2f, Vector3.zero);
				EventDelegate.Set (ts.onFinished, EndTween);

				GameMain.Instance.TutorialInputLock = false;

			}
			if (m_bIsEndTween) {
				m_eStep = STEP.IDLE;
				if (0 < m_WorkIdQueue.Count) {
					m_iPopupWorkId = m_WorkIdQueue.Dequeue ();
					m_eStep = STEP.OPEN;
				}
			}
			break;

		case STEP.MAX:
		default:
			break;
		}
		return;
	}
}
