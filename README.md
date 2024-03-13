# WorkableTestAssignment

# Introduction

An automation testing framework based on SpecFlow, PlayWright and C#

# Prerequisites & first time Set Up on Windows

*Note*: below guide assumes Rider is used as IDE - you may restart IDE when needed.  
Clone project in o the following folder which (usually) are excluded from malware bytes live search:
`C:\dev`   
Please avoid other directories like `C:\users'

Commands 1-4 below are given in Git Bash, command 5 is given in Powershell:
1. Install nvm:  
`curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.39.5/install.sh | bash`  
`export NVM_DIR="$HOME/.nvm"`  
`[ -s "$NVM_DIR/nvm.sh" ] && \. "$NVM_DIR/nvm.sh"`
2. Install node.js:  
`nvm install --lts`
3. Install Playwright:  
`npx playwright install`  
4. Download & Install [.NET 8.0 SDK x64](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
5. Upgrade Powershell:  
`winget install --id Microsoft.Powershell --source winget`
6. In project Powershell:  
`pwsh .\src\bin\Debug\net8.0\playwright.ps1 install`

# How to

## Run all the end to end tests from terminal

`dotnet test .`

## Run tests from terminal with specific tag

`dotnet test --filter "Category=TAG"`
