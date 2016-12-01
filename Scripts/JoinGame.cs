using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.Networking.Match;

public class JoinGame : MonoBehaviour {

    List<GameObject> roomList = new List<GameObject>();
    private NetworkManager netWorkManager;
    [SerializeField]
    private Text status;

    [SerializeField]
    private GameObject roomListItemPrefab;

    [SerializeField]
    private Transform roomListParent;
    void Start()
    {
        netWorkManager = NetworkManager.singleton;

        if(netWorkManager.matchMaker == null)
        {

            netWorkManager.StartMatchMaker();
        }

        RefreshRoomList();
    }

    public void RefreshRoomList()
    {
        ClearRoomList();
        netWorkManager.matchMaker.ListMatches(0,20,"",false,0,0, OnMatchList);
        status.text = "Carregando..";

    }

    public void OnMatchList(bool sucess, string  extendedInfo, List<MatchInfoSnapshot> matchList)
    {
        status.text = "";
       if(!sucess ||matchList == null)
        {
            status.text = "Não consegui obter as salas.";
            return;
        }

        ClearRoomList(); 
        foreach(MatchInfoSnapshot match in matchList)
        {
            GameObject _roomListItemGO = Instantiate(roomListItemPrefab);
            _roomListItemGO.transform.SetParent(roomListParent);

            RoomListItem _roomListItem = _roomListItemGO.GetComponent<RoomListItem>();
            if(_roomListItem != null)
            {
                _roomListItem.SetUp(match,JoinRoom);
            }
            roomList.Add(_roomListItemGO);
        }

        if (roomList.Count == 0)
            status.text = "Nenhuma sala nesse momento!";
    }

    void ClearRoomList()
    {
        for (int i = 0; i < roomList.Count; i++)
        {
            Destroy(roomList[i]);

        }

        roomList.Clear();

    }

    public void JoinRoom(MatchInfoSnapshot _match)
    {

        //
        netWorkManager.matchMaker.JoinMatch(_match.networkId, "","","",0,0, netWorkManager.OnMatchJoined);
        ClearRoomList();
        status.text = "Entrando..";
    }
}
