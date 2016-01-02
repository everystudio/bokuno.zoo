using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterLoginBonusReward : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 login_bonus_id;
		public int		 day_num;
		public string	 description;
		public int		 del_flg;

	}

}

