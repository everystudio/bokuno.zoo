using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterAlbumBonus : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 bonus_id;
		public int		 bonus_type;
		public string	 requirement_text;
		public int		 requirement_num;
		public string	 reward_text;
		public int		 reward_type;
		public int		 reward_num;
		public int		 del_flg;

	}

}

