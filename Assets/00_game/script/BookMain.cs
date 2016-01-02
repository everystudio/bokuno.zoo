using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/*
 *・ボタンでの移動
 *・未取得の暗転、文字の？化
 *・詳細表示
*/


public class BookMain : PageBase {

	public enum STEP
	{
		NONE		= 0,
		IDLE		,
		DISP_DETAIL	,
		MAX			,
	}
	public STEP m_eStep;
	public STEP m_eStepPre;

	public readonly int PAGE_DISP = 4;
	public readonly string STR_PAGE_ROOT = "pageRoot";
	public GameObject m_goPageGrid;
	private GameObject m_goCenter;
	public GameObject m_goDetailPanel;

	private List<GameObject> m_goBannerList = new List<GameObject> ();

	public List<CtrlBookIcon> m_bookIconList = new List<CtrlBookIcon> ();

	public UtilSwitchSprite m_background;

	#region SerializeFieldでの設定が必要なメンバー変数
	[SerializeField]
	private UICenterOnChild m_csCenterOnChild;
	#endregion

	protected override void initialize(){

		m_eStep = STEP.IDLE;
		m_eStepPre = STEP.MAX;

		m_bookIconList.Clear ();
		m_csCenterOnChild.onCenter = DragBanner;

		m_pageHeader = makeHeader ("header_book" , "Book1");
		makeCloseButton ();

		m_background.SetSprite ("bg002");


		List<CsvMonsterData> data_list = MasterData.MonsterList;

		int iTotalNum = data_list.Count;
		int iDiscoverNum = 0;

		int iPageNum = 1 + (iTotalNum / PAGE_DISP);

		for (int i = 0; i < iPageNum; i++) {
			GameObject objPageRoot = PrefabManager.Instance.MakeObject ("prefab/PrefBookPageRoot", m_goPageGrid);

			objPageRoot.name = STR_PAGE_ROOT + i.ToString ();
			int monster_id_start = i * PAGE_DISP;
			int monster_id_end = (i + 1) * PAGE_DISP;
			if (iTotalNum < monster_id_end) {
				monster_id_end = iTotalNum;
			}
			for (int monster_id = monster_id_start; monster_id < monster_id_end; monster_id++) {
				GameObject objChild = PrefabManager.Instance.MakeObject ("prefab/PrefBookIcon", objPageRoot);
				CtrlBookIcon bookIcon = objChild.GetComponent<CtrlBookIcon> ();

				// ここ、補正してます
				if (bookIcon.Initialize (monster_id + 1 , m_goDetailPanel )) {
					iDiscoverNum += 1;

					// 全部じゃなくて詳細表示を行うやつだけ
					m_bookIconList.Add (bookIcon);
				}
			}
			m_goBannerList.Add (objPageRoot);
			m_goDeleteList.Add (objPageRoot);
		}

		float fRate = (float)iDiscoverNum / (float)iTotalNum;
		m_pageHeader.SetCompRate ( (int)((fRate)*100.0f));

		m_goPageGrid.GetComponent<UIGrid> ().enabled = true;
	}
	protected override void close(){
		m_goBannerList.Clear ();
	}

	public CtrlBookIcon m_detailBookIcon;
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
				m_detailBookIcon = null;
				foreach (CtrlBookIcon icon in m_bookIconList) {
					icon.TriggerClear ();
				}
			}
			foreach (CtrlBookIcon icon in m_bookIconList) {
				if (icon.ButtonPushed) {
					m_detailBookIcon = icon;
					m_eStep = STEP.DISP_DETAIL;
				}
			}
			break;

		case STEP.DISP_DETAIL:
			if (bInit) {
				GameObject obj = PrefabManager.Instance.MakeObject ("prefab/PrefBookMonster", m_goDetailPanel);

				m_ctrlBookMonster = obj.GetComponent<CtrlBookMonster> ();
				m_ctrlBookMonster.Initialize (m_detailBookIcon.GetMonsterId ());
			}

			if (m_ctrlBookMonster.IsEnd ()) {
				Destroy (m_ctrlBookMonster.gameObject);
				m_eStep = STEP.IDLE;
			}
			break;
		case STEP.MAX:
		default:
			break;
		}

	
	}

	private CtrlBookMonster m_ctrlBookMonster;

	#region scroll関連
	// バナーがドラッグされて切り替わった際に呼ばれるイベント
	public void DragBanner(GameObject _goBanner) {
		//Debug.Log (_goBanner.name);
		int iBannerNo = 0;
		SetBanner(_goBanner);
		return;
	}
	public void SetBanner( int _iBannerId ){
		foreach (GameObject obj in m_goBannerList) {
			int banner_id = int.Parse (obj.name.Replace (STR_PAGE_ROOT, ""));
			if (banner_id == _iBannerId) {
				SetBanner (obj);
				break;
			}
		}
		return;
	}
	public void SetBanner( GameObject _goBanner ){
		//Debug.Log (_goBanner.name);
		if (m_goCenter != _goBanner) {
			m_goCenter = _goBanner;
			m_csCenterOnChild.CenterOn (_goBanner.transform);
		} else {
		}
	}
	#endregion




}
