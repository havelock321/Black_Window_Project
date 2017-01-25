using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Events;
using UnityStandardAssets.CrossPlatformInput;
using UnityEngine.UI;
using System.Collections.Generic;

[System.Serializable]
public class ToggleEvent : UnityEvent<bool> { }

public class Player : NetworkBehaviour
{

    [SyncVar (hook = "OnNameChanged")]
    public string playerName;

    [SyncVar (hook ="OnColorChanged")]
    public Color playerColor;

    [SerializeField]
    ToggleEvent onToggleShared;
    [SerializeField]
    ToggleEvent onToggleLocal;
    [SerializeField]
    ToggleEvent onToggleRemote;
    [SerializeField]
    float respawnTime = 5f;

    [SerializeField]
    private string weaponLayer = "Weapon";
    NetworkAnimator anim;

    [SerializeField]
    public static GameObject currentWeapon;

    GameObject mainCamera;

    static List<Player> players = new List<Player>();


    void Start()
    {
        anim = GetComponent<NetworkAnimator>();

        mainCamera = Camera.main.gameObject;

        EnablePlayer();

        SetLayerRecursively(currentWeapon, LayerMask.NameToLayer(weaponLayer));
    }

    [ServerCallback]
    void OnEnabled()
    {
        if (!players.Contains(this))
            players.Add(this);

    }

    [ServerCallback]
    void OnDisabled()
    {
        if (players.Contains(this))
            players.Remove(this);
    }

    void Update()
    {
        if (!isLocalPlayer)
            return;

        anim.animator.SetFloat("Speed", Input.GetAxis("Vertical"));
        anim.animator.SetFloat("Strafe",Input.GetAxis("Horizontal"));

        if (CrossPlatformInputManager.GetButtonDown("Jump"))
            anim.animator.SetBool("Jump", true);
        else anim.animator.SetBool("Jump", false);

    }

    void DisablePlayer()
    {
        if (isLocalPlayer)
        {
            mainCamera.SetActive(true);
            PlayerCanvas.canvas.HideReticule();
        }

        onToggleShared.Invoke(false);

        if (isLocalPlayer)
            onToggleLocal.Invoke(false);
        else
            onToggleRemote.Invoke(false);
    }

    void EnablePlayer()
    {
        if (isLocalPlayer)
        {
            PlayerCanvas.canvas.Initialize();
            mainCamera.SetActive(false);
        }

        onToggleShared.Invoke(true);

        if (isLocalPlayer)
            onToggleLocal.Invoke(true);
        else
            onToggleRemote.Invoke(true);
    }

    public void Die()
    {

        if (isLocalPlayer || playerControllerId == -1)
        {
            PlayerCanvas.canvas.PlayDeathAudio();
            anim.SetTrigger("Dead");

        }

        if (isLocalPlayer)
        {
            PlayerCanvas.canvas.WriteGameStatusText("Você Morreu!");
            PlayerCanvas.canvas.PlayDeathAudio();
            anim.SetTrigger("Dead");
        }
        DisablePlayer();

        Invoke("Respawn", respawnTime);
    }

    void Respawn()
    {
        //ControllerId quando é -1 significa que é algo controlado pelo servidor
        if (isLocalPlayer || playerControllerId == -1)
            anim.SetTrigger("Restart");

        if (isLocalPlayer)
        {
            Transform spawn = NetworkManager.singleton.GetStartPosition();
            transform.position = spawn.position;
            transform.rotation = spawn.rotation;
            anim.SetTrigger("Restart");
        }

        EnablePlayer();
    }

    public static void SetLayerRecursively(GameObject _obj, int _newLayer)
    {
        if (_obj == null)
            return;

        _obj.layer = _newLayer;

        foreach (Transform _child in _obj.transform)
        {
            if (_child == null)
                continue;

            SetLayerRecursively(_child.gameObject, _newLayer);
        }


    }

    void OnNameChanged(string value)
    {
        playerName = value;
        gameObject.name = playerName;

        //Set text

        GetComponentInChildren<Text>(true).text = playerName;
    }

    void OnColorChanged(Color value)
    {
        playerColor = value;
        GetComponentInChildren<RendererToggler>().ChangeColor(playerColor);
    }


    [Server]
    public void Won()
    {

        for (int i = 0; i < players.Count; i++)
            players[i].RpcGameOver(netId, name);


        //falar para os outros q ganhou

        //voltar para o lobby
        Invoke("BackToLobby", 10f);
    }

    [ClientRpc]
    void RpcGameOver(NetworkInstanceId networkId, string name)
    {

        DisablePlayer();

        Cursor.lockState = CursorLockMode.None;

        Cursor.visible = true;

        if(isLocalPlayer)
        {
            if (netId == networkId)
                PlayerCanvas.canvas.WriteGameStatusText("Você ganhou!");
            else
                PlayerCanvas.canvas.WriteGameStatusText("Fim do jogo!\n" + name + " ganhou a partida!");
        }
    }

    void BackToLobby()
    {
        FindObjectOfType<NetworkLobbyManager>().SendReturnToLobby();

    }

  
}