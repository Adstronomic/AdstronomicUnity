## [**🇺🇸 ENGLISH VERSION AVAILABLE 🇺🇸**](https://flrn.gitbook.io/adstronomic/adstronomic-unity/adstronomic-installation-guide-unity)



# **Adstronomic - Guide d'Installation (Unity)**



## <u>1 - Introduction</u>



Adstronomic est une plateforme publicitaire vous permettant d'exploiter et de synthétiser les données de vos utilisateurs, afin de leur proposer la publicité la plus adaptée à leurs besoins. En nous appuyant sur les spécificités de chaque jeu et de ses utilisateurs, couplé à une IA révolutionnaire, nous parvenons à identifier les publicités les plus pertinentes, afin d'augmenter les revenus de votre jeu. Pour y parvenir, Adstronomic met à votre disposition trois outils clé :

	Une plateforme web sur laquelle vous pouvez paramètrer vos projets, et les publicités associées.
	Une API permettant d'interagir avec les données d'Adstronomic.
	 Un SDK qui vous permet d'utiliser facilement et rapidement tout le potentiel d'Adstronomic

Chacun de ces outil est intuitif, afin de vous permettre de vous concentrer sur ce qui compte le plus pour vous : La réussite de votre projet. Dans ce guide, nous allons nous concentrer sur le troisième point : L'installation et l'utilisation du SDK, ici dans sa version Unity. 📱

Veuillez noter que deux solutions s'offrent à vous pour utiliser Adstronomic : Si vous commencez un nouveau projet, nous vous invitons à clôner directement ce dépôt, et à l'utiliser comme base de travail. Vous pourrez ainsi sauter la section "2 - Installation d'Adstronomic". Toutefois, si votre projet est déjà bien avancé, vous préfererez sûrement installer manuellement le SDK à votre projet existant. Dans ce cas là, la section suivante est faite pour vous !



## <u>2 - Installation d'Adstronomic</u>



En tant que développeur Unity, vous êtes sûrement famillier avec Unity Hub, le gestionnaire de projets Unity. Nous allons donc le lancer, et créer un nouveau projet.

Cliquez sur "New Project", sélectionnez 3D, et indiquez le nom et l'emplacement de votre nouveau projet. Les autres réglages n'ayant pas d'importance, vous devriez ensuite arriver sur la scène principale de votre nouveau projet.

![Adstronomic Unity 1](https://raw.githubusercontent.com/Adstronomic/AdstronomicUnity/master/Read%20Me/1.png)

Comme vous partez d'un projet vide, vous allez devoir ajouter le SDK Adstronomic. Pour cela, commençez [par le télécharger.](https://drive.google.com/file/d/1uyfIK3e0OYvjfuqj-E7394Ws18ULzLzW/view?usp=sharing)  Ensuite, assurez vous d'être dans l'onglet Project de votre projet, et faîtes un clic droit, puis sélectionnez Import Package > Custom Package, et indiquez le fichier que vous venez de télécharger.

![Adstronomic Unity 1](https://raw.githubusercontent.com/Adstronomic/AdstronomicUnity/master/Read%20Me/2.png)

Notez que par défaut le SDK d'Adstronomic va s'installer dans un dossier Scripts, mais vous pouvez le déplacer si besoin.

Félicitations ! Vous venez d'ajouter Adstronomic à votre projet ! 🥳 La prochaine étape est maintenant de le configurer !



## <u>3 - Configuration d'Adstronomic</u>



Maintenant qu'Adstronomic est intégré à votre projet, nous allons voir comment le paramétrer pour qu'il récupère et envoie les bonnes données.

Tout projet Unity fonctionne via des scripts, et vous en aurez forcément un qui va contrôler le déroulement de votre jeu. Si vous n'en avez pas, vous pouvez en créer un, et l'attacher à un objet de votre scène. Dans mon cas, je vais créer un script GameController, que je vais attacher à ma MainCamera, et ajoutez lui également le script Adstronomic.

![Adstronomic Unity 1](https://raw.githubusercontent.com/Adstronomic/AdstronomicUnity/master/Read%20Me/3.png)

Ouvrez ensuite le script GameController. Pour utiliser Adstronomic, nous allons commencer par ajouter un attribut de type Adstronomic, via la ligne suivante :

```cpp
private Adstronomic myAdstronomic;
```

Ensuite, nous devons récupérer le SDK Adstronomic, et l'injecter dans cet attribut. Comme il est attaché au même GameObject que ce script, nous pouvons le récupérer dans la fonction Start, comme ceci :

```cpp
this.myAdstronomic = this.gameObject.GetComponent<Adstronomic>();
```

Si votre script Adstronomic est lié à un autre GameObject, vous pouvez le récupérer de la même manière, mais en adaptant l'objet parent.

Pour initialiser Adstronomic, nous allons ensuite utiliser la fonction initialize :

```cpp
this.myAdstronomic.initialize("SMSdtSBHg8PAvPq7PmC1");
```

🚨 Notez bien que le paramètre correspond à l'identifiant de votre campagne, tel qu'indiqué sur la plateforme web. L'identifiant indiqué ici corresponds à un compte de démonstration, qui peut vous servir à vérifier le bon fonctionnement d'Adstronomic, mais qui ne doit absoluement pas être déployé en prodution !

Notez également que cet identifiant de campagne est propre à chaque plateforme (Android et iOS) et que vous devrez en utiliser un second si vous déployez votre application sur une seconde plateforme.

Cette fonction va permettre à Adstronomic de se connecter correctement à l'API, et va automatiquement charger et mettre en cache une publicité de chaque type (Banner Ad, Interstitial Ad et Rewarded Ad) pour accélérer son affichage par la suite.



## <u>4 - Chargement des Publicités</u>



Avec Adstronomic, les différentes publicités sont affichés dans un Canvas, afin d'être toujours visible par le joueur.

La gestion du Canvas, de l'Event System, et de l'affichage des vidéos est géré directement par Adstronomic. Pour afficher une publicité, il vous suffit simplement d'appeler l'une des fonction suivante :

```cpp
StartCoroutine(this.myAdstronomic.loadBanner());
StartCoroutine(this.myAdstronomic.loadInterstitial());
StartCoroutine(this.myAdstronomic.loadRewarded());
```

Toutefois, veuillez noter deux points importants :

Tout d'abord, ces fonctions étant asynchrones, (Notamment pour les appels vers l'API Adstronomic) vous devez les appeler via une coroutine. Appeler l'une de ces fonction directement n'affichera aucune publicité.

De plus, les publicités ne sont pas chargées directement à l'appel de ces fonctions, mais via un cache initialisé dans la fonction this.myAdstronomic.initialize que nous avons utilisé plus haut. Cela implique que vous devez attendre quelques secondes entre l'appel de cette fonction d'initialisation et l'affichage de votre première vidéo.

Enfin, vous aurez sûrement besoin de mettre votre scène en pause lors des publicités vidéo. Pour cela, le script Adstronomic.cs possède un attribut "Main Scene", qui sera mis en pause lors des publicités vidéos, et repris une fois leur fermeture. Vous pouvez donc l'utiliser et le relier à votre scène si besoin, mais il reste facultatif.



Quelle est la prochaine étape ? Et bien nous avons déjà terminé !

Tous les autres évènements sont gérés à l'intérieur du script Adstronomic.cs. À chaque affichage de la publicité, une seconde publicité est préchargée, mais cela est complètement transparent pour vous. De la même façon, les publicités sont configurées pour être cliquable, et rediriger vers la page de téléchargement de l'application en question, mais cela est également invisible pour vous.

Tout ce que vous avez à faire est d'appeler les trois fonctions présentées au dessus, et laisser la magie d'Adstronomic présenter votre jeu au reste du monde ! 😎



## <u>5 - Conclusions et Resources</u>



Au travers de ce guide, nous avons installé, initialisé et utilisé le SDK Unity d'Adstronomic. Bien sûr, ce SDK s'intègre au reste de l'écosystème Adstronomic, et je vous invite fortement à en apprendre davantage.

Pour cela, voici quelques liens qui vous seront utiles :

	Site Web : https://www.adstronomic.com
	Gestionnaire de Campagnes : https://app.adstronomic.com
	Nos CGU : https://terms.adstronomic.com
	Notre GitHub : https://www.github.com/adstronomic
	Notre Discord : https://discord.gg/Uz5EMFpWuU

La dernière étape pour vous est de vous inscrire sur notre gestionnaire de campagnes, d'y créer une campagne, y ajouter vos publicités, et indiquer votre campaignId au chargement de l'application.

Enfin, Adstronomic prospère grâce à nos utilisateurs, et nous nous devons de faire le maximum pour eux. Si vous avez la moindre remarque, question, ou suggestion à nous faire part, nous vous invitons à nous faire un retour via notre page de contact, ou notre Discord.

À très vite,

L'Équipe Adstronomic ✌️