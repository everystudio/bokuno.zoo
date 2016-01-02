using UnityEngine;
using System.Collections;

public class UtilAssetBundleSprite : UtilAssetBundleBase {

	//public Sprite m_spLoadedSprite;
	public Texture2D m_spLoadedSprite;

	public void Load( string _strAssetName , string _strPath , int _iVersion ){

		EditPlayerSettingsData epsData = ConfigManager.instance.GetEditPlayerSettingsData ();
		string resultUrl = epsData.m_strS3Url + Define.ASSET_BUNDLES_ROOT +"/" + _strPath +"/"+  _strAssetName + ".unity3d";
		resultUrl = epsData.m_strS3Url + "/" + _strPath+"/" +  _strAssetName + ".unity3d";


		//Debug.Log (resultUrl);
		//Debug.Log (_strAssetName);
		Load (_strAssetName, resultUrl, _iVersion , m_goParent);
	}

	public void Load( string _strAssetName ){

		MasterLoadSprite.Data data = new MasterLoadSprite.Data ();

		foreach (MasterSpriteCSV temp in DataManager.master_sprite_list) {

		//foreach (MasterLoadSprite.Data temp in DataContainer.Instance.MasterLoadSpriteList) {
			//Debug.Log ("file:" + temp.filename + " path:" + temp.path + " ver:" + temp.version);
			if (temp.filename.Equals (_strAssetName) == true) {
				data.filename = temp.filename;
				data.path = temp.path;
				data.pre_load = temp.pre_load;
				data.version = temp.version;
				break;
			}
		}

		Load (data.filename, data.path, data.version);
		return;
	}

	public override void afterLoaded( AssetBundle _assetBundle , string _strAssetName ){

		if (_strAssetName == "") {
			m_goLoadObject = (GameObject)Instantiate (_assetBundle.mainAsset);
		} else {
			/*
			Debug.Log (_assetBundle);
			Debug.Log (_strAssetName);
			Debug.Log(_assetBundle.LoadAsset (_strAssetName));
			Debug.Log(_assetBundle.LoadAsset (_strAssetName, typeof(Texture2D)));
			*/

			//m_goLoadObject = Instantiate (_assetBundle.LoadAsset (_strAssetName, typeof(GameObject))) as GameObject;
			m_spLoadedSprite = Instantiate (_assetBundle.LoadAsset (_strAssetName, typeof(Texture2D))) as Texture2D;

			foreach (MasterSpriteCSV data in DataManager.master_sprite_list ) {
				if (data.filename.Equals (_strAssetName) == true) {

					//Debug.Log ("insert sprite:" + _strAssetName);
					if (m_goLoadObject) {
						m_goLoadObject.SetActive (false);
					}
					SpriteManager.Instance.Add (_strAssetName, m_spLoadedSprite);
				}
			}
		}
	}

}



