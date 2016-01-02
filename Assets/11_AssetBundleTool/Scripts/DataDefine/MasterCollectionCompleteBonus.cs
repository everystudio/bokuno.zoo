using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterCollectionCompleteBonus : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 collection_bonus_id;
		public int		 complete_type;
		public int		 complete_value;
		public int		 complete_count;
		public int		 reward_type;
		public int		 reward_type_id;
		public int		 reward_num;
		public string	 complete_description;
		public string	 reward_description;
		public int		 del_flg;

	}

}

