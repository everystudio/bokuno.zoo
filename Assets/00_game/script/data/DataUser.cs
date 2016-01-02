using UnityEngine;
using System.Collections;

[System.Serializable]
public class DataUser {

	public int m_iSyakkin;
	public string m_strName;
	public int m_iLevel;
	public int m_iTotalExp;
	public int m_iGold;
	public int m_iWidth;
	public int m_iHeight;

	public int m_iTicket;
	public int m_iCollectCount;

	public int m_iGoldPerHour;
	public int m_iShisyutsuPerHour;

	public bool m_bInitialized;

	public void ParamUpdate(){
		m_iSyakkin = (GameMain.dbKvs.ReadInt (Define.USER_SYAKKIN));
		m_iGold = (GameMain.dbKvs.ReadInt (Define.USER_SYOJIKIN));
		m_iTicket = (GameMain.dbKvs.ReadInt (Define.USER_TICKET));
		m_iLevel = (GameMain.dbKvs.ReadInt (Define.USER_LEVEL));
		m_iTotalExp = (GameMain.dbKvs.ReadInt (Define.USER_TOTAL_EXP));

		m_iCollectCount = GameMain.dbKvs.ReadInt (Define.USER_COLLECT_COUNT);
		return;
	}


	public void Initialize( int _iWidth , int _iHeight ){

		if (m_bInitialized == false) {
			m_bInitialized = true;
			m_iWidth = _iWidth;
			m_iHeight = _iHeight;

			/*
			 * ここで初期化しなくても大丈夫だった
			*/
		}
		return;
	}

	public void AddSyakkin( int _iGold ){
		m_iSyakkin += _iGold;
		if (m_iSyakkin <= 0) {
			m_iSyakkin = 0;

			if (PlayerPrefs.HasKey ("kansai") == false) {
				PlayerPrefs.SetInt ("kansai", 1);
				CtrlDebit.m_bOpen = true;
			}

		}
		GameMain.dbKvs.WriteInt (Define.USER_SYAKKIN, m_iSyakkin);
		GameMain.Instance.HeaderRefresh ();
	}

	public void AddGold( int _iGold ){
		//Debug.LogError (string.Format ("gold now:{0} add_num:{1}", m_iGold, _iGold));
		m_iGold += _iGold;

		if (m_iGold < 0) {
			m_iGold = 0;
		}
		GameMain.dbKvs.WriteInt (Define.USER_SYOJIKIN, m_iGold);
		GameMain.Instance.HeaderRefresh ();
	}
	public void AddTicket( int _iTicket ){
		m_iTicket += _iTicket;
		GameMain.dbKvs.WriteInt (Define.USER_TICKET, m_iTicket);
		GameMain.Instance.HeaderRefresh ();
	}

	public void AddExp( int _iExp ){
		m_iTotalExp += _iExp;
		GameMain.dbKvs.WriteInt(Define.USER_TOTAL_EXP , m_iTotalExp);
	}
	public void AddCollect( int _iCount = 1 ){
		m_iCollectCount += _iCount;
		//Debug.LogError (m_iCollectCount); 
		GameMain.dbKvs.WriteInt(Define.USER_COLLECT_COUNT , m_iCollectCount);
		//Debug.LogError (GameMain.dbKvs.ReadInt (Define.USER_COLLECT_COUNT));
	}


	public bool AbleBuy( int _iGold , int _iTicket , int _iCost , int _iCostNokori , int _iHave , int _iLimit , ref BannerBase.ABLE_BUY_REASON _eReason ){
		bool bRet = true;
		_eReason = BannerBase.ABLE_BUY_REASON.OKEY;

		if (m_iGold < _iGold) {
			bRet = false;
			_eReason = BannerBase.ABLE_BUY_REASON.GOLD;
		}
		if (m_iTicket < _iTicket) {
			bRet = false;
			_eReason = BannerBase.ABLE_BUY_REASON.TICKET;
		}
		if (_iCostNokori < _iCost ) {
			bRet = false;
			_eReason = BannerBase.ABLE_BUY_REASON.COST;
		}

		if (0 < _iLimit) {
			Debug.Log( string.Format( "setting_limit={0}" , _iLimit ));
			if (_iLimit <= _iHave) {
				_eReason = BannerBase.ABLE_BUY_REASON.LIMIT;
			}
		}
		return bRet;


	}

}
