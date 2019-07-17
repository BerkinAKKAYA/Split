using UnityEngine;
using UnityEngine.Monetization;
using System.Collections;

public class AdManager : MonoBehaviour
{
    private string gameId = "3214113";

    private void Start() => Monetization.Initialize (gameId, false);
    public void ShowAd() => StartCoroutine("ShowAdWhenReady");

    #region ShowAdWhenReady
    IEnumerator ShowAdWhenReady ()
    {
        while (!Monetization.IsReady("video"))
            yield return new WaitForSeconds(0.25f);

        ShowAdPlacementContent ad = null;
        ad = Monetization.GetPlacementContent("video") as ShowAdPlacementContent;

        if (ad != null)
            ad.Show ();
    }
    #endregion
}