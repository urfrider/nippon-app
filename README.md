# Team Project [CSC2002, AY 22/23] - Qualynatics [Team 15]

<details>
	<summary>Table of Contents</summary>
	<ol>
		<li><a href="#about-the-project">About The Project</a></li>
        <li><a href="#project-architecture">Project Architecture</a></li>
		<li><a href="#team-structure-and-overview">Team Structure and Overview</a></li>
		<li><a href="#team-communications">Team Communications</a></li>
		<li>
			<a href="#getting-started">Getting Started</a>
			<ul>
				<li><a href="#prerequisites">Prerequisites</a></li>
				<li><a href="#installation">Installation</a></li>
			</ul>
		</li>
        <li>
            <a href="#swagger-documentation">Swagger Documentation</a>
            <ul>
				<li><a href="#login-credentials">Login Credentials</a></li>
				<li><a href="#available-apis">Available APIs</a></li>
			</ul>
        </li>
	</ol>
</details>

<hr>

## About The Project
**Project Title**<br> Activity Survey Application for Smart City Planning<br><br>
**Project Client**<br> Nippon Koei<br>

**Project Description**<br>
The goal of this project aims to develop a location data driven surveying application utilizing Gobal Positioning System (GPS) of end users' devices to better improve data analytics and surveying processes to provide greater insights or knowledge for traffic of specific areas to allow for better informed decisions to be made regarding smart city planning.  

This project will be handled by two teams, GUI Dance (Team 7) and Qualynatics (Team 15), to develop the front end mobile client and back end API services respectively.

Qualynatics (Team 15) will be focused on developing a web Application Programming Interface (API) driven infractructure to provide logic processing and data storage which will serve as a back end service for the project. The scope also includes the development of a web-based management dashbaord to provide means of access, data management and control of the application for the client.

Futher more in collaboration with GUI Dance (Team 7), both teams will work together to ensure a smooth and successful implementation of API services between the back end and front end infrastructures. 

<hr>

## Project Architecture

### Application
![architecture-screenshot](https://github.com/UofG-CS/project-TP-group15/blob/development/images/Architecture.png?raw=true)

### Database
![database-screenshot](https://github.com/UofG-CS/project-TP-group15/blob/development/images/DB_Design.png?raw=true)

<hr>

## Team Structure and Overview

**Mission**<br> Inspiring Technology for All.<br><br>
**Vision**<br> Aiming to deliver quality, unique and user-friendly technology to every sector.

|    | Team Members | Role(s) | Email |
| - | - | - | - | 
| **01** | Amiir Hamzah Bin Bakri | Database Architect, Dev Ops | 2101526@sit.singaporetech.edu.sg |
| **02** | Kim Beomjun | Lead Front End Developer | 2101249@sit.singaporetech.edu.sg |
| **03** | Koh Ding Yuan | Lead Back End Developer | 2100609@sit.singaporetech.edu.sg |
| **04** | Lau Jun Xiang | Team Leader, Scrum Master, Back End Developer | 2100582@sit.singaporetech.edu.sg |
| **05** | Roshan Dew | Product Owner | 2100666@sit.singaporetech.edu.sg |

<hr>

## Team Communications

### Internal Communications
**Daily**<br>Telegram Group Chat<br><br>
**Weekly**<br>Physical Meetups, Discord Voice Calls

### External Communications (Clients & Advisors)
Email and Online Meeting (Microsoft Teams)

<hr>

## Getting Started

### Prerequisites
Ensure the following are installed on your system:
- .Net 6.0
- Visual Studio (Recommended 2022 or above)
- Nodejs
- MS SQL Database Engine
- SQL Server Management Studio (SSMS)

## Installation

1. Pull the project from github:<br>
`https://github.com/UofG-CS/project-TP-group15.git`

2. Open a command console from the following folder:<br>
`project-TP-group15/ActivitySurveyAppForSmartCityPlanning/ActivitySurveyAppForSmartCityPlanning/ClientApp/`

3. Within the command console, run the following command to install the necessary node js packages for the dashboard UI:<br>
`npm install`

4. Open your SQL Server Management Studio (SSMS)

5. Open the following SQL query files in your SSMS<br>
`project-TP-group15/database/SQL_Scripts.sql`<br>
`project-TP-group15/database/Public_Transport.sql`

6. Run all the queries to generate the tables required for the application
   
7. Open the project's solution file with Visual Studio<br>
`project-TP-group15/ActivitySurveyAppForSmartCityPlanning/ActivitySurveyAppForSmartCityPlanning.sln`

8. Open the NuGet package manager within Visual Studio

9. Ensure following packages are installed:

| NuGet Package Name | Version |
| - | - |
| Swashbuckle.AspNetCore Version | >= 6.4.0 |
| Swashbuckle.AspNetCore.Swagger | >= 6.4.0 |
| Swashbuckle.AspNetCore.SwaggerGen | >= 6.4.0 |
| Swashbuckle.AspNetCore.SwaggerUI | >= 6.4.0 |
| Swashbuckle.AspNetCore.Filters | >= 7.05 |
| Microsoft.AspNetCore.Authentication.JwtBearer | >= 6.0.10 |
| Microsoft.AspNetCore.SpaProxy | >= 6.0.9 |
| Microsoft.EntityFrameworkCore | >= 7.0.0 |
| Microsoft.EntityFrameworkCore.Tools | >= 7.0.0 |
| Microsoft.EntityFrameworkCore.SqlServer | >= 7.0.0 |
| System.IdentityModel.Tokens.Jwt | >= 6.24.0 |
| BCrypt.Net-Next | >= 4.03 |
| CsvHelper | >= 30.0.1 |

10. Open the appsettings.json at the following path within Visual Studio<br>
`project-TP-group15/ActivitySurveyAppForSmartCityPlanning/appsettings.json`

11.  Change the settings under `DBConfig` section to your database's configuration
    
12.  Rebuild the solution with Visual Studio
    
13.  Run the application with Visual Studio

<hr>

## Swagger Documentation

### Login Credentials
**Admin**:<br>
`{
  "username": "admin",
  "password": "P@ssw0rd",
}`

### Available APIs
1. This application utilizes two different ports:
   1. Frontend; Node & ReactJS:<br>`44419`
   2. Backend; WebAPI on C# ASP 6.0:<br>`44420`
   
2. To view the available APIs, include `/swagger` at the end of the backend's url.<br>`https://localhost:44420/swagger`<br>
![Available_API_02](https://github.com/UofG-CS/project-TP-group15/blob/development/images/Available_API_02.png?raw=true)

3. Postman is recommend for unit testing/making direct calls on APIs

4. There are 3 roles in hierarchical order:
   1. Admin
   2. DashboardUser
   3. MobileUser

5. Most of the APIs are protected by Authorization which requires you to login with a valid Json Web Token (JWT)
   
6. To login, use `/api/Login` with the login credentials<br>
![Available_API_06](https://github.com/UofG-CS/project-TP-group15/blob/development/images/Available_API_06.png?raw=true)

7. A JWT (token) will be generated, copy it.
   1. If you are using Swagger:<br>
![Available_API_07-01](https://github.com/UofG-CS/project-TP-group15/blob/development/images/Available_API_07-01.png?raw=true)
      1. Scroll to the top of the page
      2. Click on the `Authorize` button
      3. Enter:`bearer <your copied token>`
      4. Click on `Authorize`<br>
![Available_API_07-01-04](https://github.com/UofG-CS/project-TP-group15/blob/development/images/Available_API_07-01-04.png?raw=true)
   2. If you are using Postman:
      1.  Create an API Request
      2.  Under `Authorization` tab, select `Bearer Token` under the `Type` drop down list
      3.  Paste the token `<your copied token>` into the Token text field

8. To create a MobileUser, you can utlize `/api/Registration/MobileUser`.<br>
![Available_API_08](https://github.com/UofG-CS/project-TP-group15/blob/development/images/Available_API_08.png?raw=true)
   1. Note, this API does not require any login
   2. Fill up the necessary details
   3. Example of details required is provided by Swagger

9.  To create DashboardUser, you can utlize `/api/Registration/DashboardUser`.<br>
![Available_API_09](https://github.com/UofG-CS/project-TP-group15/blob/development/images/Available_API_09.png?raw=true)
   1. Note, this API requires an Admin login
   2. Fill up the necessary details
   3. Example of details required is provided by Swagger
   

## Deployment Steps (Ensure that in AWS EC2 Console, Singapore region, travelrewards-webserver is running. In RDS Management Console, ensure that travel-rewards-db is running.)
### Backend API Server 
   1. cd into project folder with sln file inside: eg. project-TP-group15\ActivitySurveyAppForSmartCityPlanning
   2. Run this command: dotnet publish -c Release -r linux-x64 --self-contained true
   3. From the project folder, navigate to bin\Release\net6.0\linux-x64\publish
   4. Select all the files inside the publish folder and zip them up and name the zipped folder app.zip
   5. SSH into the EC2 instance. To use putty, use tp_keypair.ppk and insert it into the private key file for authentication.
   6. For the host name: enter ec2-user@18.139.118.56 and the port number is 22. Ensure that the SSH connection can be made.
   7. Back on the local machine, use tp_keypair pem. 
   8. cd into where tp_keypair.pem and app.zip is stored.
   9. Run this command: scp -i tp_keypair.pem app.zip ec2-user@ec2-18-139-118-56.ap-southeast-1.compute.amazonaws.com:~/.
   10. Inside the EC2 instance, there will be an app.zip in the root folder.
   11. Run these commands: mkdir app, mv app.zip app, cd app, unzip app.zip
   12. After unzipping, cd into the publish folder. (/home/ec2-user/app/publish)
   13. Run this command: chmod 777 ActivitySurveyAppForSmartCityPlanning
   14. Run this command: screen
   15. Run this command: ./ActivitySurveyAppForSmartCityPlanning --urls http://+:5000 --environment Development
   16. Ctrl + a + d
   17. Now, we can exit from the ssh session and the backend server will keep running.
   18. The API can now be accessed through this URL: http://18.139.118.56:5000
   19. To stop the server, ssh back into the EC2 instance
   20. Run this command: screen -r
   21. ctrl + c
   22. Run this command: exit
   23. The tp_keypair.ppk and tp_keypair.pem files will be sent separately via email or they can be generated via AWS Console.
   
### Frontend Web Dashboard
   1. cd into ClientApp of the project folder: project-TP-group15\ActivitySurveyAppForSmartCityPlanning\ActivitySurveyAppForSmartCityPlanning\ClientApp
   2. Run this command: npm run build
   3. Select the build folder, then zip it up into a zipped folder named build.zip
   4. scp the build.zip file into the EC2 instance: scp -i tp_keypair.pem build.zip ec2-user@ec2-18-139-118-56.ap-southeast-1.compute.amazonaws.com:~/. (Need the tp_keypair.pem file for this also)
   5. In the EC2 instance, create a build folder. Unzip the build.zip into the build folder.
   6. So, ensure that the unzipped build files are inside this folder: /home/ec2-user/build/build
   7. Run this command: sudo su
   8. Run this command: service nginx start
   9. The Frontend Web Dashboard can now be accessed through this URL: http://18.139.118.56:5001

