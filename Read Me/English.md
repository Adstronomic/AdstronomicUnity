## [**🇫🇷 VERSION FRANÇAISE DISPONIBLE 🇫🇷**](https://flrn.gitbook.io/adstronomic/adstronomic-unity/adstronomic-guide-dinstallation-unity)



# **Adstronomic - Installation Guide (Unity)**



## <u>1 - Introduction</u>



Adstronomic is an advertising platform that allows you to leverage and synthesize your users' data to deliver the most relevant advertising to them. By taking into account the specificities of each game and its users, coupled with a revolutionary AI, we can identify the most relevant ads to increase your game's revenues. To achieve this, Adstronomic provides you with three key tools :

	A web-based platform where you can set up your projects and associated ads.
	An API that allows you to interact with Adstronomic's data.
	An SDK that allows you to quickly and easily use the full potential of Adstronomic

Each of these tools is intuitive, so you can focus on what matters most to you : The success of your project. In this guide, we will focus on the third point: Installing and using the SDK, here in its Unity version. 📱

Please note that there are two ways to use Adstronomic : If you are starting a new project, we invite you to clone this repository directly, and use it as a working base. This way you can skip the section "2 - Installing Adstronomic". However, if your project is already well advanced, you may prefer to manually install the SDK to your existing project. In that case, the next section is for you !



## <u>2 - Installing Adstronomic</u>



As a Unity developer, you are probably familiar with Unity Hub, the Unity project manager. So let's launch it, and create a new project.

Click on "New Project", select 3D, and specify the name and location of your new project. The other settings don't matter, and you should then arrive on the main scene of your new project.

![Adstronomic Unity 1](https://raw.githubusercontent.com/Adstronomic/AdstronomicUnity/master/Read%20Me/1.png)

Since you are starting with an empty project, you will need to add the Adstronomic SDK. To do this, start [by downloading it.](https://drive.google.com/file/d/1uyfIK3e0OYvjfuqj-E7394Ws18ULzLzW/view?usp=sharing) Then, make sure you are in the Project tab of your project, and right click, then select Import Package > Custom Package, and specify the file you just downloaded.

![Adstronomic Unity 1](https://raw.githubusercontent.com/Adstronomic/AdstronomicUnity/master/Read%20Me/2.png)

Note that by default the Adstronomic SDK will be installed in a Scripts folder, but you can move it if needed.

Congratulations ! You've just added Adstronomic to your project ! 🥳 The next step is now to configure it !



## <u>3 - Adstronomic Configuration</u>



Now that Adstronomic is integrated into your project, we'll look at how to set it up to retrieve and send the right data.

Every Unity project runs on scripts, and you're bound to have one that controls the flow of your game. If you don't have one, you can create one, and attach it to an object in your scene. In my case, I will create a GameController script, which I will attach to my MainCamera, and add the Adstronomic script to it as well.

![Adstronomic Unity 1](https://raw.githubusercontent.com/Adstronomic/AdstronomicUnity/master/Read%20Me/3.png)

Then open the GameController script. To use Adstronomic, we'll start by adding an attribute of type Adstronomic, via the following line :

```cpp
private Adstronomic myAdstronomic;
```

Next, we need to get the Adstronomic SDK, and inject it into this attribute. Since it is attached to the same GameObject as this script, we can retrieve it in the Start function, like this :

```cpp
this.myAdstronomic = this.gameObject.GetComponent<Adstronomic>();
```

If your Adstronomic script is linked to another GameObject, you can retrieve it in the same way, but adapting the parent object.

To initialize Adstronomic, we will then use the initialize function :

```cpp
this.myAdstronomic.initialize("SMSdtSBHg8PAvPq7PmC1");
```

🚨 Please note that the parameter corresponds to the ID of your campaign, as indicated on the web platform. The campaign ID shown here is for a demo account, which you can use to check how Adstronomic works, but which should definitely not be deployed in production !

Please note that this campaign ID is specific to each platform (Android and iOS) and that you will have to use a second one if you deploy your application on a second platform.

This will allow Adstronomic to connect to the API correctly, and will automatically load and cache an ad of each type (Banner Ad, Interstitial Ad and Rewarded Ad) to speed up its display later on.



## <u>4 - Loading Ads</u>



With Adstronomic, the different ads are displayed in a Canvas, so they are always visible to the player.

The Canvas, the Event System, and the video display are managed directly by Adstronomic. To display an ad, you just have to call one of the following functions :

```cpp
StartCoroutine(this.myAdstronomic.loadBanner());
StartCoroutine(this.myAdstronomic.loadInterstitial());
StartCoroutine(this.myAdstronomic.loadRewarded());
```

However, please note two important points :

First, since these functions are asynchronous, (Especially for calls to the Adstronomic API) you must call them via a coroutine. Calling one of these functions directly will not display any ads.

Also, the ads are not loaded directly when calling these functions, but via a cache initialized in the this.myAdstronomic.initialize function we used above. This means that you have to wait a few seconds between calling this initialization function and displaying your first video.

Finally, you will probably need to pause your scene during video ads. For this, the Adstronomic.cs script has a "Main Scene" attribute, which will be paused during video ads, and resumed once they are closed. You can use it and link it to your scene if needed, but it remains optional.



What's the next step ? Well, we're already done !

All other events are handled inside the Adstronomic.cs script. Each time the ad is displayed, a second ad is preloaded, but this is completely transparent to you. Similarly, the ads are configured to be clickable, and redirect to the download page of the application in question, but this is also invisible to you.

All you have to do is call the three functions presented above, and let the magic of Adstronomic present your game to the rest of the world ! 😎



## <u>5 - Conclusions and Resources</u>



Through this guide, we have installed, initialized and used Adstronomic's Unity SDK. Of course, this SDK integrates with the rest of the Adstronomic ecosystem, and I strongly encourage you to learn more about it.

To do so, here are a few links that will be useful to you :

	Website: https://www.adstronomic.com
	Campaign Manager: https://app.adstronomic.com
	Our TOS: https://terms.adstronomic.com
	Our GitHub: https://www.github.com/adstronomic
	Our Discord: https://discord.gg/Uz5EMFpWuU

The last step for you is to sign up for our campaign manager, create a campaign, add your ads, and specify your campaignId when the application loads.

Finally, Adstronomic thrives on our users, and it is our duty to do our best for them. If you have any comments, questions, or suggestions, we invite you to send us a feedback via our contact page or our Discord.

See you soon,

The Adstronomic Team ✌️