using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlCollectGold : MonoBehaviourEx {

	#region SerializeField
	[SerializeField]
	private UILabel m_lbCollectGold;
	[SerializeField]
	private ButtonBase m_btnCollect;
	#endregion

	public float m_fTimer;
	public float m_fCheckInterval;
	public int m_iCollectGold;

	public bool m_bInitialize;
	void Start(){
		m_bInitialize = false;
	}

	private void updateCollect(){
		int iCollectGold = 0;
		int iCollectExp = 0;
		List<DataItem> aaa = GameMain.dbItem.Select (" item_serial != 0 " );
		foreach (DataItem item in aaa) {
			int iTempGold = 0;
			int iTempExp = 0;

			item.GetCollect (false , out iTempGold , out iTempExp );
			iCollectGold += iTempGold;
			iCollectExp += iTempExp;
		}
		m_iCollectGold = iCollectGold;
		m_lbCollectGold.text = string.Format( "{0}G" , iCollectGold );

		// 支出の計算
		int iShisyutsu = 0;
		List<DataStaff> staff_list = GameMain.dbStaff.Select (" office_serial != 0 ");
		foreach (DataStaff staff in staff_list) {
			iShisyutsu += staff.GetPayGold (true);
		}
		if (0 < iShisyutsu) {
			DataManager.user.AddGold (-1 * iShisyutsu);
		}

		return;
	}

	// Use this for initialization
	public void Initialize () {
		m_bInitialize = true;
		m_fTimer = 0.0f;
		m_fCheckInterval = 5.0f;
		m_btnCollect.TriggerClear ();
		updateCollect ();
	}
	
	// Update is called once per frame
	void Update () {
		if (m_bInitialize == false) {
			return;
		}

		m_fTimer += Time.deltaTime;
		if (m_fCheckInterval < m_fTimer) {
			updateCollect ();
			m_fTimer -= m_fCheckInterval;
		}

		if (m_btnCollect.ButtonPushed) {

			//Debug.LogError ("here");
			m_btnCollect.TriggerClear ();

			m_iCollectGold = 0;

			int iCollectGold = 0;
			int iCollectExp = 0;
			List<DataItem> aaa = GameMain.dbItem.Select (" item_serial != 0 ");
			foreach (DataItem item in aaa) {
				int iTempGold = 0;
				int iTempExp = 0;

				item.GetCollect (true ,out iTempGold , out iTempExp );
				iCollectGold += iTempGold;
				iCollectExp += iTempExp;
			}

			m_iCollectGold = iCollectGold;
			if (0 < m_iCollectGold) {
				SoundManager.Instance.PlaySE ("se_cash");

				DataManager.user.AddCollect ();
				DataManager.user.AddGold (m_iCollectGold);
				DataManager.user.AddSyakkin (-1 * m_iCollectGold);
				DataManager.user.AddExp (iCollectExp);
				m_lbCollectGold.text = "0";

				// ここで仕事のチェックしますか
				List<DataWork> check_work_list = GameMain.dbWork.Select (" status = 1 ");
				foreach (DataWork work in check_work_list) {
					if (work.ClearCheck ()) {
						work.MissionClear ();
					}
				}
			}
		}
		return;
	
	}
}












