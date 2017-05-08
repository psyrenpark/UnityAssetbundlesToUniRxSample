using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleUniRxSample9 : MonoBehaviour {

    private const string  assetBundle_URL     = "https://test.com/AssetBundles/";

    // Use this for initialization
    IEnumerator Start () {

        var rm = ResourcesManagerSample2.GetInst;

        yield return StartCoroutine ( rm.Initialize ( assetBundle_URL ) );

        rm.ResistPanelToPresenter ( "" , "MoneyCanvas" , "ButtonPanel" , "SamplePresnter2_1" , true );


        yield return new WaitForSeconds ( 1f );

        rm.ResistPanelToPresenter ( "" , "MoneyCanvas" , "ImgPanel" , "SamplePresnter2_2" , true );

    }
}