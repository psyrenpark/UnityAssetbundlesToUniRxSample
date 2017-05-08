using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Unity.Linq;
using System.Linq;
using System;

[System.Serializable]
public class GUIObject : MonoBehaviour  {

    //TODO: 설정해야할듯 
    enum PanelType {
        BAR,
        CONTENT,
        TEMPORARY,
        CONTROL
    }

    [SerializeField]
    private GameObject      canvasObject;
    [SerializeField]
    private Canvas          canvas;
    [SerializeField]
    private GameObject[]    panels = new GameObject[ 0 ];

    public GameObject       GetGameObject   { get { return canvasObject; } }
    public Canvas           GetCanvas       { get { return canvas; } }
    public GameObject       GetPenel    (string name) {
        return panels.Where ( ( x ) => x.name == name ).Single ();
    }
    [SerializeField]
    public readonly  string GuiName;
    [SerializeField]
    public readonly  string AssetBundlePath;

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="canvasPrefab">생성된 캔바스 게임오브젝트</param>
    public GUIObject ( string AssetBundlePath , string GuiName , GameObject go ) {

        this.GuiName            = GuiName;
        this.AssetBundlePath    = AssetBundlePath;
        this.canvasObject       = go;
        this.canvas             = this.canvasObject.GetComponent<Canvas> ();

        var penelCount = canvasObject.Children ().ToArrayNonAlloc ( x => x.name.EndsWith ( "Panel" ) , ref panels );
        /*
        for ( int i = 0 ; i < penelCount ; i++ ) {
            panels[ i ].SetActive ( false );
            //TODO: 캔버스 용도에 따라 행동 

        }
        */
    }

    public void  reset () {

        var penelCount = panels.Length;
        for ( int i = 0 ; i < penelCount ; i++ ) {
            panels[ i ].SetActive ( false );
            //TODO: 캔버스 용도에 따라 행동 

        }

    }

}
