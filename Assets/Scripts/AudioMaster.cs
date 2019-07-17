using UnityEngine;

public class AudioMaster : MonoBehaviour
{
    [SerializeField] UnityEngine.UI.Image image = null;
    [SerializeField] Sprite audioOnSprite = null;
    [SerializeField] Sprite audioOffSprite = null;
    [SerializeField] AudioSource source = null;

    #region Audio On (property)
    bool AudioOn{
        get => PlayerPrefs.GetInt("AudioOn") == 1;
        set {
            PlayerPrefs.SetInt("AudioOn", value ? 1 : 0);
            image.sprite   = value ? audioOnSprite : audioOffSprite;
            source.enabled = value;
        }
    }
    #endregion

    private void Start()  => AudioOn =  AudioOn;
    public  void Toggle() => AudioOn = !AudioOn;
}