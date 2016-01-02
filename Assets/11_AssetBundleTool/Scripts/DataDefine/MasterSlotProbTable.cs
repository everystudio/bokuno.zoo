using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MasterSlotProbTable : ScriptableObject {

	public List<Data> DataList = new List<Data> ();

	[System.SerializableAttribute]
	public class Data
	{
		public int		 field_type;
		public int		 slot_prob_none;
		public int		 slot_prob_hazure;
		public int		 slot_prob_foot_print_left;
		public int		 slot_prob_foot_print_right;
		public int		 slot_prob_foot_print_center;
		public int		 slot_prob_foot_print_left_right;
		public int		 slot_prob_foot_print_left_center;
		public int		 slot_prob_foot_print_right_center;
		public int		 slot_prob_foot_print_all;
		public int		 slot_prob_reach;
		public int		 slot_prob_ball;
		public int		 slot_prob_battle;
		public int		 slot_prob_medal;
		public int		 slot_prob_normal_win;
		public int		 slot_prob_kakuhen_win;
		public int		 slot_prob_question;
		public int		 slot_prob_exclamation;
		public int		 slot_prob_roulette;
		public int		 slot_prob_jackpot;
		public int		 slot_prob_reserve0;
		public int		 slot_prob_reserve1;
		public int		 slot_prob_reserve2;
		public int		 slot_prob_reserve3;
		public int		 slot_prob_reserve4;
		public int		 slot_prob_reserve5;
		public int		 del_flg;

	}

}

