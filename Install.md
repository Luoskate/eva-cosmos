# EVA VR

![Title and Logo](https://luoskate.github.io/eva-cosmos/title-and-logo.png)

## Description

Avec EVA VR, vous pouvez explorer des galeries d'art virtuelles sans quitter le confort de votre foyer. L'outil vous offre la possibilité de visiter des expositions d'art du monde entier, de découvrir de nouveaux artistes et de vous immerger dans leurs créations.

En tant qu'artiste, EVA VR vous permet de présenter vos œuvres d'une manière totalement nouvelle et captivante. Vous pouvez concevoir votre propre galerie virtuelle, choisir l'emplacement et la disposition des œuvres, et offrir aux visiteurs une expérience artistique interactive. Vous pouvez également organiser des événements virtuels, des vernissages ou des discussions avec les amateurs d'art du monde entier, élargissant ainsi votre portée et votre influence.

Pour les conservateurs et les galeristes, EVA VR offre une plateforme puissante pour présenter des collections d'art de manière immersive. Vous pouvez rassembler des pièces d'art provenant de différents musées et les réunir dans une seule galerie virtuelle, offrant ainsi aux visiteurs une expérience unique de découverte artistique. De plus, grâce à des fonctionnalités avancées telles que la visite guidée virtuelle et les informations détaillées sur les œuvres, vous pouvez enrichir l'expérience des visiteurs en leur fournissant des contextes et des explications approfondies.

Les passionnés d'art peuvent également profiter de l'expérience offerte par EVA VR. Que vous soyez un amateur d'art contemporain, un fan de maîtres classiques ou que vous souhaitiez explorer des styles artistiques variés, EVA VR vous permet de plonger dans un monde virtuel fascinant rempli de chefs-d'œuvre. Vous pouvez interagir avec les œuvres, les examiner de près, en apprendre davantage sur les artistes et leur démarche, et partager vos découvertes avec d'autres passionnés d'art.

En résumé, EVA VR révolutionne la manière dont nous expérimentons l'art en combinant réalité virtuelle et réalité augmentée pour créer des galeries d'art virtuelles immersives. Que vous soyez créateur, conservateur ou amateur d'art, EVA VR vous permet de repousser les limites de la créativité, de la découverte artistique et de l'interaction avec les œuvres.

## Outils / Logiciel

### Moteur de jeu

> Unity (version 2021.3.17)

Unity est un moteur de jeu populaire et puissant utilisé pour le développement de jeux vidéo. La version spécifiée, `2021.3.17`, est une version spécifique du moteur Unity. Il offre un large éventail de fonctionnalités, notamment la création d'environnements 2D et 3D, la gestion des ressources, la physique, l'intelligence artificielle, les animations, etc.

### Système d'exploitation

> Compatible sous (Windows, macOS, Linux).

Ces sont les systèmes d'exploitation couramment utilisés sur les ordinateurs. Unity Game Engine est compatible avec ces trois systèmes d'exploitation, ce qui signifie que vous pouvez l'installer et l'exécuter sur votre ordinateur fonctionnant sous Windows, macOS ou Linux.

### Outils

> Meta Quest developer Hub

Le Meta Quest Developer Hub est une plateforme dédiée aux développeurs pour le casque de réalité virtuelle Oculus Quest. C'est un hub centralisé qui fournit des ressources, des outils et des informations essentielles pour développer des applications et des jeux pour l'Oculus Quest. Il comprend des guides de développement, des didacticiels, une documentation, des forums de discussion et d'autres ressources pour aider les développeurs à créer des expériences immersives pour l'Oculus Quest.

> SideQuest

SideQuest est une plateforme tiers pour les jeux et les applications de réalité virtuelle sur l'Oculus Quest. Il permet aux utilisateurs d'explorer, de télécharger et d'installer des applications non officielles qui ne sont pas disponibles sur l'Oculus Store. SideQuest est souvent utilisé pour découvrir des expériences de réalité virtuelle indépendantes, des démos de jeux, des prototypes et des applications expérimentales créées par des développeurs indépendants. Il offre également des fonctionnalités de gestion et de sauvegarde des applications installées.

## Installation

- Ignorez l'avertissement du mode sans échec : Lorsque vous installez/ manques des packages ou effectuez des modifications dans Unity, il peut parfois afficher un avertissement indiquant que vous êtes en mode sans échec. Cela peut se produire si Unity a rencontré une erreur lors de la dernière session et qu'il s'est ouvert en mode de récupération. Dans ce cas, vous pouvez simplement ignorer cet avertissement et continuer à travailler normalement.

- Installez le package GLTFast à partir d'OpenUPM : Pour installer le package GLTFast, vous devez visiter l'URL suivante : <https://package-installer.glitch.me/v1/installer/OpenUPM/com.atteneder.gltfast?registry=https%3A%2F%2Fpackage.openupm.com&scope=com.atteneder>.

Cette URL vous dirigera vers un installateur de package spécifique à OpenUPM. Suivez les instructions fournies sur la page pour télécharger et installer le package GLTFast dans votre projet Unity.

- Importez le package `com.unity.vectorgraphics` et `com.unity.nuget.newtonsoft-json` :
  Pour importer le package `com.unity.vectorgraphics`, accédez au Gestionnaire de packages dans Unity. Vous pouvez y accéder en sélectionnant "Window" (Fenêtre) dans la barre de menu principale, puis en choisissant "Package Manager" (Gestionnaire de packages). Sélectionnez "Ajouter un package par nom" et entrez `com.unity.vectorgraphics` pour importer le package dans votre projet. Faites de même pour importer le package `com.unity.nuget.newtonsoft-json`.

- Activez la gestion des plugins XR dans les paramètres du projet : Pour activer la gestion des plugins XR, accédez aux paramètres du projet dans Unity. Vous pouvez y accéder en sélectionnant "Edit" (Modifier) dans la barre de menu principale, puis en choisissant "Project Settings" (Paramètres du projet). Dans les paramètres du projet, recherchez la section "XR Plugin Management" (Gestionnaire des plugins XR). Activez cette option pour permettre la gestion des plugins XR dans votre projet.

- Installez le plugin approprié pour votre plateforme cible : Une fois la gestion des plugins XR activée, vous devez installer le plugin approprié pour votre plateforme cible. Par exemple, si vous ciblez la plateforme Android, vous devez installer le plugin XR spécifique à Oculus pour Android. Cela permettra à votre projet Unity de prendre en charge les fonctionnalités de réalité virtuelle sur la plateforme Android.

- Passez à la plateforme Android dans les paramètres de construction : Pour passer à la plateforme Android dans les paramètres de construction, accédez aux paramètres de construction dans Unity. Vous pouvez y accéder en sélectionnant "File" (Fichier) dans la barre de menu principale, puis en choisissant "Build Settings" (Paramètres de construction). Dans les paramètres de construction, sélectionnez la plateforme Android et confirmez votre choix.

- Redémarrez Unity : Après avoir effectué les étapes précédentes, il est recommandé de redémarrer Unity pour appliquer les modifications et les configurations. Vous pouvez fermer Unity complètement et le relancer pour que les changements prennent effet.

## Licence

![Licence MIT](https://img.shields.io/badge/License-MIT-green.svg)

Ce projet est publié sous la licence MIT. La licence MIT est une licence open source permissive qui permet une utilisation libre, la modification et la distribution du code source, sous réserve de conserver l'avis de copyright et la clause de non-responsabilité dans les copies du logiciel. Elle offre une grande flexibilité aux développeurs et aux utilisateurs du logiciel pour l'utiliser, le modifier et le distribuer selon leurs besoins.

## Conclusion

En conclusion, EVA VR est une plateforme immersive qui révolutionne l'expérience artistique en combinant réalité virtuelle et réalité augmentée pour créer des galeries d'art virtuelles captivantes. Que vous soyez un artiste, un conservateur, un galeriste ou un passionné d'art, EVA VR offre des possibilités infinies pour explorer, créer et interagir avec l'art.

Pour les artistes, EVA VR offre une nouvelle manière de présenter leurs œuvres, en leur permettant de concevoir leurs propres galeries virtuelles et d'offrir des expériences interactives uniques aux visiteurs. Les conservateurs et les galeristes peuvent utiliser la plateforme pour rassembler des collections d'art provenant de différents musées et les présenter de manière immersive, offrant ainsi aux visiteurs une expérience de découverte artistique inédite.

Les passionnés d'art bénéficient également de cette plateforme, qui leur permet de plonger dans un monde virtuel fascinant rempli de chefs-d'œuvre. Ils peuvent interagir avec les œuvres, en apprendre davantage sur les artistes et partager leurs découvertes avec d'autres amateurs d'art du monde entier.

En utilisant Unity comme moteur de jeu, EVA VR offre des fonctionnalités avancées pour créer des environnements 2D et 3D, gérer les ressources, ajouter des interactions et des animations, et bien plus encore. L'utilisation du Meta Quest Developer Hub et de SideQuest permet aux développeurs d'accéder à des ressources supplémentaires et d'explorer de nouvelles possibilités pour leurs créations.

En résumé, EVA VR repousse les limites de la créativité, de la découverte artistique et de l'interaction avec les œuvres. Grâce à cette plateforme, chacun peut vivre une expérience artistique immersive sans quitter le confort de son foyer. Que vous souhaitiez explorer des galeries d'art du monde entier, présenter vos propres créations ou simplement vous plonger dans l'univers artistique, EVA VR offre une expérience unique et passionnante.

## Suivant

- **[Avant de Démarrer](https://luoskate.github.io/eva-cosmos/BeforeStarting.html)**
- **[Fonctionnalités](https://luoskate.github.io/eva-cosmos/features.html)**
