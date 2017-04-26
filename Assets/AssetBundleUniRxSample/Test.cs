using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using UnityEngine.SceneManagement;


public class Test : MonoBehaviour {

	// Use this for initialization
	void Start () {
        GameObject.Find ( "Back" ).GetComponent<Button> ()
            .OnClickAsObservable ()
            .Subscribe ( ( x ) => {

                SceneManager.UnloadSceneAsync ( "Main" );
                SceneManager.UnloadSceneAsync ( "Test" );
                SceneManager.LoadScene ( "Main" );
            } );
	}
	
}
