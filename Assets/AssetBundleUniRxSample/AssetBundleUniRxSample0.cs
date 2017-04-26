using AssetBundles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//상단의 AssetBundles메뉴 안에는 
//Simulation Mode               -> 선택시 서버를 사용하지않고 streamingAssets폴더에서 직접가져옴
//                              -> 선택하지 않으면 AssetBundleManager 폴더 안의Resources 폴더에 있는 url.txt안의 링크를 사용함
//Build AssetBundles            -> 에셋번들을 현재 플랫폼에 맞게 streamingAssets폴더에 생성
//Rebuild Build AssetBundles    -> 변경된 부분만 재빌드
//Build Player                  -> 현재 플랫폼으로 빌드됨
public class AssetBundleUniRxSample0 : MonoBehaviour {

    private const string assetBundle_URL  = "https://test.com/AssetBundles/";

    void Start () {

        //테스트 하기위해 url.txt를 사용할지 변수 assetBundle_URL를 사용할지 선택하여
        //밑에 함수를 선택적으로 사용할것 

        //DEVELOPMENT_BUILD || UNITY_EDITOR
        AssetBundleManager.SetDevelopmentAssetBundleServer ();
        Debug.Log ( "Editoer : "+ AssetBundleManager.BaseDownloadingURL );

        //other // 직접 입력
        AssetBundleManager.SetSourceAssetBundleURL ( assetBundle_URL );
        Debug.Log ("Other : " + AssetBundleManager.BaseDownloadingURL );
    }


    private void InitializeSourceURL () {
        // If ODR is available and enabled, then use it and let Xcode handle download requests.
#if ENABLE_IOS_ON_DEMAND_RESOURCES
        if (UnityEngine.iOS.OnDemandResources.enabled)
        {
            AssetBundleManager.SetSourceAssetBundleURL("odr://");
            return;
        }
#endif
#if DEVELOPMENT_BUILD || UNITY_EDITOR
        // With this code, when in-editor or using a development builds: Always use the AssetBundle Server
        // (This is very dependent on the production workflow of the project.
        //      Another approach would be to make this configurable in the standalone player.)
        AssetBundleManager.SetDevelopmentAssetBundleServer ();
        return;
#else
        // Use the following code if AssetBundles are embedded in the project for example via StreamingAssets folder etc:
        //AssetBundleManager.SetSourceAssetBundleURL(Application.dataPath + "/");
        // Or customize the URL based on your deployment or configuration
        AssetBundleManager.SetSourceAssetBundleURL(assetBundle_URL);
        return;
#endif
    }
}
