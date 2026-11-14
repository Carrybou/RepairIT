# RepairIT

**Binôme :** Guibert Thomas / Froger Romeo

Application de gestion pour **RepairIT**, prestataire de vente et de réparation d'ordinateurs : catalogue et stock, clients, réparations, ventes et suivi de l'activité.

Projet réalisé dans le cadre du module *Environnement .NET et C# (ALTN71)* à l'EFREI. L'application est construite en **microservices ASP.NET Core** avec un front **Blazor WebAssembly**.

> **Version actuelle : V1, catalogue informatique**

---

## Sommaire

1. [Architecture](#architecture)
2. [Lancer le projet](#lancer-le-projet)
3. [API disponibles](#api-disponibles)
4. [Modèle de données](#modèle-de-données)
5. [Choix techniques](#choix-techniques)
6. [Tests](#tests)
7. [Organisation Git et rendus](#organisation-git-et-rendus)
8. [Feuille de route](#feuille-de-route)

---

## Architecture

```
RepairIT.sln
├── src/
│   ├── RepairIT.Client                   Front Blazor WebAssembly          :5000
│   ├── RepairIT.CatalogService           API catalogue des ordinateurs     :5101
│   ├── RepairIT.UserService              API gestion des utilisateurs      :5102
│   ├── RepairIT.RepairService            API réparations     (squelette)   :5103
│   ├── RepairIT.SalesService             API ventes          (squelette)   :5104
│   ├── RepairIT.AuthentificationService  API authentification (squelette)  :5105
│   └── RepairIT.Shared                   Modèles et requêtes partagés
└── tests/
    ├── RepairIT.CatalogService.Tests
    ├── RepairIT.UserService.Tests
    └── RepairIT.Shared.Tests
```

Chaque microservice possède **sa propre base de données**. Les services ne partagent que les modèles de `RepairIT.Shared` (contrats d'échange). Les microservices métier sont tous organisés en couches de la même façon :

```
Controllers/    Endpoints HTTP : traduisent les résultats en codes HTTP (200, 201, 404, 409…)
Services/       Règles métier et journalisation (ILogger)
Repositories/   Accès aux données via EF Core
Data/           DbContext et initialisation de la base
Options/        Classes de configuration liées à appsettings.json
Extensions/     Enregistrement des dépendances (injection de dépendances)
```

## Lancer le projet

**Prérequis** : [SDK .NET 10](https://dotnet.microsoft.com/download).

| Système | Commande |
|---|---|
| Windows | double-cliquer sur `start-all.cmd` (une fenêtre par service) |
| macOS / Linux | `./start-all.sh` (`Ctrl+C` arrête tous les services, journaux dans `logs/`) |

Les bases SQLite sont créées et remplies automatiquement au premier démarrage (6 ordinateurs, 1 administrateur et 4 gestionnaires de démonstration). Pour repartir de zéro, il suffit de supprimer les fichiers `*.db` dans `src/RepairIT.CatalogService` et `src/RepairIT.UserService`.

| Service | URL |
|---|---|
| Front | http://localhost:5000 |
| Catalogue (Swagger) | http://localhost:5101/swagger |
| Utilisateurs (Swagger) | http://localhost:5102/swagger |
| Réparations / Ventes / Authentification | http://localhost:5103/health · 5104 · 5105 |

## API disponibles

### Catalogue (`RepairIT.CatalogService`, port 5101)

| Méthode | Route | Description |
|---|---|---|
| `GET` | `/api/ordinateurs` | Lister le catalogue |
| `GET` | `/api/ordinateurs/{id}` | Détail d'un ordinateur |
| `POST` | `/api/ordinateurs` | Ajouter un ordinateur |
| `PUT` | `/api/ordinateurs/{id}` | Modifier un ordinateur |
| `DELETE` | `/api/ordinateurs/{id}` | Supprimer un ordinateur |

### Utilisateurs (`RepairIT.UserService`, port 5102)

| Méthode | Route | Description |
|---|---|---|
| `GET` | `/api/utilisateurs` | Lister les utilisateurs |
| `GET` | `/api/utilisateurs/{id}` | Détail d'un utilisateur |
| `POST` | `/api/utilisateurs` | Créer un utilisateur (`409 Conflict` si l'email existe déjà) |

Les données envoyées sont validées par annotations (`[Required]`, `[Range]`, `[EmailAddress]`…) : une requête invalide reçoit automatiquement une réponse `400 Bad Request` détaillant les erreurs.

## Modèle de données

### Base `catalog.db`, table `Ordinateurs`

| Colonne | Type | Contraintes |
|---|---|---|
| `Id` | int | clé primaire, auto-incrément |
| `Nom` | texte (100) | obligatoire |
| `Marque` | texte (50) | obligatoire, indexée avec `Nom` |
| `Processeur` | texte (100) | |
| `RamGo` | int | 1 à 1024 |
| `StockageGo` | int | 1 à 65536 |
| `Type` | texte (20) | `Portable`, `Fixe`, `ToutEnUn`, `Serveur` |
| `Prix` | décimal (10,2) | 0 à 100 000 |
| `Stock` | int | 0 à 10 000 |
| `DateAjout` | date | renseignée par le serveur |

### Base `users.db`, table `Utilisateurs`

| Colonne | Type | Contraintes |
|---|---|---|
| `Id` | int | clé primaire (identifiant technique) |
| `UserKeycloak` | texte (100) | identifiant Keycloak, valeur par défaut `non-associe` en V1 |
| `Nom` / `Prenom` | texte (50) | obligatoires |
| `Email` | texte (150) | obligatoire, **unique**, stocké en minuscules |
| `Role` | texte (20) | `Gestionnaire` ou `Administrateur` |
| `DateCreation` | date | renseignée par le serveur |

Aucun mot de passe n'est stocké : l'authentification sera entièrement déléguée à Keycloak en V5.

## Choix techniques

- **Un microservice = une base.** Le catalogue et les utilisateurs évoluent indépendamment. Les autres services (réparations, ventes) référenceront les ordinateurs et utilisateurs par leur identifiant.
- **Pattern Repository + Service.** Le service porte la logique métier et dépend d'une interface `I…Repository`. On peut ainsi le tester avec **Moq** sans base de données. Le repository, lui, est testé sur une vraie base SQLite en mémoire.
- **Injection de dépendances.** Toutes les dépendances (DbContext, repositories, services, initialiseur, `TimeProvider`) sont enregistrées dans `Extensions/ServiceCollectionExtensions.cs`. `Program.cs` reste court et lisible. L'horloge est injectée via `TimeProvider`, ce qui rend les dates testables.
- **Configuration.** Chaînes de connexion et `UserKeycloak` par défaut (`Utilisateurs:UserKeycloakParDefaut`) sont lus depuis `appsettings.json` via le *pattern Options* (`IOptions<T>`).
- **Logging.** Les services journalisent chaque opération d'écriture (`Information`) et chaque ressource introuvable (`Warning`), avec des messages structurés (`{Id}`, `{Role}`…).
- **Initialisation de la base.** Au démarrage, `EnsureCreated()` crée le schéma, puis un jeu de données de démonstration est inséré si la base est vide. Les migrations EF Core seront introduites en V2 avec PostgreSQL : des migrations écrites pour SQLite ne seraient pas réutilisables.
- **Enums sérialisés en texte** (`"Portable"` plutôt que `0`) en JSON comme en base, pour des échanges lisibles.
- **Front sur mesure.** Le template Blazor par défaut et Bootstrap sont remplacés par un thème propre (clair/sombre, responsive). En V1, le front est le squelette de navigation (menu du sujet). La consommation des API depuis le front arrive en V4 : en attendant, les API se testent via Swagger.

## Tests

```bash
dotnet test RepairIT.sln --settings coverage.runsettings
```

- **xUnit** pour les tests, **Moq** pour simuler les repositories et les services, **SQLite en mémoire** pour tester les repositories et l'initialisation de la base.
- 29 tests couvrent les services, les repositories, les contrôleurs et l'initialisation des bases.
- Couverture de lignes : **75 %** au global (objectif du module : 50 %). `coverage.runsettings` exclut le code généré (OpenAPI) et les `Program.cs`.
- La CI GitHub Actions (`.github/workflows/ci.yml`) restaure, compile, lance les tests et calcule la couverture à **chaque commit poussé** et à chaque Pull Request vers `production`. Le résumé de couverture s'affiche sur la page de l'exécution et le rapport HTML est téléchargeable en artefact. Aucune étape de déploiement (CD).

## Organisation Git et rendus

- `production` : dernière version rendue et corrigée.
- `main` : intégration du développement en cours.
- Chaque version est livrée par une **Pull Request vers `production`**. La PR centralise les remarques et les notes de l'enseignant.

## Feuille de route

| Version | Contenu | Échéance | État |
|---|---|---|---|
| **V1** | Squelette, CRUD ordinateurs, création des utilisateurs, EF Core + SQLite, tests | 20/11/2026 | ✅ |
| V2 | Clients et réparations, passage à PostgreSQL | 06/12/2026 | |
| V3 | Ventes, tableau de bord (LINQ, async/await) | 20/12/2026 | |
| V4 | Intégration du front Blazor et Gateway | 10/01/2027 | |
| V5 | Keycloak, protection des endpoints, dockerisation | 24/01/2027 | |
