##Fullstack opgave
#Introduktion
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
>Projektet skal være veldokumenteret og enten afleveres via github eller zip til karriere@ivaldi.dk senest torsdag d. 22. januar kl. 12."

Denne Readme er dokumentation for min løsning af opgaven.

# Overblik
Jeg har defineret et domæne som vist herunder, hvor en bruger administrere en liste af virksomheder identificeret ved virksomhedens CVR-nummer. Virksomheden har et navn, en adresse, et postnummer og by.

	[bruger] 1 -- administrerer -- * [virksomhed] 1 -- har -- 1 [cvr]
	                                  1        
	                                  |        
									 har     
							          |         
                                      1         
					               [navn, Adresse, postnumer, by]

 Løsningen gemmer virksomhederne i databasen, hvor løsningen henter virksomhedernes grunddata fra web-sitet cvrapi.dk.

 Brugeren kan arbejde med virksomhederne som følger:
 * Oprette en ny virksomhed i databasen ved at indtaste dens cvr-nummer,
 * Redigere en oprettet virksomhed,
 * Liste virksomheder op i databasen,
 * Slette en virksomhed fra databasen.

 Stakken i løsningen er 

