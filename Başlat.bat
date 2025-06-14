@echo off
title Keşif Metni Oluşturucu
cd /d "%~dp0"
dotnet run --project KesifUDFGenerator
