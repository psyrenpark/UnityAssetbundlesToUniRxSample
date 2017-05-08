using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//동시 호출 문제

public class AssetBundleUniRxSample7 : MonoBehaviour {

    private const string  assetBundle_URL     = "https://test.com/AssetBundles/";

    // Use this for initialization
    IEnumerator Start () {

        var rm = ResourcesManagerSample2.GetInst;

        //yield return rm.Initialize ( assetBundle_URL ) ;
        yield return StartCoroutine ( rm.Initialize ( assetBundle_URL ) );
        
    
        rm.SelectGUIObject ( "base_canvas", "BaseCanvas" ,
            ( x) => {
                Debug.Log ("name : " + x.GuiName);
            } );
        
        //rm.CanvasEnabled ( "base_canvas" , "BaseCanvas" , false );
   
        //rm.ResistPanelToPresenter ( "base_canvas" , "BaseCanvas" , "Panel" , "AssetBundleUniRxSample3",true );



    }

}
