using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Linq;
using System.Linq;
using UniRx;
using UnityEngine.SceneManagement;

//Sceane을  Main 0
//          Test 1 순으로 등록할것 


//TODO : [ Criticality : Middle ]
//초기화시 AssetBundleManager가 계속 생성되는 문제가있음 
//4번 사운드 호출문제가 있음

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {

        SceneManager.LoadScene ( "Test" , LoadSceneMode.Additive );

        var mainPanel   = GameObject.Find ( "MainPanel" );
        var mainCanvas = mainPanel.Parent ().GetComponent<Canvas> ();

        int i = 0;
        foreach ( var bt in mainPanel.Children ().Where ( ( x ) => x.name.StartsWith ( "Button" ) ) ) {

            var count = i++;

            bt.Child ( "Text" ).GetComponent<Text> ().text = "Sample " + count;

            bt.GetComponent<Button> ().OnClickAsObservable().Subscribe( ( x ) => {

                var temp = System.Type.GetType ( "AssetBundleUniRxSample" + count );

                GameObject.Find ( "TestCanvas" ).AddComponent ( temp );

                mainCanvas.enabled = false;

            } );


        }



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
