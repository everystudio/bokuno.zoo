using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterSafeBox : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 safe_box_id;
		public int		 safe_box_type;
		public string	 name;
		public int		 price;
		public int		 medal_limit;
		public int		 period;
		public string	 description;
		public int		 del_flg;

	}

}

