# CalendrierCours

Application servant à récupérer les informations des séances de cours des différentes cohortes de la formation continue du cégep de ste-Foy afin de les convertir en rendez-vous pour les applications d'agenda (outlook, gmail).

## version 2.0

 - Récupération des cohortes et des séances depuis https://externe5.csfoy.ca/horairecohorte/index.php
 - Création des cours en fonction de l'intitulé, du numéro de cours et de l'enseignant
 - Modification des entités :
    - Dissociation du numéro de cours de et l'intitulé
    - Rajout de catégorie pour l'exportation
    - Rajout de d'une descritpion/commentaire <em>entité et dépot ok - pas dans l'application</em>
 - Possibilité de modifier l'intitulé, les noms et prénoms des enseignants
 - Possibilité d'exporter tous les cours d'une cohorte ou seulement un cours
 - Application winform
 - Exporation au format ICS
 - Optimisation de l'existant
 - Possibilité de récupérer l'ensemble des cohortes
