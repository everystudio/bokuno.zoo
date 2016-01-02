using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;

public class LocalNotificationManager : MonoBehaviour {

	public List<CsvLocalNotificationData> m_localNotificationDataList = new List<CsvLocalNotificationData>();

	public List<string> add_message_list = new List<string>();

	public List<int> id_list = new List<int>();
	#if UNITY_ANDROID
	static AndroidJavaObject m_plugin2 = null;
	#endif
	protected static LocalNotificationManager instance = null;
	public static LocalNotificationManager Instance {
		get {
			if (instance == null) {
				GameObject obj = GameObject.Find ("LocalNotificationManager");
				if (obj == null) {
					obj = new GameObject("LocalNotificationManager");
					//Debug.LogError ("Not Exist AtlasManager!!");
				}
				instance = obj.GetComponent<LocalNotificationManager> ();
				if (instance == null) {
					//Debug.LogError ("Not Exist AtlasManager Script!!");
					instance = obj.AddComponent<LocalNotificationManager>() as LocalNotificationManager;
				}
				instance.Initialize ();
			}
			return instance;
		}
	}



	private void Initialize(){
		DontDestroyOnLoad(gameObject);
		localnotificate_list.Clear ();
		add_message_list.Clear ();
		m_localNotificationDataList.Clear ();
		id_list.Clear ();
		#if UNITY_ANDROID && !UNITY_EDITOR
		// プラグイン名をパッケージ名+クラス名で指定する。
		m_plugin2 = new AndroidJavaObject( "com.everystudio.test001.TestLocalnotification" );
		#endif

	}

	public bool Add( CsvLocalNotificationData _data ){

		bool bHit = false;
		foreach (CsvLocalNotificationData data in m_localNotificationDataList) {
			if (data.id == _data.id) {
				bHit = true;
			}
		}
		if (bHit == false) {
			m_localNotificationDataList.Add (_data);
			add_message_list.Add (_data.message);

			//Debug.Log (string.Format ("second:{0} message{1}", _data.second, _data.message));
		}
		return bHit;
	}

	private List<int> localnotificate_list = new List<int> ();
	void OnApplicationPause(bool pauseStatus) {
		// ローカル通知用
		if (pauseStatus) {
			//TODO
			#if UNITY_IPHONE

			foreach( CsvLocalNotificationData data in m_localNotificationDataList ){
				ISN_LocalNotification local_notification = new ISN_LocalNotification (
					DateTime.Now.AddSeconds (data.second),
					data.message,
					false);

				id_list.Add( local_notification.Id );
				IOSNotificationController.Instance.ScheduleNotification (local_notification);
			}

			#elif UNITY_ANDROID

			int iTemp = 0;
			/*
			iTemp = EtceteraAndroid.scheduleNotification( Define.iLocalNotifyMessageTime1, "どうぶつみっけ!", Define.strLocalNotifyMessage1, Define.strLocalNotifyMessage1, Define.strLocalNotifyMessage1 );
			localnotificate_list.Add( iTemp );
			iTemp = EtceteraAndroid.scheduleNotification( Define.iLocalNotifyMessageTime2, "どうぶつみっけ!", Define.strLocalNotifyMessage2, Define.strLocalNotifyMessage2, Define.strLocalNotifyMessage2 );
			localnotificate_list.Add( iTemp );
			*/

			foreach( CsvLocalNotificationData data in m_localNotificationDataList ){
				//Debug.LogError( string.Format( "add local notification:{0}" , data.message ));

				m_plugin2.Call ("sendNotification", (long)data.second , iTemp , data.title, data.message);
				iTemp += 1;
				//iTemp = EtceteraAndroid.scheduleNotification( data.second, data.title, data.message, data.message, data.message );
			}
			//iTemp = EtceteraAndroid.scheduleNotification( 20, "どうぶつみっけ!", Define.strLocalNotifyMessage2, Define.strLocalNotifyMessage2, Define.strLocalNotifyMessage2 );
			#endif
		} else {
			#if UNITY_IPHONE
			// こっちの削除はなくてもいいらしい
			foreach( int set_id in id_list ){
				IOSNotificationController.Instance.CancelLocalNotificationById( set_id );
			}
			//IOSNotificationController.Instance.CancelAllLocalNotifications ();
			#elif UNITY_ANDROID
			int iTemp = 0;
			foreach( CsvLocalNotificationData data in m_localNotificationDataList ){
				m_plugin2.Call ("clearNotification", iTemp );
				iTemp += 1;
			}
			#endif
		}




	}
}
