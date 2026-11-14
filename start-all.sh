#!/usr/bin/env bash
# Lance tous les projets RepairIT (macOS / Linux). Ctrl+C arrête l'ensemble des services.
set -euo pipefail
cd "$(dirname "$0")"

PROJETS=(CatalogService UserService RepairService SalesService AuthentificationService Client)

echo "Compilation de la solution..."
dotnet build RepairIT.sln

mkdir -p logs
pids=()
for projet in "${PROJETS[@]}"; do
    dotnet run --no-build --project "src/RepairIT.$projet" > "logs/$projet.log" 2>&1 &
    pids+=($!)
    echo "RepairIT.$projet démarré (journal : logs/$projet.log)"
done

trap 'echo; echo "Arrêt des services..."; kill "${pids[@]}" 2>/dev/null' INT TERM

cat <<INFO

  Front Blazor           : http://localhost:5000
  Catalogue (Swagger)    : http://localhost:5101/swagger
  Utilisateurs (Swagger) : http://localhost:5102/swagger
  Réparations            : http://localhost:5103/health
  Ventes                 : http://localhost:5104/health
  Authentification       : http://localhost:5105/health

Ctrl+C pour tout arrêter.
INFO

wait
