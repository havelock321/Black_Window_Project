using UnityEngine;
using UnityEngine.Networking;
public class PlayerSetUp : NetworkBehaviour
{
    [SerializeField]
    Behaviour[] componentsToDisable;
    // Use this for initialization
    Camera sceneCamera;
    [SerializeField]
    string remoteLayerName = "RemotePlayer";

    [SerializeField]
    string dontDrawLayerName = "DontDrawn"; //Para não desenhar algumas coisas na camera

    [SerializeField]
    GameObject playerGraphics;

    [SerializeField]
    GameObject playerUIPreFab;

    private GameObject playerUIinstance;


    void Start()
    {

        if (!isLocalPlayer)
        {
            DisableComponents();
            AssingRemoteLayer();


        }
        else
        {
            sceneCamera = Camera.main;

            if (sceneCamera != null)
            {
                sceneCamera.gameObject.SetActive(false);
            }

            //Disable player gaphics for local player

            SetLayerRecursively(playerGraphics, LayerMask.NameToLayer(dontDrawLayerName));

            //Create Player UI
            playerUIinstance = Instantiate(playerUIPreFab);
            playerUIinstance.name = playerUIPreFab.name;
        }

        GetComponent<PlayerManager>().SetUp();

    }

    public override void OnStartClient()
    {
        base.OnStartClient();
        string _NetId = GetComponent<NetworkIdentity>().netId.ToString();
        PlayerManager _Player = GetComponent<PlayerManager>();

        GameManager.RegisterPlayer(_NetId, _Player);
    }

    void OnDiblase()
    {
        Destroy(playerUIinstance);
    
        if (sceneCamera != null)
        {
            sceneCamera.gameObject.SetActive(true);
        }
        GameManager.UnRegisterPlayer(transform.name);

    }

    void DisableComponents()
    {
        for (int i = 0; i < componentsToDisable.Length; i++)
        {
            componentsToDisable[i].enabled = false;
        }

    }
    void AssingRemoteLayer()
    {
        gameObject.layer = LayerMask.NameToLayer(remoteLayerName); //setando o layer do clientes..

    }
    public override void OnStartLocalPlayer()
    {
        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);

    }


    public override void PreStartClient()
    {

        GetComponent<NetworkAnimator>().SetParameterAutoSend(0, true);


    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {

        obj.layer = newLayer;

  
       foreach (Transform child in obj.transform)
        {
            // child.gameObject.layer = newLayer;
            if (child.gameObject.name == "polySurface16" || child.gameObject.name == "polySurface18" ||
                 child.gameObject.name == "polySurface9" || child.gameObject.name == "polySurface13"
                 || child.gameObject.name == "polySurface7")
                continue;

            SetLayerRecursively(child.gameObject, newLayer); //chama ela mesma
        }
    }
}
