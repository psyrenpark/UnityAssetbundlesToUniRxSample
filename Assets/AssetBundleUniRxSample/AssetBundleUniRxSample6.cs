using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Linq;
using UniRx;
using System.Linq;

//TODO : [ Criticality : Middle ]
//시뮬레이터 모드에서  3번째 sprite LoadAssetAsync 안됨;
//이 방법은 차후 고려;

public class AssetBundleUniRxSample6 : MonoBehaviour {

    private const string  assetBundle_URL     = "https://test.com/AssetBundles/";

    // Use this for initialization
    IEnumerator Start () {

        var rm = ResourcesManagerSample.GetInst;

        //yield return rm.Initialize ( assetBundle_URL ) ;
        yield return StartCoroutine ( rm.Initialize ( assetBundle_URL ) );

        yield return rm.LoadAssetAsync<GameObject> ( "base_canvas" , "BaseCanvas" ,
            ( obj ) => {
                Debug.Log ( "exit : " + obj.name );
                GameObject go = Instantiate ( obj as GameObject );
                go.name = "BaseCanvas";
            } );

        yield return rm.LoadAssetAsync<GameObject> ( "test_box" , "TestBox2" ,
            ( obj ) => {
                Debug.Log ( "exit : " + obj.name );
                GameObject go = Instantiate ( obj as GameObject );
                go.name = "TestBox";
            } );

        yield return new WaitForSeconds ( 1f );

        //TODO : [ Criticality : Middle ]
        yield return rm.LoadAssetAsync<Sprite> ( "sprites/weapons" , "weapon0" ,
            ( obj ) => {
                Debug.Log ( "exit : " + obj.name );

                foreach ( var item in GameObject.Find ( "BaseCanvas" )
                            .Descendants ()
                            .Where ( x => x.name.StartsWith ( "Image" ) ) ) {

                    Debug.Log ( item.name );
                    item.GetComponent<UnityEngine.UI.Image> ().sprite = obj as Sprite;
                }

            } );

    }

}
 

