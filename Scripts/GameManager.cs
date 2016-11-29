using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GameManager : MonoBehaviour {

    public static GameManager instance;

    public MatchSettings matchSettings;

    void Awake()
    {
        if(instance != null)
        {
            Debug.LogError("Mais de 1 GameManager na Cena!");
        }
        else
        {
            instance = this;
        }
    }

    #region Player tracking


    private static Dictionary<string, PlayerManager> players =  new Dictionary<string,PlayerManager>();
    private const string PLAYER_ID_PREFIX ="Player";
	// Use this for initialization
    public static void RegisterPlayer(string _netId, PlayerManager _player)
    {
        string _playerId = PLAYER_ID_PREFIX + _netId;

        players.Add(_playerId, _player);
        _player.transform.name = _playerId;

    }

    public static void UnRegisterPlayer(string _playerID)
    {

        players.Remove(_playerID);
    }

  /*  void OnGUI()
    {
        GUILayout.BeginArea(new Rect(200, 200, 200, 500));
        GUILayout.BeginVertical();
        foreach(string _playerId in players.Keys)
        {
            GUILayout.Label(_playerId + " - " + players[_playerId].transform.name);
        }
        GUILayout.EndVertical();
        GUILayout.EndArea();
    }
    */
    public static PlayerManager GetPlayer(string _PlayerId)
    {
        return players[_PlayerId];

    }
    #endregion
}
