using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterSkit : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 skit_id;
		public int		 skit_type;
		public int		 page;
		public int		 page_next;
		public string	 memo;
		public string	 message;
		public string	 image;
		public int		 del_flg;

	}

}

