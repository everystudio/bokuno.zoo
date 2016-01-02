using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterJackpotLotFirst : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 pocket_id;
		public int		 prob;
		public int		 get_medal;
		public int		 sugoroku_grid;
		public int		 is_second;
		public int		 del_flg;

	}

}

