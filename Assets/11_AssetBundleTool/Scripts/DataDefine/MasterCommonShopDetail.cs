using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterCommonShopDetail : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 common_shop_id;
		public int		 common_shop_detail_id;
		public int		 item_type;
		public int		 item_type_id;
		public int		 item_num;
		public int		 del_flg;

	}

}

