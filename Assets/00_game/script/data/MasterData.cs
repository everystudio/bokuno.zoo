using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterData : MonoBehaviour {

	static MasterData instance = null;

	public static MasterData Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("MasterData");
				if (obj == null) {
					obj = new GameObject("MasterData");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<MasterData> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<MasterData>() as MasterData;
				}
				instance.initialize ();
			}
			return instance;
		}
	}

	private void initialize(){
		DontDestroyOnLoad(gameObject);
		return;
	}

	private List<CsvMonsterData> m_monsterDataList = new List<CsvMonsterData>();
	static public List<CsvMonsterData> MonsterList {
		get{
			if (0 == Instance.m_monsterDataList.Count) {
				var csvMonster = new CsvMonster ();
				csvMonster.Load ();
				foreach (CsvMonsterData csv_data in csvMonster.All) {
					Instance.m_monsterDataList.Add (csv_data);
				}
			}
			return Instance.m_monsterDataList;
		}
	}
	static public CsvMonsterData Monster( int _iMonsterId ){
		foreach (CsvMonsterData monster in MonsterList) {
			if (monster.monster_id == _iMonsterId) {
				return monster;
			}
		}
		return new CsvMonsterData ();
	}


}












