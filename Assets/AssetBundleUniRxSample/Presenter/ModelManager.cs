using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UniRx.Triggers;
using UnityEngine;


//MVRP모델에서의 모델에 해당하는것을 관리해주는 싱글턴 매니져
//개별적으로 분리할지 그대로 쓸지 고민중 
//직접 접근하는 방식은 좋은방법은아님
public class ModelManager : Singleton<ModelManager> {

    public ReactiveProperty<int> Money = new ReactiveProperty<int> ( 0 );

    public ReactiveProperty<float> patchPersent = new ReactiveProperty<float> ( 0f );

    public ReactiveProperty<bool> isRestartScenes = new ReactiveProperty<bool> ( false );

    public ReactiveProperty<string> messageTitle = new ReactiveProperty<string> ( "" );

    public ReactiveProperty<string> messageMsg = new ReactiveProperty<string> ( "" );

    public ReactiveProperty<bool> isErrorState = new ReactiveProperty<bool> ( false );


}

