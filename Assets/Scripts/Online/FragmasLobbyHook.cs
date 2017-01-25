using Prototype.NetworkLobby;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class FragmasLobbyHook : LobbyHook {

    public override void OnLobbyServerSceneLoadedForPlayer(NetworkManager manager, GameObject lobbyPlayer, GameObject gamePlayer)
    {
        base.OnLobbyServerSceneLoadedForPlayer(manager, lobbyPlayer, gamePlayer);
        LobbyPlayer lPlayer = lobbyPlayer.GetComponent<LobbyPlayer>();

        Player gPlayer = gamePlayer.GetComponent<Player>();

        gPlayer.playerName = lPlayer.playerName;

        gPlayer.playerColor = lPlayer.playerColor;
    }
}
