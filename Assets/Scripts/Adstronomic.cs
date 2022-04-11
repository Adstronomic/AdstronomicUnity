using System.Collections;
using System.Collections.Generic;
using UnityEngine.Networking;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEditor;

public class Adstronomic : MonoBehaviour
{
    private string version = "1.0.0";
    private string api = "https://europe-west1-adstronomic-f1bb2.cloudfunctions.net/";
    private string campaignId = "";
    private float videoDelay = 0.0f;

    private GameObject canvas;

    private AdObject bannerAdObject;
    private AdObject bannerAdObjectBuffer;

    private GameObject bannerAdImage;
    private GameObject bannerAdImageBuffer;

    private AdObject interstitialAdObject;
    private AdObject interstitialAdObjectBuffer;

    private GameObject interstitialAdVideo;
    private RenderTexture interstitialAdRender;
    private RenderTexture interstitialAdRenderBuffer;

    private AdObject rewardedAdObject;
    private AdObject rewardedAdObjectBuffer;

    private GameObject rewardedAdVideo;
    private RenderTexture rewardedAdRender;
    private RenderTexture rewardedAdRenderBuffer;

    private GameObject videoBackground;
    private GameObject videoClose;

    public GameObject mainScene;

    private void Start()
    {

    }

    private void Update()
    {
        if(videoDelay > 0.0f) {
            videoDelay -= Time.deltaTime;

            this.videoClose.GetComponentInChildren<Text>().text = (Mathf.CeilToInt(videoDelay)).ToString();
        } else if(videoDelay < 0.0f) {
            videoDelay = 0.0f;

            this.videoClose.GetComponentInChildren<Text>().text = "X";
        }
    }

    public void initialize(string campaignId) {
        this.campaignId = campaignId;

        GameObject eventSystem = new GameObject();
        eventSystem.AddComponent<EventSystem>();
        eventSystem.AddComponent<StandaloneInputModule>();
        eventSystem.name = "Event System";

        this.canvas = new GameObject();
        this.canvas.name = "Canvas";

        this.canvas.AddComponent<Canvas>();
        this.canvas.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;

        this.canvas.AddComponent<GraphicRaycaster>();

        this.bannerAdObject = new AdObject();
        this.bannerAdObjectBuffer = new AdObject();

        this.bannerAdImageBuffer = new GameObject();
        this.bannerAdImageBuffer.transform.SetParent(this.canvas.transform);
        this.bannerAdImageBuffer.SetActive(false);
        this.bannerAdImageBuffer.name = "Banner Ad Buffer";

        this.bannerAdImageBuffer.AddComponent<RectTransform>();
        this.bannerAdImageBuffer.GetComponent<RectTransform>().anchorMin = new Vector2(0.5f, 0.0f);
        this.bannerAdImageBuffer.GetComponent<RectTransform>().anchorMax = new Vector2(0.5f, 0.0f);
        this.bannerAdImageBuffer.AddComponent<Image>();

        this.interstitialAdVideo = new GameObject();
        this.interstitialAdVideo.transform.SetParent(this.canvas.transform);
        this.interstitialAdVideo.name = "Interstitial Ad";

        this.interstitialAdVideo.AddComponent<RectTransform>();
        this.interstitialAdVideo.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        this.interstitialAdVideo.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        this.interstitialAdVideo.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        this.interstitialAdVideo.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        this.interstitialAdVideo.AddComponent<RawImage>();
        this.interstitialAdVideo.GetComponent<RawImage>().enabled = false;

        this.interstitialAdVideo.AddComponent<UnityEngine.Video.VideoPlayer>();

        this.interstitialAdVideo.AddComponent<UnityEngine.UI.Button>();
        this.interstitialAdVideo.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => StartCoroutine(this.clickInterstitial()));

        this.rewardedAdVideo = new GameObject();
        this.rewardedAdVideo.transform.SetParent(this.canvas.transform);
        this.rewardedAdVideo.name = "Rewarded Ad";

        this.rewardedAdVideo.AddComponent<RectTransform>();
        this.rewardedAdVideo.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        this.rewardedAdVideo.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        this.rewardedAdVideo.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        this.rewardedAdVideo.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        this.rewardedAdVideo.AddComponent<RawImage>();
        this.rewardedAdVideo.GetComponent<RawImage>().enabled = false;

        this.rewardedAdVideo.AddComponent<UnityEngine.Video.VideoPlayer>();

        this.rewardedAdVideo.AddComponent<UnityEngine.UI.Button>();
        this.rewardedAdVideo.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => StartCoroutine(this.clickRewarded()));

        this.videoBackground = new GameObject();

        this.videoBackground.transform.SetParent(this.canvas.transform);
        this.videoBackground.transform.SetSiblingIndex(0);
        this.videoBackground.SetActive(false);
        this.videoBackground.name = "Background";

        this.videoBackground.AddComponent<RectTransform>();
        this.videoBackground.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        this.videoBackground.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        this.videoBackground.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        this.videoBackground.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        this.videoBackground.AddComponent<Image>();
        this.videoBackground.GetComponent<Image>().color = new Color(0, 0, 0);

        this.videoClose = new GameObject();
        this.videoClose.transform.SetParent(this.canvas.transform);
        this.videoClose.SetActive(false);
        this.videoClose.name = "Close Button";

        this.videoClose.AddComponent<RectTransform>();
        this.videoClose.GetComponent<RectTransform>().anchoredPosition = new Vector2(100.0f, -100.0f);
        this.videoClose.GetComponent<RectTransform>().anchorMin = new Vector2(0, 1);
        this.videoClose.GetComponent<RectTransform>().anchorMax = new Vector2(0, 1);

        this.videoClose.AddComponent<Image>();
        this.videoClose.GetComponent<Image>().color = Color.grey;

        this.videoClose.AddComponent<Button>();
        this.videoClose.GetComponent<Button>().onClick.AddListener(() => {
            if(videoDelay == 0.0f) {
                this.mainScene.SetActive(true);

                this.videoBackground.SetActive(false);
                this.videoClose.SetActive(false);

                this.interstitialAdVideo.GetComponent<RawImage>().enabled = false;
                (this.interstitialAdVideo.GetComponents(typeof(UnityEngine.Video.VideoPlayer))[0] as UnityEngine.Video.VideoPlayer).enabled = false;

                this.rewardedAdVideo.GetComponent<RawImage>().enabled = false;
                (this.rewardedAdVideo.GetComponents(typeof(UnityEngine.Video.VideoPlayer))[0] as UnityEngine.Video.VideoPlayer).enabled = false;

                this.videoDelay = 1.0f;
            }
        });

        GameObject videoCloseText = new GameObject();
        videoCloseText.transform.SetParent(this.videoClose.transform);
        videoCloseText.name = "Close Text";

        videoCloseText.AddComponent<RectTransform>();
        videoCloseText.GetComponent<RectTransform>().anchorMin = new Vector2(0, 0);
        videoCloseText.GetComponent<RectTransform>().anchorMax = new Vector2(1, 1);
        videoCloseText.GetComponent<RectTransform>().offsetMin = new Vector2(0, 0);
        videoCloseText.GetComponent<RectTransform>().offsetMax = new Vector2(0, 0);

        videoCloseText.AddComponent<Text>();
        videoCloseText.GetComponent<Text>().font = Resources.GetBuiltinResource(typeof(Font), "Arial.ttf") as Font;
        videoCloseText.GetComponent<Text>().fontSize = 20;
        videoCloseText.GetComponent<Text>().alignment = TextAnchor.MiddleCenter;
        videoCloseText.GetComponent<Text>().color = Color.white;

        StartCoroutine(this.fetchBanner());
        StartCoroutine(this.fetchInterstitial());
        StartCoroutine(this.fetchRewarded());
    }

    public IEnumerator loadBanner() {
        if(this.bannerAdObjectBuffer.advertiserCampaignId != "") {
            this.bannerAdObject = this.bannerAdObjectBuffer;

            Destroy(this.bannerAdImage);

            this.bannerAdImage = GameObject.Instantiate(this.bannerAdImageBuffer);
            this.bannerAdImage.transform.SetParent(this.canvas.transform);
            this.bannerAdImage.transform.SetSiblingIndex(2);
            this.bannerAdImage.SetActive(true);
            this.bannerAdImageBuffer.name = "Banner Ad";

            int width = (int)(this.bannerAdImageBuffer.transform.parent.GetComponent<RectTransform>().rect.width);

            this.bannerAdImage.GetComponent<RectTransform>().sizeDelta = new Vector2(width, width / 6.0f);
            this.bannerAdImage.GetComponent<RectTransform>().anchoredPosition = new Vector2(0.0f, width / 12.0f);

            this.bannerAdImage.AddComponent<Button>();
            this.bannerAdImage.GetComponent<Button>().onClick.AddListener(() => StartCoroutine(this.clickBanner()));

            string link = this.api + "adVisualized?adType=banner&publisherCampaignId=" + this.campaignId + "&advertiserCampaignId=" + this.bannerAdObject.advertiserCampaignId + "&deviceID=" + SystemInfo.deviceUniqueIdentifier;

            yield return UnityWebRequest.Get(link).SendWebRequest();

            StartCoroutine(this.fetchBanner());
        }
    }

    private IEnumerator fetchBanner() {
        string link = this.api + "getAd?adType=banner&publisherCampaignId=" + this.campaignId + "&deviceID=" + SystemInfo.deviceUniqueIdentifier;

        UnityWebRequest requestAd = UnityWebRequest.Get(link);

        yield return requestAd.SendWebRequest();

        if(requestAd.result == UnityWebRequest.Result.Success) {
            AdObject responseAd = JsonUtility.FromJson<AdObject>(System.Text.Encoding.ASCII.GetString(requestAd.downloadHandler.data));

            if(new List<string>() { "image/png", "image/jpg", "image/jpeg", "image/webp" }.Contains(responseAd.type)) {
                this.bannerAdObjectBuffer = responseAd;

                UnityWebRequest textureAd = UnityWebRequestTexture.GetTexture(responseAd.url);

                yield return textureAd.SendWebRequest();

                Texture myTexture = DownloadHandlerTexture.GetContent(textureAd);
                this.bannerAdImageBuffer.GetComponent<Image>().sprite = Sprite.Create((Texture2D)myTexture, new Rect(0.0f, 0.0f, myTexture.width, myTexture.height), new Vector2(1.0f, 1.0f), 100.0f);
            } else print("Incorrect Type");
        } else print("Incorrect Datas");
    }

    private IEnumerator clickBanner() {
        string link = this.api + "adClicked?type=banner&publisherCampaignId=" + this.campaignId + "&advertiserCampaignId=" + this.bannerAdObject.advertiserCampaignId + "&deviceID=" + SystemInfo.deviceUniqueIdentifier;

        yield return UnityWebRequest.Get(link).SendWebRequest();

        Application.OpenURL(this.bannerAdObject.redirection);
    }

    public IEnumerator loadInterstitial(GameObject myCamera) {
        if(this.interstitialAdObjectBuffer.advertiserCampaignId != "" && this.videoDelay == 0.0f) {
            this.interstitialAdObject = this.interstitialAdObjectBuffer;

            (this.interstitialAdVideo.GetComponents(typeof(UnityEngine.Video.VideoPlayer))[1] as UnityEngine.Video.VideoPlayer).enabled = true;
            Destroy(this.interstitialAdVideo.GetComponents(typeof(UnityEngine.Video.VideoPlayer))[0] as UnityEngine.Video.VideoPlayer);

            this.interstitialAdVideo.GetComponent<RawImage>().texture = this.interstitialAdRenderBuffer;
            this.interstitialAdVideo.GetComponent<RawImage>().enabled = true;

            this.videoBackground.gameObject.SetActive(true);
            this.videoClose.gameObject.SetActive(true);
            this.videoDelay = 10.0f;

            this.mainScene.SetActive(false);

            StartCoroutine(this.fetchInterstitial());

            string link = this.api + "adVisualized?adType=interstitial&publisherCampaignId=" + this.campaignId + "&advertiserCampaignId=" + this.interstitialAdObject.advertiserCampaignId + "&deviceID=" + SystemInfo.deviceUniqueIdentifier;

            yield return UnityWebRequest.Get(link).SendWebRequest();
        }
    }

    private IEnumerator fetchInterstitial() {
        string link = this.api + "getAd?adType=interstitial&publisherCampaignId=" + this.campaignId + "&deviceID=" + SystemInfo.deviceUniqueIdentifier;

        UnityWebRequest requestAd = UnityWebRequest.Get(link);

        yield return requestAd.SendWebRequest();

        if(requestAd.result == UnityWebRequest.Result.Success) {
            AdObject responseAd = JsonUtility.FromJson<AdObject>(System.Text.Encoding.ASCII.GetString(requestAd.downloadHandler.data));

            if(new List<string>() { "video/mp4", "video/m4a" }.Contains(responseAd.type)) {
                this.interstitialAdObjectBuffer = responseAd;

                this.interstitialAdVideo.AddComponent<UnityEngine.Video.VideoPlayer>();
                UnityEngine.Video.VideoPlayer videoPlayer = this.interstitialAdVideo.GetComponents(typeof(UnityEngine.Video.VideoPlayer))[1] as UnityEngine.Video.VideoPlayer;

                videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;
                videoPlayer.aspectRatio = UnityEngine.Video.VideoAspectRatio.FitInside;
                videoPlayer.url = responseAd.url;
                videoPlayer.Prepare();

                while(!videoPlayer.isPrepared) yield return new WaitForEndOfFrame();

                videoPlayer.enabled = false;

                var canvas = this.interstitialAdVideo.transform.parent.GetComponent<RectTransform>().rect;

                this.interstitialAdRenderBuffer = new RenderTexture((int)(1024 * canvas.width / canvas.height), 1024, 16);
                this.interstitialAdRenderBuffer.Create();

                videoPlayer.targetTexture = this.interstitialAdRenderBuffer;
            } else print("Incorrect Type");
        } else print("Incorrect Datas");
    }

    private IEnumerator clickInterstitial() {
        string link = this.api + "adClicked?type=interstitial&publisherCampaignId=" + this.campaignId + "&advertiserCampaignId=" + this.interstitialAdObject.advertiserCampaignId + "&deviceID=" + SystemInfo.deviceUniqueIdentifier;

        yield return UnityWebRequest.Get(link).SendWebRequest();

        Application.OpenURL(this.interstitialAdObject.redirection);
    }

    public IEnumerator loadRewarded(GameObject myCamera) {
        if(this.rewardedAdObjectBuffer.advertiserCampaignId != "" && this.videoDelay == 0.0f) {
            this.rewardedAdObject = this.rewardedAdObjectBuffer;

            (this.rewardedAdVideo.GetComponents(typeof(UnityEngine.Video.VideoPlayer))[1] as UnityEngine.Video.VideoPlayer).enabled = true;
            Destroy(this.rewardedAdVideo.GetComponents(typeof(UnityEngine.Video.VideoPlayer))[0] as UnityEngine.Video.VideoPlayer);

            this.rewardedAdVideo.GetComponent<RawImage>().texture = this.rewardedAdRenderBuffer;
            this.rewardedAdVideo.GetComponent<RawImage>().enabled = true;

            this.videoBackground.gameObject.SetActive(true);
            this.videoClose.gameObject.SetActive(true);
            this.videoDelay = 10.0f;

            this.mainScene.SetActive(false);

            StartCoroutine(this.fetchRewarded());

            string link = this.api + "adVisualized?adType=rewarded&publisherCampaignId=" + this.campaignId + "&advertiserCampaignId=" + this.rewardedAdObject.advertiserCampaignId + "&deviceID=" + SystemInfo.deviceUniqueIdentifier;

            yield return UnityWebRequest.Get(link).SendWebRequest();
        }
    }

    private IEnumerator fetchRewarded() {
        string link = this.api + "getAd?adType=rewarded&publisherCampaignId=" + this.campaignId + "&deviceID=" + SystemInfo.deviceUniqueIdentifier;

        UnityWebRequest requestAd = UnityWebRequest.Get(link);

        yield return requestAd.SendWebRequest();

        if(requestAd.result == UnityWebRequest.Result.Success) {
            AdObject responseAd = JsonUtility.FromJson<AdObject>(System.Text.Encoding.ASCII.GetString(requestAd.downloadHandler.data));

            if(new List<string>() { "video/mp4", "video/m4a" }.Contains(responseAd.type)) {
                this.rewardedAdObjectBuffer = responseAd;

                this.rewardedAdVideo.AddComponent<UnityEngine.Video.VideoPlayer>();
                UnityEngine.Video.VideoPlayer videoPlayer = this.rewardedAdVideo.GetComponents(typeof(UnityEngine.Video.VideoPlayer))[1] as UnityEngine.Video.VideoPlayer;

                videoPlayer.renderMode = UnityEngine.Video.VideoRenderMode.RenderTexture;
                videoPlayer.aspectRatio = UnityEngine.Video.VideoAspectRatio.FitInside;
                videoPlayer.url = responseAd.url;
                videoPlayer.Prepare();

                while(!videoPlayer.isPrepared) yield return new WaitForEndOfFrame();

                videoPlayer.enabled = false;

                var canvas = this.rewardedAdVideo.transform.parent.GetComponent<RectTransform>().rect;

                this.rewardedAdRenderBuffer = new RenderTexture((int)(1024 * canvas.width / canvas.height), 1024, 16);
                this.rewardedAdRenderBuffer.Create();

                videoPlayer.targetTexture = this.rewardedAdRenderBuffer;
            } else print("Incorrect Type");
        } else print("Incorrect Datas");
    }

    private IEnumerator clickRewarded() {
        string link = this.api + "adClicked?type=rewarded&publisherCampaignId=" + this.campaignId + "&advertiserCampaignId=" + this.rewardedAdObject.advertiserCampaignId + "&deviceID=" + SystemInfo.deviceUniqueIdentifier;

        yield return UnityWebRequest.Get(link).SendWebRequest();

        Application.OpenURL(this.rewardedAdObject.redirection);
    }

    [System.Serializable]
    private class AdObject {
        public string advertiserCampaignId;
        public string url;
        public string type;
        public string redirection;

        public AdObject() {
            this.advertiserCampaignId = "";
            this.url = "";
            this.type = "";
            this.redirection = "";
        }
    }
}
