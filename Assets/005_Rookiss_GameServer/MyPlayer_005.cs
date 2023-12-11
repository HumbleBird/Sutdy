using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPlayer_005 : Player_005
{
    NetworkManager _netWork;
    private void Start()
    {
        StartCoroutine(CoSendPacket());
       _netWork = GameObject.Find("NetworkManager").GetComponent<NetworkManager>();
    }

    IEnumerator CoSendPacket()
    {
        while (true)
        {
            yield return new WaitForSeconds(0.25f);

            C_Move movePacket = new C_Move();
            movePacket.posX = UnityEngine.Random.Range(-50, 50);
            movePacket.posY = 0;
            movePacket.posZ = UnityEngine.Random.Range(-50, 50);
            _netWork.Send(movePacket.Write());
        }
    }
}
