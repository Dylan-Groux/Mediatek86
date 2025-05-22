# 📚 Mediatek86

> Application de gestion d’une médiathèque en C#.

## 📖 Présentation

**Mediatek86** est une application desktop développée en **C#** et basée sur **Windows Forms** (ou **WPF** si applicable) permettant de gérer facilement une médiathèque.  
Elle propose la gestion des abonnés, des livres, des emprunts et des retours, avec une interface utilisateur conviviale.

---

## 📸 Aperçu de l’application

![Aperçu de l'application](./captures/screenshot1.png)

> 📌 Remplacez `./captures/screenshot1.png` par vos propres captures d'écran du projet.

---

## 🖥️ Stack Technique

| Technologie        | Utilisation                        |
|:-----------------:|:----------------------------------|
| **C#**             | Langage principal                  |
| **.NET Framework / .NET** | Framework d'exécution (.NET X.X à préciser) |
| **Windows Forms**  | Interface graphique (GUI)         |
| **SQL Server** (optionnel) | Base de données (si applicable) |
| **ADO.NET**        | Communication avec la base de données |
| **Git** / **GitHub** | Gestion de version et hébergement |

---

## 📂 Architecture du Projet

Mediatek86/
├── bin/ # Fichiers compilés
├── obj/ # Fichiers temporaires
├── Mediatek86.sln # Solution Visual Studio
├── Mediatek86/ # Code source principal
│ ├── Controllers/ # Gestion de la logique applicative
│ ├── Models/ # Classes métiers (Livre, Abonné, etc.)
│ ├── Views/ # Interfaces utilisateur (Forms)
│ └── Program.cs # Point d’entrée de l’application
└── README.md # Fichier de présentation


---

## 🛠️ Installation et Lancement

1. Cloner le projet git clone https://github.com/Dylan-Groux/Mediatek86.git

2. Ouvrir la solution Avec Visual Studio 2022 ou supérieur.

3. Charger le fichier Mediatek86.sln.

4. Configurer la base de données (si nécessaire)

5. Mettre à jour la chaîne de connexion dans le fichier Access.cs.

6. Compiler et exécuter

---

## 📑 FrmMediatekController.cs

Ce contrôleur gère la logique métier de l'application MediaTekDocuments en orchestrant les appels aux services d'accès aux données (`Access`), et en fournissant des méthodes pour récupérer, créer et modifier les ressources.

### 📂 Résumé des fonctionnalités :

- **Récupération des données** :
  - `GetAllGenres()` → Liste des genres.
  - `GetAllLivres()` → Liste des livres.
  - `GetAllDvd()` → Liste des DVD.
  - `GetAllRevues()` → Liste des revues.
  - `GetAllRayons()` → Liste des rayons.
  - `GetAllPublics()` → Liste des publics.
  - `GetExemplairesRevue(string idDocument)` → Liste des exemplaires pour une revue.
  - `GetAllCommandes()` → Liste des commandes.
  - `GetAllSuivi()` → Liste des suivis.
  - `GetAllCommnadesDocuments()` → Liste des commandes-documents.

- **Création** :
  - `CreerExemplaire(Exemplaire exemplaire)` → Créer un exemplaire.
  - `CreerCommande(Commande commande)` → Créer une commande.
  - `CreerSuivi(Suivi suivi)` → Créer un suivi.
  - `CreerCommandeDocument(CommandesDocuments commandedocument)` → Créer une commande-document.

- **Jointure et DTO** :
  - `GetCommandesSuivisDTO()` → Jointure entre commandes et suivis pour un suivi détaillé sous forme de DTO.

- **Récupération spécifique** :
  - `GetCommandeAvecSuivis(string commandeId)` → Récupérer une commande et ses suivis associés.

- **Génération automatique d'ID** :
  - `GenerateCommandeId()` → Générer un nouvel ID commande sous forme `C001`, `C002`...
  - `GenerateSuiviId()` → Générer un nouvel ID suivi sous forme `S001`, `S002`...
  - `GenerateCommandeDocumentId()` → Générer un nouvel ID commande-document sous forme `CD001`, `CD002`...

- **Mise à jour** :
  - `ModifierStatutCommande(string idSuivi, string commandeId, int nouveauStatut)` → Modifier le statut d'une commande via son suivi.

### 📌 Exemple d'utilisation

FrmMediatekController controller = new FrmMediatekController();
var livres = await controller.GetAllLivres();
string newCommandeId = await controller.GenerateCommandeId();

### 📎 Notes
Les méthodes sont asynchrones (async/await) pour améliorer les performances et ne pas bloquer l'interface utilisateur.

Les IDs sont générés automatiquement en fonction des derniers IDs existants en base pour garantir l'unicité et le bon format.

Le contrôleur s'appuie exclusivement sur la couche d'accès aux données Access (singleton).

---
 
### 📄 Documentation Technique — LoginForm.cs
### 📌 Présentation

Ce fichier définit le formulaire de connexion de l’application MediaTekDocuments.
Il permet à l’utilisateur de saisir son identifiant et mot de passe, et valide ces informations via un contrôleur qui effectue une authentification à distance (probablement via une API REST).

### 🖥️ Technologies & Bibliothèques utilisées
Technologie / Lib	Rôle
C# .NET Windows Forms	Gestion de l’interface graphique
HttpClient	Requêtes HTTP (via le contrôleur)
Newtonsoft.Json	Sérialisation/Désérialisation JSON
MediaTekDocuments.controller	Contrôleur métier qui encapsule la logique d’authentification

### 📦 Structure des classes et membres
📌 LoginForm : Form
Élément	Type	Description
controller	FrmMediatekController	Instance du contrôleur principal
LoginForm()	Constructeur	Initialise les composants et le contrôleur
btnLogin_Click(object sender, EventArgs e)	Méthode événementielle async	Déclenchée lors du clic sur le bouton Se connecter

### ⚙️ Fonctionnement détaillé
📍 Constructeur
public LoginForm()
{
    InitializeComponent();
    this.controller = new FrmMediatekController();
}
👉 Initialise le formulaire et crée une instance du contrôleur de l’application.

📍 btnLogin_Click (événement clic sur bouton Login)
csharp
Copier
Modifier
private async void btnLogin_Click(object sender, EventArgs e)
Déroulé :

Validation des champs
Vérifie que les champs txtUsername et txtPassword ne sont pas vides ou composés uniquement d'espaces.

Appel asynchrone au contrôleur

var user = await controller.LoginUtilisateur(username, password);
→ Cette méthode contacte l’API REST (via HttpClient) pour authentifier l’utilisateur.

Traitement du résultat

Si user est non null :

Vérifie la présence de Username et Role

Si valide : stocke le rôle et ferme la fenêtre avec DialogResult.OK

Sinon :

Affiche un message d'erreur et ferme l'application.

### 📝 Exemple de flux d’utilisation
Action utilisateur	Résultat
Remplit les champs + clique sur Se connecter	Lancement d’une requête API
Authentification réussie	Formulaire fermé, retour OK
Identifiants incorrects	Message d’erreur + application fermée

---

### 📄 Documentation Technique — AddCommandeWindows.cs
### 📌 Présentation

Ce fichier gère la fenêtre AddCommandeWindows de l’application MediaTekDocuments.
Elle permet à l'utilisateur d'ajouter une nouvelle commande en spécifiant :

Le montant

Le nombre d'exemplaires

L’identifiant du document à commander

Une fois validée, elle crée et retourne :

Une commande

Un suivi associé à cette commande

Une liaison entre la commande et le document

### 🖥️ Technologies & Bibliothèques utilisées
Technologie / Lib	Rôle
C# .NET Windows Forms	Interface utilisateur
MediaTekDocuments.controller	Contrôleur métier
MediaTekDocuments.model	Modèles Commande, Suivi et CommandesDocuments
MediaTekDocuments.dal	Accès aux données
System.Drawing, System.Data	Gestion des composants graphiques

### 📦 Structure des classes et membres
### 📌 AddCommandeWindows : Form

Élément	Type	Description
idCommande, idSuivi, idCommandeDocument	string	Identifiants récupérés depuis le formulaire principal
CommandeCreee	Commande	Commande créée par l’utilisateur
SuiviCree	Suivi	Suivi de commande associé
LiaisonCreee	CommandesDocuments	Liaison entre commande et document
controller	FrmMediatekController	Contrôleur de gestion

### ⚙️ Fonctionnement détaillé

📍 Constructeur
public AddCommandeWindows(string idCommande, string idSuivi, string idCommandeDocument)
Rôle :

Initialise la fenêtre et récupère les identifiants transmis par le formulaire principal.

Instancie un nouveau contrôleur.

📍 BT_ADD_ONE_COMMANDE_Click (clic sur bouton Ajouter Commande)

private void BT_ADD_ONE_COMMANDE_Click(object sender, EventArgs e)
Déroulé :

Vérification des champs

Contrôle que les champs TB_MONTANT_COMMANDE et TB_ID_LIVRE sont remplis.

Contrôle que TB_MONTANT_COMMANDE et NUMBER_OF_EXEMPLAIRE_FOR_COMMANDE sont bien des entiers.

Création des objets

Crée une nouvelle Commande
new Commande(idCommande, dateCommande, montant);

Crée un Suivi
new Suivi { id_suivi, IdCommande, Status, DateSuivi }

Crée une liaison commande-document
new CommandesDocuments { id_commandedocument, id_document, nbExemplaire, id_commande }

Retour OK et fermeture

Si tout est valide, ferme la fenêtre avec DialogResult.OK

### 📖 Exemple de scénario d’utilisation

Action utilisateur	Résultat
Remplit les champs montant, exemplaire et ID livre	Vérifie et valide
Clique sur Ajouter Commande	Crée les objets et ferme la fenêtre

### 📌 Suggestions d’améliorations

Vérification du format des ID : vérifier la validité des identifiants passés.

Gestion des exceptions : envelopper la logique de création dans un try-catch.

Validation côté DAL : vérifier côté base que le document et la commande existent avant création.

Optimisation UX

Afficher un message de confirmation.

Remettre les champs à zéro après ajout si la fenêtre reste ouverte.

Désactiver le bouton tant que les champs ne sont pas valides.

### 📖 Conclusion

Ce module constitue un élément important de l’application MediaTekDocuments, en assurant l’ajout contrôlé de nouvelles commandes et de leurs suivis documentés.
Il respecte l’architecture MVC :

Vue : AddCommandeWindows

Contrôleur : FrmMediatekController

Modèle : Commande, Suivi, CommandesDocuments
