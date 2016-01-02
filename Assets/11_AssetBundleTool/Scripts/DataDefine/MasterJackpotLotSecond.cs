using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterJackpotLotSecond : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 pocket_id;
		public int		 prob;
		public int		 get_medal;
		public int		 is_jackpot;
		public int		 del_flg;

	}

}

