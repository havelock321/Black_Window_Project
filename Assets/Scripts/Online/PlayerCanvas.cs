using UnityEngine;
using UnityEngine.UI;

public class PlayerCanvas : MonoBehaviour
{
    public static PlayerCanvas canvas;

    [Header("Component References")]
    [SerializeField]
    Image reticule;
    [SerializeField]
    UIFader damageImage;
    [SerializeField]
    Text gameStatusText;
    [SerializeField]
    Text healthValue;
    [SerializeField]
    Text killsValue;
    [SerializeField]
    Text logText;
    [SerializeField]
    AudioSource deathAudio;

    [SerializeField]
    [Header("CROSSHAIR")]
    RectTransform[] crosshairs;

    public static PlayerCanvas instance;

    float walkSize;


    //Ensure there is only one PlayerCanvas
    void Awake()
    {
        if (canvas == null)
            canvas = this;
        else if (canvas != this)
            Destroy(gameObject);

        walkSize = crosshairs[0].localPosition.y;

        instance = this;
    }

    void Update()
    {
        UpdateCrosshair();
    }

    //Find all of our resources
    void Reset()
    {
        reticule = GameObject.Find("Reticule").GetComponent<Image>();
        damageImage = GameObject.Find("DamagedFlash").GetComponent<UIFader>();
        gameStatusText = GameObject.Find("GameStatusText").GetComponent<Text>();
        healthValue = GameObject.Find("HealthValue").GetComponent<Text>();
        killsValue = GameObject.Find("KillsValue").GetComponent<Text>();
        logText = GameObject.Find("LogText").GetComponent<Text>();
        deathAudio = GameObject.Find("DeathAudio").GetComponent<AudioSource>();
    }

    public void Initialize()
    {
        reticule.enabled = true;
        gameStatusText.text = "";
    }

    public void HideReticule()
    {
        reticule.enabled = false;
    }

    public void FlashDamageEffect()
    {
        damageImage.Flash();
    }

    public void PlayDeathAudio()
    {
        if (!deathAudio.isPlaying)
            deathAudio.Play();

        damageImage.StartFadeOut();
    }

    public void SetKills(int amount)
    {
        killsValue.text = amount.ToString();
    }

    public void SetHealth(int amount)
    {
        healthValue.text = amount.ToString();
    }

    public void WriteGameStatusText(string text)
    {
        gameStatusText.text = text;
    }

    public void WriteLogText(string text, float duration)
    {
        CancelInvoke();
        logText.text = text;
        Invoke("ClearLogText", duration);
    }

    void ClearLogText()
    {
        logText.text = "";
    }

    public void UpdateCrosshair()
    {
        // y+ x+ x- y- 
        float crossHairSize = calculateCrossHair();

        crosshairs[0].localPosition = Vector3.Slerp(crosshairs[0].localPosition, new Vector3(0f, crossHairSize, 0f), Time.deltaTime * 8f);
        crosshairs[1].localPosition = Vector3.Slerp(crosshairs[1].localPosition, new Vector3(crossHairSize, 0f, 0f), Time.deltaTime * 8f);
        crosshairs[2].localPosition = Vector3.Slerp(crosshairs[2].localPosition, new Vector3(-crossHairSize, 0f, 0f), Time.deltaTime * 8f);
        crosshairs[3].localPosition = Vector3.Slerp(crosshairs[3].localPosition, new Vector3(0f, -crossHairSize, 0f), Time.deltaTime * 8f);
    }

    public float calculateCrossHair()
    {
        float size = walkSize *  1;

        if (Input.GetKey(KeyCode.Z) || Input.GetKey(KeyCode.Q) || Input.GetKey(KeyCode.D))
        {
            if (Input.GetKey(KeyCode.LeftShift))
                size *= 2;
        }
        else
            size /= 2;

        return size;
    }
}