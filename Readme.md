# Time Zone Manager Web App
Sample application for demonstration purposes. Showcases Vue.js web application with ASP.NET Core back-end.

Live version available at [https://timezonemanager.azurewebsites.net/](https://timezonemanager.azurewebsites.net/)

![demo](screencast.gif)

# Overview

## Back-end
 - ASP.NET Core 1.1
 - ASP.NET Identity
 - Entity Framework Core
 - JWT authentication & authorization (OpenIdConnect Server)
 - AutoMapper
 - NLog
 - Sample Unit Testing (both controllers and services)
 - Moq

 ## Front-end
  - Vue.js (via vue-loader and webpack)
  - vue-router
  - Vuex
  - vue element-ui
  - axios
  - TypeScript (vue-class-component, vue-property-decorator)


# Run locally

## Prerequisites
 - Visual Studio 2017 (15.2) with ASP.NET Core 1.1
 - NodeJS >=6.x

 ## How to run
 1. Clone the repo   
 2. Open command prompt at `TimeZoneManager\TimeZoneManager\app\`  
 3. Run `npm install`  
 4. Run `npm run build`  
 5. Open project in Visual Studio 2017 - `TimeZoneManager\TimeZoneManager.sln`  
 6. Rebuild the project. 
 7. Hit `Ctrl + F5` to start the app without debugging

 ## Users
  - admin P@ssw0rd
## Roles
 - admin
 - manager
 - user
 