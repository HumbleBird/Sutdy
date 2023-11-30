using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;
using TMPro;

[Serializable]
public class CharacterInfo
{
    public string serverId;
    public string characterId;
    public string characterName;
    public int level;
    public string jobId;
    public string jobGrowId;
    public string jobName;
    public string jobGrowName;
    public int fame;

}

[Serializable]
public class CharacterRoot
{
    public List<CharacterInfo> rows;

}

[Serializable]
public class ServerRoot
{
    public List<ServerInfo> rows;
}

[Serializable]
public class ServerInfo
{
    public string serverId;
    public string serverName;
}

public class DFSearch_Server : MonoBehaviour
{
    public TMP_Dropdown serverList;
    public TextMeshProUGUI selectedServerName;
    string serverid = "";
    string characterName = "";
    public TMP_InputField inputText;
    string characterId = "";
    public RawImage _image;

    ServerRoot serverData = new ServerRoot();
    CharacterRoot characterData = new CharacterRoot();

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ServerRequest());
    }

    public void CharacterSearch()
    {
        string temp = selectedServerName.text;
        serverid = serverData.rows.Find(x => x.serverName == temp).serverId;

        characterName = UnityWebRequest.EscapeURL(inputText.text);

        StartCoroutine(CharacterRequest(serverid, characterName));

    }

    // 캐릭터 이미지 가져오기
    IEnumerator CharacterImageRequest(string serverId, string characterId)
    {
        string url = $"https://img-api.neople.co.kr/df/servers/{serverId}/characters/{characterId}?zoom=1";

        UnityWebRequest www = UnityWebRequestTexture.GetTexture(url);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            _image.texture = ((DownloadHandlerTexture)www.downloadHandler).texture;

        }
        else
        {
            Debug.Log(www.error);
        }
    }

    // 캐릭터 정보 가져오기
    IEnumerator CharacterRequest(string serverId, string characterName)
    {
        string url = $"https://api.neople.co.kr/df/servers/{serverId}/characters?characterName={characterName}&apikey=hr63tfE6WGawf0pHVKryTYmVubIGGuwg";

        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if (www.error == null)
        {
            characterData = JsonUtility.FromJson<CharacterRoot>(www.downloadHandler.text);
            characterId = characterData.rows.Find(x => x.characterName == inputText.text).characterId;

            StartCoroutine(CharacterImageRequest(serverid, characterId));
        }

        else
        {
            Debug.Log("Error");
        }
    }

    // 서버 정보 가져오기
    IEnumerator ServerRequest()
    {
        string url = "https://api.neople.co.kr/df/servers?apikey=hr63tfE6WGawf0pHVKryTYmVubIGGuwg";
        UnityWebRequest www = UnityWebRequest.Get(url);

        yield return www.SendWebRequest();

        if(www.error == null)
        {
            serverData =  JsonUtility.FromJson<ServerRoot>(www.downloadHandler.text);

            foreach (var item in serverData.rows)
            {
                TMP_Dropdown.OptionData option = new TMP_Dropdown.OptionData();
                option.text = item.serverName;

                serverList.options.Add(option);
            }
        }
        else
        {
            Debug.Log("Error");
        }
    }
}
