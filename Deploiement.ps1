# Script de deploiement de CalendrierCours
# Version 1.0
# Simon Quillaud
# Decembre 2022

$chemin= Get-Location

dotnet restore $chemin\CalendrierCours.sln
dotnet build $chemin\CalendrierCours.sln --configuration Release

dotnet publish $chemin\CalendrierCours.ConsoleUI --configuration Release --output $chemin\CalendrierCours.Publish