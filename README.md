# AR_Lucie_Thomas

## Description

Ce projet est composé de trois exécutables ayant tous un même sujet : utiliser la kinect. Le premier projet est une application WPF nommée FirstKinectProjet qui permet en ayant branché une kinect de voir les différents flux possibles. Le deuxième est une application console qui permet de reconnaître des postures et des gestes, tels qu'avoir la main droit levée ou faire un balayage de la main droite. Et la dernière est un jeu basé sur un mini jeu de wii sport resort: "speed slice" (https://www.ign.com/wikis/wii-sports-resort/Speed_Slice).


## Application WPF

Pour lancer l'application WPF, vous devez choisir comme projet de démarrage le projet FirstKinectProjet. Suite à ça, une fenêtre apparaîtra avec une ellipse indiquant si la kinect est connectée. De plus, vous pourrez voir différents boutons représentant les différents type de flux vidéos. Nous avons implémenté tous les flux vidéos demandés, c'est-à-dire le ColorStream, le DepthStream, l'IRStream, le BodyStream et le BodyColorStream. Pour faire la liaison entre le XAML et les classes de notre modèle, nous avons utilisé le MVVM toolkit.

## Application Console

Pour lancer l'application Console, vous devez choisir comme projet de démarrage le projet ConsoleApp. Ensuite, une invite de commande apparaîtra et affichera le nom de la posture ou du mouvement que la kinect a reconnu. L'application peut reconnaître lorsque l'utilisateur a la main droite lever au-dessus de la tête, quand il a les mains jointes et quand il a les bras croisés en X. Elle peut aussi reconnaître les gestes, tels qu'un balayage sur la gauche avec la main droite, un balayage sur la droite avec la main gauche, un coupé en diagonale en partant au-dessus à droite de la tête et en arrivant à gauche de la hanche avec la main droite (https://www.youtube.com/watch?v=xEwfYR8ihEA) et la même chose symétriquement avec la main gauche.

## Jeu 

Pour lancer le jeu, vous devez choisir comme projet de démarrage le projet FruitNinja. Après ça, vous verrez une fenêtre apparaître vous demandant de joindre vos mains afin de lancer le jeu. Cela vous emmènera sur une nouvelle page ou vous verrez des objets apparaître avec une flèche indiquant le sens de la coupe à effectuer. Lorsque le geste correspond avec la flèche, un nouvel objet apparaîtra à la place et vous marquerez un point. L'objectif est donc de couper le plus d'objets en le moins de temps possible.