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
        // �����δ� yield retrun �ð��� �ƴ϶� �ؾ� �ϴ� �ൿ�� �����Ű�� progress�� �ø���
        // �ε��� ������ ������ ������ �Ű澵�� ex) �� �ҷ����⸦ �����ϰ� ������ ��ġ�ϱ� ��
        yield return new WaitForSecondsRealtime(1f);
        GameObject map = GameManager.Resource.Load<GameObject>("Map/SampleMap");
        gameInterFace = GameManager.Resource.Load<GameObject>("Map/GameInterFace");
        gameInterFace = GameManager.Resource.Instantiate(gameInterFace);
        GameManager.Resource.Instantiate(map);
        GameObject player = GameManager.Resource.Load<GameObject>("Map/Player");
        GameManager.Resource.Instantiate(player);
        // �÷��̷��� sceneobject�� �����Ұ�
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
