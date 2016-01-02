using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterItem : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 item_id;
		public string	 name;
		public int		 item_type;
		public int		 item_value;
		public int		 category;
		public string	 unit;
		public string	 description;
		public string	 thumb;
		public string	 release_date;
		public int		 del_flg;

	}

}

