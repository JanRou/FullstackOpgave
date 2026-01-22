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
Jeg har defineret et domæne som vist herunder, hvor en bruger administrere en liste af virksomheder identificeret ved virksomhedens CVR-nummer. Virksomheden har et navn, en adresse, et postnummer og by.

	[bruger] 1 -- administrerer -- * [virksomhed] 1 -- har -- 1 [cvr]
	                                  1        
	                                  |        
									 har     
							          |         
                                      1         
					               [navn, Adresse, postnumer, by]

 Løsningen gemmer virksomhederne i en database, hvor løsningen henter virksomhedernes data fra web-sitet cvrapi.dk med cvr-nummeret som nøgle.

 Du kan arbejde med virksomhederne som følger:

  * Oprette en ny virksomhed i databasen ved at indtaste dens cvr-nummer,
  * Redigere en virksomhed,
  * Liste virksomheder,
  * Slette en virksomhed.

Du har en enkel og simpel web-side, hvor du kan udføre alle operationerne.

Stakken i løsningen har Vue i toppen som frontend mod brugeren implementeret i delprojektet _fullstackfe_. Frontend kommunikerer med backend, som er implementeret i delprojektet _fullstackbe_.

## Sådan kommer du i gang
Du bør have følgende klar:
	* Visual Studio Code
	* Internet adgang for at oprette nye virksomheder
	* Eventuelt Visual Studio, som jeg har brugt
	* DBBrowser eller andet værktøj til at inspicere SQLite databasen

1. Hent projeket fra Github, https://github.com/JanRou/FullstackOpgave
2. Åben backend solution i Visual Studio i folderen _fullstackbe_
3. Start backend i Debug eller F5 i Visual Studio (i VSC er det dotnet kommandoer)
4. Åben frontend projektet i Visual Studio Code i folderen _fullstackfe_, 
5. Start frontend med npm run dev
6. Åben hjemmesiden på localhost:3000, hvor frontend skulle køre

Du skulle gerne have løsningen kørende. Og du kan rette i frontend i VirksomhedMain.vue i folderen _src_.

Noter at, jeg har ikke test procduren.

## Frontend
Dialogen er en web-side, der er organiseret med en overskrift, _Virksomheder_, og en knap ![OPRET NY](OpretNy.png) til at oprette en ny virksomhed. Under overskriften er en liste af paneler, der viser alle virksomheder oprettet med deres navn som overskrift i panelerne. Du kan folde ud et panel og se detaljer for virksomheden, som cvr-nummer, adresse, postnummer og by. Der er to knapper i panelet til at redigere ![Rediger](Rediger.png) og slette ![Slet](Slet.png) virkomheden foldet ud.

Til frontend er brugt skabelonen Vuetify for brugerdialoger og layout og programmeret i Typescript, som specificeret i opgaven. Til kommunikationen med backend fra frontend er brugt en Apollo GraphQL klient.

## Backend
I backend er HotChocolate GraphQL brugt som præsentation af domænet med de fire operationer oprette, redigere og slette en virksomhed samt liste alle virksomheder. Backend gemmer virksomhederne i en SQlite database, hvor den bruger Microsoft Entity Framework Core til at under operationerne.

Backend er struktureret i stil med en clean code folder struktur:
Core
	Application - Virksomheds operationer
	Domain - Entiteten Virksomhed
Gateways
	CVRAPI - Integration til web-sitet cvrapi.dk
	Dal - Data access layer med db context og virksomhed dao, data access object
	Repository - Entity Framework Core operationer at liste, oprette, rediger og slette virksomheder i databasen
Presenters
	Types - GraphQL forespørgsler Query og Mutation



		
		 




