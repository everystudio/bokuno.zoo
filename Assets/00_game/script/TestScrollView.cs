using UnityEngine;
using System.Collections;

public class TestScrollView : MonoBehaviour {

	[SerializeField]
	private UICenterOnChild m_csCenterOnChild;

	GameObject m_goCenter;

	// Use this for initialization
	void Start () {
		m_csCenterOnChild.onCenter = DragBanner;
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// バナーがドラッグされて切り替わった際に呼ばれるイベント
	public void DragBanner(GameObject _goBanner) {
		//Debug.Log (_goBanner.name);
		int iBannerNo = 0;
		SetBanner(_goBanner);
		return;
	}

	// デッキ（バナー）を切り替える処理
	public void SetBanner(int _iNo) {
		int next = 0;
		int pre = 0;

		// バナーの移動

	}
	public void SetBanner( GameObject _goBanner ){
		//Debug.Log (_goBanner.name);
		Debug.Log (_goBanner.name);
		if (m_goCenter != _goBanner) {
			m_goCenter = _goBanner;
			m_csCenterOnChild.CenterOn (_goBanner.transform);
		} else {
		}
	}




}


