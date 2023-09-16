## Magic The Gathering deckbuilder

### Description:

- The task was to create a deck builder with the functionality somehow similar to https://aetherhub.com/Deck/Builder by using .net technology. Mtg Database Backup (use the restore function in SSMS or ADS to put the tables and their data in place): mtg.bak.

### Technologies used:
- .NET and C#
- Blazor
- MongoDB
- GraphQL

### Mandatory requirements:
- Data Access Library
(contains Entities, DB Context, Repository classes)
- Shared Library
(contains DTO’s, Mappings, Filters, Extension methods, FluentValidation)
- Minimal API
(serves the end points for posting/update/deleting cards in the deck)
- Web API
(serves the end points for getting the cards data)
- Web Application (Blazor Server)
• Showing, filtering and sorting cards
• Adding/deleting cards to/from the deck
- GraphQL API
(for querying cards and artists)
Blazor App
REQUIREMENTS – the complete solution