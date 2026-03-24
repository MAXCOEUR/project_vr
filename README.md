
## Réalisé par :
## Sacha (Création des modèles 3D et page d'accueil, interface de jeu et tcheckup archi)
## Maxence (Interactions du menu, de la preview et merge, bug fix, logique de jeu et gestion des ressources)
## Gael (Gestion des animations et brainstorming, logique de jeu et gestion des ressources)
## Amaury (Gestion de la partie "Level up", système d'occlusion, vérification du rendu et readme)

Le projet "Arc Builder Clicker" est un projet AR qui a pour but de faire apparaître un village, composé d'une maison, d'un arbre et d'un rocher. Le joueur pourra cliquer sur l'arbre pour récupérer du bois et sur la pierre pour ensuite améliorer la maison. Il y a en tout 3 niveaux de maisons dans le jeu, le niveau 1, celui de base, et les deux disponibles après améliorations.<br/>
Le joueur est limité qu'à un arbre et qu'une roche, et une seule maison. Ce dernier peut poser les structures dans l'ordre de son choix. 
En cliquant sur la maison, si il n'est pas en mesure de l'améliorer, il fera spawn un villagois. Un villageois va ensuite se rendre vers la pierre ou le bois, et récupérer toutes les x secondes des matériaux. Il n'y a pas de limitation au nombre de villagoies qui peuvent spawn, mais à l'instant où le joueur a un minimum de 3 villagoies, un ours apparaîtra pour les manger. En cliquant sur l'ours, il mourra et pendant x secondes, ce dernier ne sera plus là.

##L'application vient avec un système d'occlusion compatible iphone et android permettant de cacher les éléments virtuels dans l'environnement

## Les 2 niveaux supérieurs pour la maison demandent :

### Level 2 :
10 bois
5 roches

### Level 3:
20 bois
15 roches

Nous avions choisi de partir sur la AR plutôt que la VR, car plus tard nous serons probablement plus amenés à travailler avec cette technologie que la VR, mais aussi car cette dernière nous inspire plus sur les possibilités.

# Difficultés rencontrées :
Gestion des scripts et interactions dans Unity <br/>
Init du projet propre <br/>
Gestion des animations <br/>
Gestion des merge, push, conflict etc... <br/>
