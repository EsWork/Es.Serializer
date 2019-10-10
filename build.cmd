@echo off
set artifacts=%~dp0artifacts

if exist %artifacts%  rd /q /s %artifacts%

call dotnet restore src/Es.Serializer
call dotnet restore src/Es.Serializer.Jil
call dotnet restore src/Es.Serializer.JsonNet
call dotnet restore src/Es.Serializer.NetSerializer
call dotnet restore src/Es.Serializer.ProtoBuf

call dotnet pack -c release src/Es.Serializer  -o %artifacts%
call dotnet pack -c release src/Es.Serializer.Jil  -o %artifacts%
call dotnet pack -c release src/Es.Serializer.JsonNet  -o %artifacts%
call dotnet pack -c release src/Es.Serializer.NetSerializer  -o %artifacts%
call dotnet pack -c release src/Es.Serializer.ProtoBuf  -o %artifacts%

pause
