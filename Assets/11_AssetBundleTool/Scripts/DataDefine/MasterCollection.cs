using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterCollection : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 collection_id;
		public string	 name;
		public int		 rarity;
		public int		 attr;
		public int		 category;
		public string	 description;
		public int		 sell_price;
		public string	 release_date;
		public int		 del_flg;

	}

}

