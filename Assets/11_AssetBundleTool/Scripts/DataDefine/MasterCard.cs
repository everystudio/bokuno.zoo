using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterCard : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 card_id;
		public string	 name;
		public int		 rarity;
		public int		 attr;
		public int		 size;
		public int		 card_type;
		public int		 card_category;
		public int		 mlv;
		public int		 hp;
		public int		 atk;
		public int		 def;
		public int		 skill_id;
		public int		 qskill_id;
		public int		 lskill_id;
		public int		 evolution_card_id;
		public int		 evolution_material_1_l;
		public int		 evolution_material_1_m;
		public int		 evolution_material_1_s;
		public int		 evolution_material_2_l;
		public int		 evolution_material_2_m;
		public int		 evolution_material_2_s;
		public int		 evolution_material_3_l;
		public int		 evolution_material_3_m;
		public int		 evolution_material_3_s;
		public int		 sell_price;
		public int		 gacha_prob1;
		public int		 gacha_prob2;
		public int		 gacha_prob3;
		public int		 gacha_prob4;
		public int		 gacha_prob5;
		public int		 gacha_prob6;
		public int		 gacha_prob7;
		public int		 gacha_prob8;
		public int		 gacha_prob9;
		public int		 del_flg;

	}

}

