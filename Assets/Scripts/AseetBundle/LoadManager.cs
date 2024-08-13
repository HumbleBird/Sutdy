using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoadManager : MonoBehaviour
{
    IEnumerator Start()
    {
        AssetBundle asset = AssetBundle.LoadFromFile("Bundle/monster");

        if(asset == null)
        {
            yield break;
        }

        var killer = asset.LoadAsset<GameObject>("Killer_01");
        var prefab = Instantiate(killer);
        killer.transform.position = Vector3.zero;

        yield return new WaitUntil(() => Input.GetMouseButtonDown(0));
        asset.Unload(true); // Load시 메모리에 할당하기 때문에 반ㄷ시 Unload로 메모리에서 해제해야 한다.
        Destroy(prefab);
    }
}
