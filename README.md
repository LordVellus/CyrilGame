# CyrilGame

# New game project CLI

dotnet new classlib --output CyrilGame.Pong -f net6.0
dotnet sln add CyrilGame.Pong
dotnet add CyrilGame/CyrilGame reference CyrilGame.Pong

-- Progmatically delete Class1.cs, and then copy default files to project folder.