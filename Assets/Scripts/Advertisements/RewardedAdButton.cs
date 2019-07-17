using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Monetization;

[RequireComponent (typeof (Button))]
public class RewardedAdButton : MonoBehaviour {

    private Button adButton;

    void Start ()
    {
        adButton = GetComponent<Button> ();

        if (adButton)
            adButton.onClick.AddListener (ShowAd);
    }

    void Update ()
    {
        if (adButton)
            adButton.interactable = Monetization.IsReady("rewardedVideo");
    }

    public void ShowAd ()
    {
        ShowAdCallbacks options = new ShowAdCallbacks ();
        options.finishCallback = HandleShowResult;
        ShowAdPlacementContent ad = Monetization.GetPlacementContent("rewardedVideo") as ShowAdPlacementContent;
        ad.Show (options);
    }

    void HandleShowResult (ShowResult result) {
        if (result == ShowResult.Finished) {
            FindObjectOfType<MenuSystem>().Continue();
        } else if (result == ShowResult.Skipped) {
            Debug.LogWarning ("The player skipped the video.");
        } else if (result == ShowResult.Failed) {
            Debug.LogError ("Video failed to show.");
        }
    }
}