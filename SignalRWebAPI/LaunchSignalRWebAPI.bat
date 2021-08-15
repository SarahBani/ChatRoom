@echo Launch SignalRWebAPI

title  SignalR Web API
dotnet build --configuration "Debug"
set ASPNETCORE_ENVIRONMENT=Development
dotnet run --configuration "Debug" 
