using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager_005
{
    MyPlayer_005 _myPlayer;
    Dictionary<int, Player_005> _players = new Dictionary<int, Player_005>();

    public static PlayerManager_005 Instance { get; } = new PlayerManager_005();

    public void Add(S_PlayerList packet)
    {
        Object obj = Resources.Load("Player_005");

        foreach (S_PlayerList.Player p in packet.players)
        {
             GameObject go = Object.Instantiate(obj) as GameObject;

            if(p.isSelf)
            {
                MyPlayer_005 myPlayer = go.AddComponent<MyPlayer_005>();
                myPlayer.PlayerId = p.playerId;
                myPlayer.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                _myPlayer = myPlayer;
            }

            else
            {
                Player_005 player = go.AddComponent<Player_005>();
                player.PlayerId = p.playerId;
                player.transform.position = new Vector3(p.posX, p.posY, p.posZ);
                _players.Add(p.playerId, player);
            }
        }
    }

    public void Move(S_BroadcastMove packet)
    {
        if(_myPlayer.PlayerId == packet.playerId)
        {
            _myPlayer.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        }
        else
        {
            Player_005 player = null;
            if (_players.TryGetValue(packet.playerId, out player))
            {
                player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
            }
        }
    }

    public void LeaveGame(S_BroadcastLeaveGame packet)
    {
        if(_myPlayer.PlayerId == packet.playerId)
        {
            GameObject.Destroy(_myPlayer.gameObject);
            _myPlayer = null;
        }
        else
        {
            Player_005 player = null;
            if(_players.TryGetValue(packet.playerId, out player))
            {
                GameObject.Destroy(player.gameObject);
                _players.Remove(packet.playerId);
            }

        }
    }

    public void EnterGame(S_BroadcastEnterGame packet)
    {
        if (packet.playerId == _myPlayer.PlayerId)
            return;

        Object obj = Resources.Load("Player_005");
        GameObject go = Object.Instantiate(obj) as GameObject;

        Player_005 player = go.AddComponent<Player_005>();
        player.transform.position = new Vector3(packet.posX, packet.posY, packet.posZ);
        _players.Add(packet.playerId, player);
    }

}
