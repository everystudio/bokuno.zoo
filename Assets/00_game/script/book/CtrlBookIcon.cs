using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CtrlBookIcon : ButtonBase {

	#region SerializeFieldとか
	[SerializeField]
	private UILabel m_lbNumber;
	[SerializeField]
	private UILabel m_lbName;
	[SerializeField]
	private UISprite m_sprMonster;
	#endregion

	public CsvMonsterData m_monsterData;

	private CtrlBookMonster m_ctrlBookMonster;
	private GameObject m_posDisplay;

	public bool Initialize( CsvMonsterData _data , GameObject _posDisplay ){
		bool bRet = true;
		m_monsterData = _data;

		m_posDisplay = _posDisplay;

		m_lbNumber.text = "NO." + m_monsterData.monster_id.ToString ();
		m_lbName.text = m_monsterData.name;

		string strMonster = "chara" + m_monsterData.monster_id.ToString ();
		m_sprMonster.atlas = AtlasManager.Instance.GetAtlas (strMonster);
		m_sprMonster.spriteName = strMonster;

		List<DataMonster> datamonster_list = GameMain.dbMonster.Select (string.Format (" monster_id = {0}", m_monsterData.monster_id));

		if (datamonster_list.Count == 0) {
			bRet = false;
			m_sprMonster.color = Color.black;
			m_lbName.text = "？？？？？？";
		}
		return bRet;
	}
	public bool Initialize( int _iMonsterId , GameObject _posDisplay ){
		return Initialize (MasterData.Monster (_iMonsterId), _posDisplay);
	}

	public int GetMonsterId(){
		return m_monsterData.monster_id;
	}


	// Update is called once per frame
	void Update () {

		/*
		if (ButtonPushed && m_ctrlBookMonster == null ) {

			TriggerClear ();

			GameObject obj = PrefabManager.Instance.MakeObject ("prefab/PrefBookMonster", m_posDisplay);

			m_ctrlBookMonster = obj.GetComponent<CtrlBookMonster> ();
			m_ctrlBookMonster.Initialize (m_monsterData.monster_id);
		}
		*/


	
	}
}






















