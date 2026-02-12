# Elementi razvoja softvera projekat – sistem parfimerije **O'Sinjel de Or**

Konzolna .NET aplikacija za upravljanje procesom proizvodnje i prodaje parfema – od biljaka i prerade, preko ambalaže i skladišta, do evidencije događaja i prodaje.

## Kratak opis
Aplikacija simulira rad informacionog sistema parfimerije:
- vođenje evidencije biljaka,
- preradu biljaka u parfeme,
- upravljanje ambalažom i skladištima,
- beleženje događaja i logova,
- prodaju i izdavanje fiskalnih računa,
- autentifikaciju i rad sa korisničkim ulogama.

## Tehnologije
- **.NET 8**
- **C#**
- **NUnit + Moq** (testiranje)
- Slojevita arhitektura sa više projekata unutar jednog solution fajla.

## Arhitektura i struktura projekta
Rešenje (`ERS_PROJEKAT.sln`) je podeljeno na sledeće projekte:

- `Application/` – ulazna tačka aplikacije (`Main`) i povezivanje zavisnosti.
- `Domain/` – domen modeli, interfejsi repozitorijuma, servisi, enumeracije, konstante.
- `Services/` – implementacije poslovne logike i servisa.
- `Database/` – pristup i rad sa bazom podataka (JSON skladištenje).
- `Presentation/` – meni i korisnički interfejs (konzolni).
- `Tests/` – unit testovi za ključne servise.

## Početni podaci i prijava
Pri prvom pokretanju aplikacija automatski:
- kreira podrazumevano skladište (`Glavno skladište`),
- kreira početne korisnike ako ne postoje.

Podrazumevani nalozi:
- **menadzer / menadzer123**
- **prodavac / prodavac123**

## Glavne funkcionalnosti
- Prijava i registracija korisnika
- Rad sa biljkama (dodavanje, pregled, statusi)
- Kreiranje parfema iz raspoloživih biljaka
- Upravljanje ambalažom i lagerom
- Logistika između magacinskog i distributivnog centra
- Evidencija događaja i sistemsko logovanje
- Prodaja i fiskalni računi

## Napomene
- Aplikacija namenjena edukativnoj/demonstracionoj upotrebi.
