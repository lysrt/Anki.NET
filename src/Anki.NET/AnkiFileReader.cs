using System.IO.Compression;
using AnkiNet.database;
using AnkiNet.database.model;
using AnkiNet.mapper;
using AnkiNet.model;
using Microsoft.Data.Sqlite;

namespace AnkiNet;

public class AnkiFileReader
{
    public static async Task<AnkiCollection> ReadCollection(string filePath)
    {
        var db = await ReadDbAsync(filePath);
        var collection = ConvertDbToModels(db);
        return ConvertCollectionToAnkiCollection(collection);
    }

    private static async Task<DatabaseExtract> ReadDbAsync(string filePath)
    {
        if (!File.Exists(filePath))
        {
            throw new FileNotFoundException($"File '{filePath}' does not exist");
        }

        using var zipArchive = ZipFile.OpenRead(filePath);

        // collection.anki2
        // collection.anki21
        // media
        const string dbFileName = "collection.anki21";
        var databaseEntry = zipArchive.Entries.SingleOrDefault(e => e.Name == dbFileName);
        if (databaseEntry == null)
        {
            throw new InvalidOperationException($"File '{filePath}' has no database called '{dbFileName}'");
        }

        // Open DB file
        var tempFile = Path.GetTempFileName();
        databaseEntry.ExtractToFile(tempFile, true);

        SQLitePCL.Batteries.Init();

        using var conn = new SqliteConnection($"Data Source={tempFile};");
        conn.Open();

        var col = (await new ColRepository().ReadAll(conn)).Single();
        var cards = await new CardRepository().ReadAll(conn);
        var graves = await new GraveRepository().ReadAll(conn);
        var notes = await new NoteRepository().ReadAll(conn);
        var revLogs = await new RevLogRepository().ReadAll(conn);

        File.Delete(tempFile);

        return new DatabaseExtract(col, cards, graves, notes, revLogs);
    }

    private static Collection ConvertDbToModels(DatabaseExtract db)
    {
        var collection = CollectionMapper.FromDb(db.col);

        collection.Cards = db.cards.Select(c => CardMapper.FromDb(c)).ToArray();
        collection.Graves = db.graves.Select(g => GraveMapper.FromDb(g)).ToArray();
        collection.Notes = db.notes.Select(n => NoteMapper.FromDb(n)).ToArray();
        collection.RevLogs = db.revLogs.Select(r => RevisionLogMapper.FromDb(r)).ToArray();

        return collection;
    }

    private static AnkiCollection ConvertCollectionToAnkiCollection(Collection collection)
    {
        // Ignore:
        //    collection.RevLogs
        //    collection.Graves
        // Keep:
        //    collection [properties]
        //    collection.Cards
        //    collection.Decks
        //    collection.Models

        var resultCollection = new AnkiCollection();

        foreach (var model in collection.Models.Values)
        {
            var noteType = new AnkiNoteType(model.Id, model.Name)
            {
                Css = model.Css,
                Fields = model.Fields.Select(field =>
                {
                    return field.FieldName;
                }).ToArray(),
                CardTypes = model.CardTemplates.Select(ct =>
                {
                    return new AnkiCardType(
                        ct.TemplateName,
                        ct.TemplateOrdinal,
                        ct.QuestionFormat,
                        ct.AnswerFormat
                    );
                }).ToArray()
            };
            resultCollection.AddNoteType(noteType);
        }

        foreach (var deck in collection.Decks.Values)
        {
            if (deck.Id == 1)
            {
                // Do not add default deck at AnkiCollection already has it.
                continue;
            }
            resultCollection.AddDeck(deck.Id, deck.Name);
        }

        var notes = collection.Notes.ToDictionary(n => n.Id);
        /*
        foreach (var note in collection.Notes)
        {
            var ankiNote = new AnkiNote(note.ModelId, note.Fields);
            resultCollection.AddNote(ankiNote);
        }
        */

        foreach (var card in collection.Cards)
        {
            // TODO
            // Keep
            //   NoteId
            //   DeckId
            //   Ordinal (CardTypeOrdinal)

            var cardId = card.Id;
            var deckId = card.DeckId;
            var cardTypeOrdinal = card.Ordinal;

            var note = notes[card.NoteId];

            var fieldValues = note.Fields;
            var noteTypeId = note.ModelId;

            /*
            var noteType = resultCollection.NoteTypes[noteTypeId];
            var fieldNames = noteType.Fields;
            
            var cardType = noteType.CardTypes.Single(ct => ct.Ordinal == cardTypeOrdinal);
            var front = cardType.QuestionFormat;
            var back = cardType.AnswerFormat;

            for (var i = 0; i < fieldNames.Length; i++)
            {
                var fieldName = fieldNames[i];
                var fieldValue = fieldValues[i];

                var fieldNameToken = $"{{{{{fieldName}}}}}";

                front = front.Replace(fieldNameToken, fieldValue);
                back = back.Replace(fieldNameToken, fieldValue);
            }
            */

            var ankiNote = new AnkiNote {
                Id = card.NoteId,
                NoteTypeId = noteTypeId,
                Fields = fieldValues
            };
            var newCard = new AnkiCard {
                Id = cardId,
                Note = ankiNote,
                NoteCardTypeOrdinal = cardTypeOrdinal
            };

            var deckToAddCardTo = resultCollection.GetDeckById(deckId);
            if (deckToAddCardTo == null)
            {
                throw new InvalidOperationException($"Deck with ID '{deckId}' not found in collection");
            }

            deckToAddCardTo.Cards.Add(newCard);
        }

        /*
        //
        // Build decks hierarchy
        //
        var rootDeck = BuildTrie(decks);

        var cleanedDeck = CleanDeck(rootDeck);

        resultCollection.AddDeck(cleanedDeck);
        */

        return resultCollection;
    }

    /*
    private static AnkiDeck CleanDeck(AnkiDeck root)
    {
        // Remove default deck (id == 1) if it is empty and there is at least another deck
        // If there is only one deck left, use it as the new root

        if (root.SubDecks.Count() == 1)
        {
            return root.SubDecks.Single();
        }

        // There are several decks
        var defaultDeck = root.SubDecks.Single(d => d.Id == 1);
        var defaultDeckIsEmpty = defaultDeck.SubDecks.Count() == 0 || defaultDeck.Cards.Count() == 0;

        if (defaultDeckIsEmpty)
        {
            root.SubDecks.Remove(defaultDeck);
        }

        if (root.SubDecks.Count() == 1)
        {
            return root.SubDecks.Single();
        }

        return root;
    }
    */

    /*
    private static AnkiDeck BuildTrie(AnkiDeck[] input)
    {
        var root = new AnkiDeck(0, "");

        foreach (var deck in input)
        {
            Insert(root, deck);
        }

        // TODO Some decks here will have Id -1 if they're only "virtual".

        return root;
    }
    */

    /*
    private static void Insert(AnkiDeck root, AnkiDeck deckToInsert)
    {
        var currentDeck = root;
        var childNames = deckToInsert.Name.Split("::");
        foreach (var childName in childNames)
        {
            if (!currentDeck.SubDecks.Any(child => child.Name == childName))
            {
                var newDeck = new AnkiDeck(-1, childName);
                currentDeck.AddSubDeck(newDeck);
            }
            currentDeck = currentDeck.SubDecks.Single(child => child.Name == childName);
        }

        // Copy all properties to the node
        foreach (var cardToAdd in deckToInsert.Cards)
        {
            currentDeck.AddCard(cardToAdd);
        }
        currentDeck.SetId(deckToInsert.Id);

        // TODO And more properties to copy...
    }
    */
}
