using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScene : BaseScene
{
    protected override IEnumerator LoadingRoutine()
    {
        yield return StartCoroutine(InitRoutine());
        progress = 1f;
    }

    IEnumerator InitRoutine()
    {
        // temporary loading time ���� �� �ڷ�ƾ �ȿ� �ִ� ������ �� ���ÿ� �����Դϴ�.
        // ������ �ۼ��Ҷ� ���� ������ ����� �ۼ��Ͻø� �˴ϴ�.
        // �����δ� yield retrun �ð��� �ƴ϶� �ؾ� �ϴ� �ൿ�� �����Ű�� progress�� �ø���
        // �ε��� ������ ������ ������ �Ű澵�� ex) �� �ҷ����⸦ �����ϰ� ������ ��ġ�ϱ� ��
        yield return new WaitForSecondsRealtime(1f);
        progress = 0.7f;
        yield return new WaitForSecondsRealtime(2f);

        yield return null;
    }
}