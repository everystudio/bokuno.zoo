using UnityEngine;
using System.Collections;
using System.Collections.Generic;

//動的生成ページなので注意!!
public class DataContainer : MonoBehaviour {
	
	public List<MasterCard.Data>  MasterCardList;
	public List<MasterItem.Data>  MasterItemList;
	public List<MasterSkill.Data>  MasterSkillList;
	public List<MasterCollection.Data>  MasterCollectionList;
	public List<MasterSafeBox.Data>  MasterSafeBoxList;
	public List<MasterCardCompleteBonus.Data>  MasterCardCompleteBonusList;
	public List<MasterCollectionCompleteBonus.Data>  MasterCollectionCompleteBonusList;
	public List<MasterLeaderSkill.Data>  MasterLeaderSkillList;
	public List<MasterGacha.Data>  MasterGachaList;
	public List<MasterQuiz.Data>  MasterQuizList;
	public List<MasterLoginBonus.Data>  MasterLoginBonusList;
	public List<MasterLoginBonusReward.Data>  MasterLoginBonusRewardList;
	public List<MasterLoginBonusRewardDetail.Data>  MasterLoginBonusRewardDetailList;
	public List<MasterCommonShop.Data>  MasterCommonShopList;
	public List<MasterCommonShopDetail.Data>  MasterCommonShopDetailList;
	public List<MasterRuin.Data>  MasterRuinList;
	public List<MasterLoadAtlas.Data>  MasterLoadAtlasList;
	public List<MasterLoadPrefab.Data>  MasterLoadPrefabList;
	public List<MasterLoadSprite.Data>  MasterLoadSpriteList;
	public List<MasterLoadAudio.Data>  MasterLoadAudioList;
	public List<MasterRuinSugoroku.Data>  MasterRuinSugorokuList;
	public List<MasterRuinSugorokuGrid.Data>  MasterRuinSugorokuGridList;
	public List<MasterJackpotLotFirst.Data>  MasterJackpotLotFirstList;
	public List<MasterJackpotLotFirstAnimation.Data>  MasterJackpotLotFirstAnimationList;
	public List<MasterJackpotLotSecond.Data>  MasterJackpotLotSecondList;
	public List<MasterJackpotLotSecondAnimation.Data>  MasterJackpotLotSecondAnimationList;
	public List<MasterQuestSkill.Data>  MasterQuestSkillList;
	public List<MasterSkit.Data>  MasterSkitList;
	public List<MasterSlotProbTable.Data>  MasterSlotProbTableList;

	public static void DataSet (GameObject obj, LoadManager.LoadDataParam loadDataParam)
	{
		switch (loadDataParam.fileName) {
		
		case "MasterCardPrefab":
			DataContainer.Instance.MasterCardList = obj.GetComponent<MasterCardDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterItemPrefab":
			DataContainer.Instance.MasterItemList = obj.GetComponent<MasterItemDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterSkillPrefab":
			DataContainer.Instance.MasterSkillList = obj.GetComponent<MasterSkillDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterCollectionPrefab":
			DataContainer.Instance.MasterCollectionList = obj.GetComponent<MasterCollectionDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterSafeBoxPrefab":
			DataContainer.Instance.MasterSafeBoxList = obj.GetComponent<MasterSafeBoxDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterCardCompleteBonusPrefab":
			DataContainer.Instance.MasterCardCompleteBonusList = obj.GetComponent<MasterCardCompleteBonusDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterCollectionCompleteBonusPrefab":
			DataContainer.Instance.MasterCollectionCompleteBonusList = obj.GetComponent<MasterCollectionCompleteBonusDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterLeaderSkillPrefab":
			DataContainer.Instance.MasterLeaderSkillList = obj.GetComponent<MasterLeaderSkillDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterGachaPrefab":
			DataContainer.Instance.MasterGachaList = obj.GetComponent<MasterGachaDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterQuizPrefab":
			DataContainer.Instance.MasterQuizList = obj.GetComponent<MasterQuizDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterLoginBonusPrefab":
			DataContainer.Instance.MasterLoginBonusList = obj.GetComponent<MasterLoginBonusDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterLoginBonusRewardPrefab":
			DataContainer.Instance.MasterLoginBonusRewardList = obj.GetComponent<MasterLoginBonusRewardDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterLoginBonusRewardDetailPrefab":
			DataContainer.Instance.MasterLoginBonusRewardDetailList = obj.GetComponent<MasterLoginBonusRewardDetailDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterCommonShopPrefab":
			DataContainer.Instance.MasterCommonShopList = obj.GetComponent<MasterCommonShopDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterCommonShopDetailPrefab":
			DataContainer.Instance.MasterCommonShopDetailList = obj.GetComponent<MasterCommonShopDetailDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterRuinPrefab":
			DataContainer.Instance.MasterRuinList = obj.GetComponent<MasterRuinDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterLoadAtlasPrefab":
			DataContainer.Instance.MasterLoadAtlasList = obj.GetComponent<MasterLoadAtlasDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterLoadPrefabPrefab":
			DataContainer.Instance.MasterLoadPrefabList = obj.GetComponent<MasterLoadPrefabDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterLoadSpritePrefab":
			DataContainer.Instance.MasterLoadSpriteList = obj.GetComponent<MasterLoadSpriteDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterLoadAudioPrefab":
			DataContainer.Instance.MasterLoadAudioList = obj.GetComponent<MasterLoadAudioDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterRuinSugorokuPrefab":
			DataContainer.Instance.MasterRuinSugorokuList = obj.GetComponent<MasterRuinSugorokuDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterRuinSugorokuGridPrefab":
			DataContainer.Instance.MasterRuinSugorokuGridList = obj.GetComponent<MasterRuinSugorokuGridDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterJackpotLotFirstPrefab":
			DataContainer.Instance.MasterJackpotLotFirstList = obj.GetComponent<MasterJackpotLotFirstDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterJackpotLotFirstAnimationPrefab":
			DataContainer.Instance.MasterJackpotLotFirstAnimationList = obj.GetComponent<MasterJackpotLotFirstAnimationDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterJackpotLotSecondPrefab":
			DataContainer.Instance.MasterJackpotLotSecondList = obj.GetComponent<MasterJackpotLotSecondDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterJackpotLotSecondAnimationPrefab":
			DataContainer.Instance.MasterJackpotLotSecondAnimationList = obj.GetComponent<MasterJackpotLotSecondAnimationDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterQuestSkillPrefab":
			DataContainer.Instance.MasterQuestSkillList = obj.GetComponent<MasterQuestSkillDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterSkitPrefab":
			DataContainer.Instance.MasterSkitList = obj.GetComponent<MasterSkitDataHolder> ().assetBundleData.DataList;
			break;
		case "MasterSlotProbTablePrefab":
			DataContainer.Instance.MasterSlotProbTableList = obj.GetComponent<MasterSlotProbTableDataHolder> ().assetBundleData.DataList;
			break;
		default:
			Debug.LogError("未定義のデータが来ています" + loadDataParam.fileName);
			break;
		}
	}

    private static DataContainer instance = null;
    public static DataContainer Instance {
        get{
            if( instance == null ){
                instance = (DataContainer) FindObjectOfType(typeof(DataContainer));
                if (instance == null)
                {
                    GameObject obj = new GameObject();
                    obj.name = "DataContainer";
                    obj.AddComponent<DataContainer>();
                    instance = obj.GetComponent<DataContainer>();
                    instance.Init();
                }
            }
            return instance;
        }
    }

    protected void Init(){
        DataContainer[] obj = FindObjectsOfType(typeof(DataContainer)) as DataContainer[];
        if( obj.Length > 1 ){
            // 既に存在しているなら削除
            Destroy(gameObject);
        }else{
            // 音管理はシーン遷移では破棄させない
            DontDestroyOnLoad(gameObject);
        }
        return;
    }
}
