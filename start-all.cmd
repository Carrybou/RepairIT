@echo off
rem Lance tous les projets RepairIT (une fenêtre par microservice).
cd /d "%~dp0"

echo Compilation de la solution...
dotnet build RepairIT.sln || exit /b 1

for %%P in (CatalogService UserService RepairService SalesService AuthentificationService Client) do (
    start "RepairIT.%%P" dotnet run --no-build --project src\RepairIT.%%P
)

echo.
echo  Front Blazor         : http://localhost:5000
echo  Catalogue (Swagger)  : http://localhost:5101/swagger
echo  Utilisateurs (Swagger): http://localhost:5102/swagger
echo  Reparations          : http://localhost:5103/health
echo  Ventes               : http://localhost:5104/health
echo  Authentification     : http://localhost:5105/health
echo.
echo Fermez les fenetres ouvertes pour arreter les services.
