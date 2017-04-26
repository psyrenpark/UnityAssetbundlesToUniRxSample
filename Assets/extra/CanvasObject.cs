using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;
using Unity.Linq;
using System.Linq;

public class CanvasObject  {

    //TODO: 설정해야할듯 
    enum PanelType {
        BAR,
        CONTENT,
        TEMPORARY,
        CONTROL
    }


    private GameObject      canvasObject;
    private Canvas          canvas;
    private GameObject[]    panels = new GameObject[ 0 ];

    public GameObject       GetGameObject   { get { return canvasObject; } }
    public Canvas           GetCanvas       { get { return canvas; } }
    public GameObject       GetPenel    (string name) {
        return panels.Where ( ( x ) => x.name == name ).Single ();
    }

    public string       GuiName             { get; set; }
    //public string       AssetBundleName     { get { return StaticMethod.GetAssetBundleName ( GuiName ); } }

    /// <summary>
    /// 생성자
    /// </summary>
    /// <param name="canvasPrefab">생성된 캔바스 게임오브젝트</param>
    public CanvasObject ( string Name ) {

        this.GuiName        = Name;
        //this.canvasObject   = ResourcesManager.GetInst.gameObject
        //                        .Child ( ResourcesManager.GetInst.GuiObjectsParentName )
        //                        .Child ( GuiName );
        this.canvas         = this.canvasObject.GetComponent<Canvas> ();


        var penelCount = canvasObject.Children ().ToArrayNonAlloc ( x => x.name.EndsWith ( "Panel" ) , ref panels );
        for ( int i = 0 ; i < penelCount ; i++ ) {
            panels[ i ].SetActive ( false );
            //TODO: 캔버스 용도에 따라 행동 

        }

    }

    public void  reset () {

        var penelCount = panels.Length;
        for ( int i = 0 ; i < penelCount ; i++ ) {
            panels[ i ].SetActive ( false );
            //TODO: 캔버스 용도에 따라 행동 

        }

    }

}
