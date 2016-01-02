using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterLoginBonusRewardDetail : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 login_bonus_id;
		public int		 day_num;
		public int		 login_bonus_reward_detail_id;
		public int		 reward_type;
		public int		 reward_type_id;
		public int		 reward_num;
		public string	 reward_message;
		public int		 is_present;
		public int		 del_flg;

	}

}

