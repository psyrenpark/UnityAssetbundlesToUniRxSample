using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleUniRxSample8 : MonoBehaviour {

    private const string  assetBundle_URL     = "https://test.com/AssetBundles/";

    // Use this for initialization
    IEnumerator Start () {

        var rm = ResourcesManagerSample2.GetInst;

        yield return StartCoroutine ( rm.Initialize ( assetBundle_URL ) );

        rm.ResistPanelToPresenter ( "base_canvas" , "BaseCanvas" , "Panel" , "SamplePresnter" , true );

    }


}
