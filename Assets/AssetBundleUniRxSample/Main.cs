using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Linq;
using System.Linq;
using UniRx;
using UnityEngine.SceneManagement;

//TODO : [ Criticality : Middle ]
//초기화시 AssetBundleManager가 계속 생성되는 문제가있음 
//4번 사운드 호출문제가 있음

public class Main : MonoBehaviour {

	// Use this for initialization
	void Start () {

        SceneManager.LoadScene ( 1 , LoadSceneMode.Additive );


        var mainPanel = GameObject.Find ( "MainPanel" );
        int i = 0;
        foreach ( var bt in mainPanel.Children ().Where ( ( x ) => x.name.StartsWith ( "Button" ) ) ) {

            var count = i++;

            bt.GetComponent<Button> ().onClick.AddListener ( () => {

            var temp = System.Type.GetType ( "AssetBundleUniRxSample" + count );
            GameObject.Find ( "TestCanvas" ).AddComponent ( temp );

                mainPanel.Parent ().GetComponent<Canvas> ().enabled = false;

            } );

        }



	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
