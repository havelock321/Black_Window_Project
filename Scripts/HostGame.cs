using UnityEngine;
using UnityEngine.Networking;
public class HostGame : MonoBehaviour {

    [SerializeField]
    private uint roomSize =  6;

    private string roomName ="Party";

    private NetworkManager networkManager;

    void Start()
    {

        networkManager = NetworkManager.singleton;
        if(networkManager.matchMaker == null)
        {
            networkManager.StartMatchMaker();
        }
    }

    public void SetRoomName(string _name)
    {
        roomName = _name;
        Debug.Log("Nome Atual " + roomName);
    }
    

    public void CreateRoom()
    {
        if(roomName != "" && roomName != null)
        {
            Debug.Log("Criou o Room " + roomName);
           networkManager.matchMaker.CreateMatch(roomName, roomSize, true, "","","",0,0,networkManager.OnMatchCreate);

        }

    }

}
