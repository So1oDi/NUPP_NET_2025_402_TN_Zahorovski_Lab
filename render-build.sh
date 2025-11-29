#!/bin/bash
set -o errexit

# Install .NET 8
curl -sSL https://dot.net/v1/dotnet-install.sh | bash /dev/stdin --version 8.0.100

# Build the project
dotnet restore
dotnet publish -c Release -o publish