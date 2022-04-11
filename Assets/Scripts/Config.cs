using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Config : MonoBehaviour
{
    public List<string> bannerAdTypes = new List<string>() { "image/png", "image/jpg", "image/jpeg", "image/webp" };
    public List<string> interstitialAdTypes = new List<string>() { "video/mp4", "video/m4a" };
    public List<string> rewardedAdTypes = new List<string>() { "video/mp4", "video/m4a" };
}
