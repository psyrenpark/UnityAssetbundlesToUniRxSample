using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AssetBundleUniRxSample7 : MonoBehaviour {

    private const string  assetBundle_URL     = "https://test.com/AssetBundles/";

    // Use this for initialization
    IEnumerator Start () {

        var rm = ResourcesManagerSample.GetInst;

        //yield return rm.Initialize ( assetBundle_URL ) ;
        yield return StartCoroutine ( rm.Initialize ( assetBundle_URL ) );




    }

}
