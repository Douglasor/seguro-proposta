#!/bin/bash
set -e

# Executando migrations...
dotnet ef database update --no-build

# Iniciando aplicação...
exec dotnet ContratacaoService.dll

