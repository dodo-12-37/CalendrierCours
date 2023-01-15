# Script de deploiement de CalendrierCours V2.0
# Version 2.0
# Simon Quillaud
# Janvier 2023

$chemin= Get-Location

dotnet restore $chemin\CalendrierCours.sln
dotnet build $chemin\CalendrierCours.sln --configuration Release

dotnet publish $chemin\CalendrierCours.WinFormUI --configuration Release --output $chemin\CalendrierCours.Publish