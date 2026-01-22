# Fullstack opgave
I forbindelse med anden samtale hos Ivaldi ApS har jeg løst følgende opgave stillet af Ivaldi:

>Implementér et projekt, hvor man skal kunne håndtere virksomheder med nogle få stamdata. Systemet skal give mulighed for at oprette, redigere, liste og slette virksomheder, samt man skal kunne hente informationer ind fra cvrapi.dk ved indtastning af CVR-nr.
>Backend:
>- .NET Core
>- HotChocolate GraphQL
>- SQLite
>Frontend:
>- Vue.js
>- Typescript
>- Vuetify
>Projektet skal være veldokumenteret og enten afleveres via github eller zip til email senest torsdag d. 22. januar kl. 12."

Denne Readme er dokumentation for min løsning af opgaven.

## Overblik
Jeg har defineret et domæne som vist herunder, hvor en bruger administrere en liste af virksomheder identificeret ved virksomhedens CVR-nummer i det Centrale VirksomhedsRegister. Virksomheden har et navn, en adresse, et postnummer og en by.

	[bruger] 1 -- administrerer -- * [virksomhed] 1 -- har -- 1 [cvr]
	                                  1        
	                                  |        
									 har     
							          |         
                                      1         
					               [navn, Adresse, postnumer, by]

 Løsningen gemmer virksomhederne i en database. Den henter virksomhedernes data fra web-sitet cvrapi.dk med cvr-nummeret som nøgle, når du opretter en virksomhed.

 Du kan arbejde med virksomhederne som følger:

  * Oprette en ny virksomhed i databasen ved at indtaste dens CVR-nummer
  * Redigere en virksomheds oplysninger dog ikke CVR-nummer
  * Liste virksomheder ved deres navne
  * Slette en virksomhed

I en enkel web-side kan du udføre alle operationerne.

Stakken i løsningen har Vue i toppen som frontend mod brugeren implementeret i delprojektet _fullstackfe_. Frontend kommunikerer med backend, som er implementeret i delprojektet _fullstackbe_.

## Sådan kommer du i gang
Du bør have følgende klar:

	* Visual Studio Code
	* Internet adgang for at oprette nye virksomheder
	* Visual Studio, som jeg har brugt, alternativt brug Visusl Studio Code
	* DBBrowser eller andet værktøj til at inspicere SQLite databasen

1. Hent projeket fra Github, https://github.com/JanRou/FullstackOpgave
2. Åben backend solution i Visual Studio i folderen _fullstackbe_ i Backend folder
3. Start backend i Debug med F5 i Visual Studio (i VSC er det dotnet kommandoer)
4. Åben frontend projektet i Visual Studio Code i folderen _fullstackfe_ i Frontend folder,
5. Start frontend med kommandoen _npm run dev_ i en terminal
6. Åben hjemmesiden localhost:3000 i en browser, hvor frontend skulle køre

Backend åbner en GraphQL Nitro web-side, hvor man kan afvikle forespørgsler direkte mod backend i SDL, GraphQL Schema Defintion Language. Backend har endpoint url http://localhost:5095/graphql/.

Frontend kører en web-side på url http://localhost:3000/. Den er sat op til at gå på backend's graphql endpoint.

Du skulle gerne have løsningen kørende. Og du kan rette i frontend i VirksomhedMain.vue i folderen _./src/components_.

Noter, at jeg ikke har testet proceduren.

Til min bærbare med Ubuntu skulle nedenstående procedure til. 

 ###Backend
 Der manglede .Net SDK, runtime og aspnetcore i version 8 og 9. Og databasen skulle oprettes med dotnet-ef tool, som heller ikke var installeret.
 1. Installer dotnet-sdk-8 og 9, aspnetcore 8 og 9 og runtime 8 og 9 bash:
 - sudo add-apt-repository ppa:dotnet/backports
 - sudo apt-get update &&   sudo apt-get install -y dotnet-sdk-9.0
 - sudo apt-get update &&   sudo apt-get install -y aspnetcore-runtime-9.0
 - sudo apt-get install -y dotnet-runtime-9.0
 - sudo apt-get update &&   sudo apt-get install -y dotnet-sdk-8.0
 - sudo apt-get install -y aspnetcore-runtime-8.0
 - sudo apt-get update

 2. I Visual Studio Code, VSC, terminal restore pakker og byg
 - dotnet restore
 - dotnet build

 3. Installer dotnet-ef tool
 - dotnet tool install --global dotnet-ef

 4. Check migrationer
 - dotnet ef migrations list

 5. Ret database stien til linux i Program.cs:
```
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlite("Data Source=./data/database.db"));`
```
 6. Kør migrationer
 - dotnet build
 - dotnet ef database update

 7. Start applikationen og åben et browser, url http://localhost:5095/graphql
 - ASPNETCORE_URLS="http://localhost:5095" dotnet run & xdg-open http://localhost:5095/graphql

 8. Test i Nitro GraphQL, hent allevirksomheder
	`query {
	  hentAlleVirksomheder {
	    adresse
	    by
	    cvr
	    navn
	    postnummer
	  }
	}`

9. Og opret en virksomhed:
	```
 	mutation OpretVirksomhed($cvr: Int!) {
	   opretVirksomhed(cvr: $cvr) {
	    virksomhed {
	      adresse
	      by
	      cvr
	      navn
	      postnummer
	    }
	  }
   ```
	Variables:
	```
	{
	  "cvr": 28106661
	}
    ```
	Resultat:
	```
	"data": {
	    "opretVirksomhed": {
	      "virksomhed": {
	        "adresse": "Grenåvej 728",
	        "by": "Skødstrup",
	        "cvr": 28106661,
	        "navn": "Skødstrup Tandklinik ApS.",
	        "postnummer": 8541
	      }
	    }
	  }
   ```
  ###Frontend
  1. Installer npm og node i bash:
 - sudo apt install npm
 - sudo apt install nodejs

2. Installer pakker til frontend i VSC:
- npm install

3. Kør
- npm run dev
 
## Frontend
Dialogen er en web-side, der er organiseret med en overskrift, _Virksomheder_, og en knap ![OPRET NY](OpretNy.png) til at oprette en ny virksomhed. Under overskriften er en liste af paneler, der viser alle virksomheder oprettet med deres navn som overskrift i panelerne. Du kan folde et panel ud og se detaljer for virksomheden som CVR-nummer, adresse, postnummer og by. Der er to knapper i panelet til at redigere ![Rediger](Rediger.png) og slette ![Slet](Slet.png) virkomheden foldet ud.

Til frontend er brugt skabelonen Vuetify for brugerdialoger og layout samt programmeret i Typescript, som specificeret i opgaven. Til kommunikationen med backend fra frontend er brugt en Apollo GraphQL klient.

Hoveddialogen er programmeret i Virksomhedmain.vue, der er en SFC, Single-File Component, som ligger i _./src/components_. I folderen  _./src/graphql_ finder man graphql query, mutationer og typer. I kodefilen apollo-client.ts er opsætningen af graphql og backend's endpoint.

Der er ingen login side.

## Backend
I backend er HotChocolate GraphQL brugt som præsentation af domænet med de fire operationer oprette, redigere og slette en virksomhed samt liste alle virksomheder. Backend gemmer virksomhederne i en SQLite database, hvor den bruger EFCore, Microsoft Entity Framework Core, til at tilgå databasen.

Backend er struktureret i stil med en clean code folder struktur:

	- Core
		- Application - Virksomheds operationer i VirksimhedCrud.cs
		- Domain - Entiteten i Virksomhed.cs
 	- Gateways
		- CVRAPI - Integration til web-sitet cvrapi.dk i Cvrapi.cs
		- Dal - Data access layer med AppDbContext.cs og VirksomhedDao.cs med data access object
		- Repository - EFCore funktioner i VirksomhedRepository.cs
 	- Presenters
		- Types - GraphQL forespørgsler og typer i Query.cs, Mutationcs og VirksomhedInType.cs

I Program.cs finder man opsætningen af backend herunder at tillade alle former for adgange mht. CORS, Cross-Origin Resource sharing. Der er ingen sikring af adgang til data.
