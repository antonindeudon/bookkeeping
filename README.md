WARNING DO NOT SHARE BECAUSE IT HAS MY GOOGLE API CREDENTIALS & IDENTIFIANTS IN SOME COMMITS

#Configurer l'application

##Prérequis
Avoir un compte Google. Vous pouvez en créer un gratuitement.

##Création du tableur
1. Accédez au modèle : https://docs.google.com/spreadsheets/d/1z_C62pSJzeZUJbcnKUp7ii37ZnUCZWbHW7teQD9OGDQ/edit?usp=sharing
2. Fichier -> Créer une copie
3. Entrez un nom pour votre tableur et validez.
4. Le tableur se compose de deux feuilles. Commencez par ajouter quelques catégories, puis vous pourrez saisir des entrées.
5. Gardez le tableur ouvert pour la suite.

##Configuration Google Cloud
1. Si vous n'avez pas de compte google, créez-en un gratuitement.
2. Rendez-vous sur https://console.cloud.google.com
3. Si on vous propose de payer ou d'essayer gratuitement, vous pouvez cliquer sur ignorer.
4. En haut à gauche, cliquez sur Sélectionner un projet puis créez-en un nouveau.
5. Donnez-lui un nom. Vous pouvez laisser la zone par défaut.
6. Sélectionnez maintenant ce projet.
7. À gauche, choisissez IAM et administration -> Comptes de service
8. Créez un compte de service.
9. Entrez un nom, puis cliquez sur OK.
10. Le compte de service est créé. Copiez l'email.
11. Dans votre tableur, cliquez sur Partager.
12. Collez l'email, choisissez Editeur, puis cliquez sur Envoyer.
13. Revenez sur Google Cloud et cliquez sur l'email.
14. Allez dans l'onglet CLÉS.
15. Ajoutez une clé, choisissez le type JSON.
16. Enregistrez le fichier et rangez-le en lieu sur.
17. Dans la barre de recherche, chercher Google Sheets API, cliquer sur Google Sheets API.
18. Cliquer sur Activer (si c'est écrit Gérer, c'est que vous l'aviez déjà activé auparavant)

##Paramètres de connexion
1. Ouvrez l'application de comptes. Vous serez invité à renseigner des paramètres de connexion.
2. Sur la page de votre tableur, copier la partie de l'URL située entre les deux derniers '/'
3. Collez ce texte dans le champ Spreadsheet ID
4. Dans le champ Credentials file path, indiquez le chemin d'accès au fichier téléchargé à l'onglet 16 de la configuration Google Cloud.
5. Ne touchez pas aux deux autres champs.
6. Validez et la connexion devrait fonctionner.
