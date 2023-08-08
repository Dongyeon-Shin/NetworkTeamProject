using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameScene : BaseScene
{
    public static GameObject gameInterFace;
    protected override IEnumerator LoadingRoutine()
    {
        yield return StartCoroutine(InitRoutine());
        progress = 1f;
    }

    IEnumerator InitRoutine()
    {
        // temporary loading time
        // 실제로는 yield retrun 시간이 아니라 해야 하는 행동을 실행시키고 progress를 올릴것
        // 로딩할 순서도 문제가 없도록 신경쓸것 ex) 맵 불러오기를 먼저하고 아이템 배치하기 등
        yield return new WaitForSecondsRealtime(1f);
        GameObject map = GameManager.Resource.Load<GameObject>("Map/SampleMap");
        gameInterFace = GameManager.Resource.Load<GameObject>("Map/GameInterFace");
        gameInterFace = GameManager.Resource.Instantiate(gameInterFace);
        GameManager.Resource.Instantiate(map);
        GameObject player = GameManager.Resource.Load<GameObject>("Map/Player");
        GameManager.Resource.Instantiate(player);
        // 플레이러는 sceneobject로 생성할것
        //PhotonNetwork.InstantiateSceneObject("Player");
        progress = 0.7f;
        yield return new WaitForSecondsRealtime(2f);

        yield return null;
    }

    IEnumerator GameStartRoutine()
    {
        yield return null;
    }

    IEnumerator DebugGameStartRoutine()
    {
        yield return null;
    }
}
