using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterCommonShop : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 common_shop_id;
		public int		 category;
		public int		 price;
		public string	 name;
		public string	 description;
		public string	 sell_start_date;
		public string	 sell_end_date;
		public int		 del_flg;

	}

}

