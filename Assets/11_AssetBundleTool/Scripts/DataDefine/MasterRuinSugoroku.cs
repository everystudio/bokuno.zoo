using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterRuinSugoroku : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 ruin_id;
		public int		 sugoroku_id;
		public string	 sugoroku_data;
		public int		 del_flg;

	}

}

