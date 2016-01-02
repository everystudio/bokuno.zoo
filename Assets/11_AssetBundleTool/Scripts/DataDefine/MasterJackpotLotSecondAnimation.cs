using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterJackpotLotSecondAnimation : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 pocket_id;
		public int		 animation_id;
		public string	 animation;
		public int		 del_flg;

	}

}

