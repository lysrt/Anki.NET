using AnkiNet.CollectionFile.Model;
using AnkiNet.CollectionFile.Model.Json;

namespace AnkiNet.CollectionFile;

internal sealed class InternalConverter
{
    public InternalConverter()
    {
    }

    public Collection ConvertAnkiCollectionToCollection(AnkiCollection collection)
    {
        var result = new Collection(
            1, // Arbitrary
            0,
            0,
            0,
            11, // See https://github.com/ankitects/anki/blob/main/rslib/src/storage/upgrades/mod.rs (should it be 18?)
            0,
            0,
            0,
            new JsonConfiguration
            {
                SortBackwards = false,
                CurrentDeck = 1,
                DueCounts = true,
                SortType = "noteFld",
                CurrentModel = collection.NoteTypes.First().Id,
                TimeLimit = 0,
                NewSpread = 0,
                CollapseTime = 1200,
                EstimateTimes = true,
                AddToCurrent = true,
                NextPosition = 1,
                DayLearnFirst = false,
                SchedulerVersion = 2,
                CreationOffset = -480,
                ActiveDecks = new[] { 1 },
                NewBury = false,
                LastUnburied = 0,
            },
            collection.NoteTypes.ToDictionary(
                nt => nt.Id,
                nt => new JsonModel
                {
                    Id = nt.Id,
                    Name = nt.Name,
                    ModificationTime = 0,
                    Css = nt.Css ?? string.Empty,
                    DefaultDeckId = null,
                    ModelType = 0,
                    UpdateSequenceNumber = 0,
                    LegacyVersionNumber = null,
                    LatexPost = "\\end{ document }",
                    LatexPre = "\\documentclass[12pt]{article}\n\\special{papersize=3in,5in}\n\\usepackage[utf8]{inputenc}\n\\usepackage{amssymb,amsmath}\n\\pagestyle{empty}\n\\setlength{\\parindent}{0in}\n\\begin{document}\n",
                    LatexSvg = false,
                    BrowserSortField = 0,
                    LastAddedNoteTags = null,
                    CardTemplates = nt.CardTypes.Select(ct => new JsonCardTemplate
                    {
                        TemplateName = ct.Name,
                        TemplateOrdinal = ct.Ordinal,
                        DeckOverrideId = null,
                        AnswerFormat = ct.AnswerFormat,
                        BrowserAnswerFormat = string.Empty,
                        QuestionFormat = ct.QuestionFormat,
                        BrowserQuestionFormat = string.Empty,
                        BFont = string.Empty,
                        BSize = 0
                    }).ToArray(),
                    Fields = nt.FieldNames.Select((field, index) => new JsonField
                    {
                        FieldName = field,
                        FieldNumber = index,
                        IsRightToLeft = false,
                        IsSticky = false,
                        Font = "Arial", // TODO Make this customizable?
                        FontSize = 20, // TODO Make this customizable?
                        Description = string.Empty,
                        Media = null
                    }).ToArray(),
                    RequiredFields = new object[]
                    {
                        0,
                        "any",
                        new object[] {0}
                    }
                }
            ),
            collection.Decks.ToDictionary(
                d => d.Id,
                d => new JsonDeck
                {
                    Id = d.Id,
                    LastModificationTime = 0,
                    Name = d.Name,
                    UpdateSequenceNumber = 0,
                    NewToday = new[] { 0, 0 },
                    ReviewedToday = new[] { 0, 0 },
                    LearnedToday = new[] { 0, 0 },
                    TimeToday = new[] { 0, 0 },
                    IsCollapsed = false,
                    IsCollapsedInBrowser = false,
                    Description = string.Empty, // TODO Handle deck description?
                    IsDynamic = 0,
                    ConfigurationGroupId = 1,
                    ExtendedNewCardLimit = 0,
                    ExtendedReviewCardLimit = 0,
                }
            ),
            new Dictionary<long, JsonDeckConfguration>
            {
                {
                    1,
                    new JsonDeckConfguration
                    {
                        Id = 1,
                        LastModificationTime = 0,
                        Name = "Default",
                        UpdateSequenceNumber = 0,
                        AutoplayQuestionAudio = true,
                        ReplayQuestionAudio = true,
                        ShowTimer = 0,
                        IsDynamic = false,
                        StopTimerAfterSeconds = 0,
                        LapseCardsConfiguration = new JsonLapseCardsConfiguration
                        {
                            Delays = new[]{ 10f },
                            LapsedIntervalMultiplierPercent = 0,
                            LeechAction = 1,
                            LeechFailsAllowedCount = 8,
                            MinimumInterfalAfterLeech = 1
                        },
                        NewCardsConfiguration = new JsonNewCardsConfiguration
                        {
                            Bury = false,
                            Delays = new[] { 1f, 10f },
                            InitialEaseFactor = 2500,
                            IntDelays = new[] { 1, 4, 0 },
                            NewCardsPerDay = 20,
                            NewCardsShowOrder = 1,
                            Separate = 0
                        },
                        ReviewCardsConfiguration = new JsonReviewCardsConfiguration
                        {
                            Bury = false,
                            CardsToReviewPerDay = 200,
                            Ease4 = 1.3f,
                            Fuzz = 0,
                            HardFactor = 1.2f,
                            IntervalMultiplicationFactor = 1,
                            MaximumReviewInterval = 36500,
                            MinSpace = 0
                        }
                    }
                }
            },
            "{}"
        );

        var allCards = collection.Decks.SelectMany(d => d.Cards).ToArray();
        var allNotes = allCards.Select(d => d.Note).Distinct().ToArray();
        var deckIdByCardId = collection.Decks
            .SelectMany(d => d.Cards.ToDictionary(c => c.Id, c => d.Id))
            .ToDictionary(d => d.Key, d => d.Value);

        result.Notes = allNotes.Select(n => new Note(
            n.Id,
            Guid.NewGuid().ToString().Substring(0, 10),
            n.NoteTypeId,
            0,
            0,
            "",
            n.Fields,
            n.Fields[0], // TODO Check this is correct
            0, // TODO Check this is correct
            0,
            ""
        )).ToArray();

        result.Cards = allCards.Select(c => new Card(
            c.Id,
            c.Note.Id,
            deckIdByCardId[c.Id],
            c.NoteCardTypeOrdinal,
            0,
            0,
            CardLearningType.New,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            0,
            ""
        )).ToArray();

        result.RevLogs = Array.Empty<RevisionLog>();
        result.Graves = Array.Empty<Grave>();
        
        return result;
    }

    public AnkiCollection ConvertCollectionToAnkiCollection(Collection collection)
    {
        var resultCollection = new AnkiCollection();

        foreach (var model in collection.Models.Values)
        {
            var cardTypes = model.CardTemplates.Select(ct =>
            {
                return new AnkiCardType(
                    ct.TemplateName,
                    ct.TemplateOrdinal,
                    ct.QuestionFormat,
                    ct.AnswerFormat
                );
            }).ToArray();

            var fields = model.Fields.Select(field =>
            {
                return field.FieldName;
            }).ToArray();

            var noteType = new AnkiNoteType(model.Id, model.Name, cardTypes, fields, model.Css);
            resultCollection.AddNoteType(noteType);
        }

        // Add the decks.
        foreach (var deck in collection.Decks.Values)
        {
            if (deck.Id == 1)
            {
                // Do not add default deck as AnkiCollection already has it.
                continue;
            }
            resultCollection.AddDeck(deck.Id, deck.Name);
        }

        // Add the notes and their associated cards, keeping the existing ids.
        var cardsByNoteId = collection.Cards.ToLookup(c => c.NoteId);
        foreach (var note in collection.Notes)
        {
            var cardsForThisNote = cardsByNoteId[note.Id]; // TODO Error handling
            var deckId = cardsForThisNote.Select(c => c.DeckId).Distinct().Single(); // TODO Error handling

            var ids = cardsForThisNote.Select(c => (c.Ordinal, c.Id)).ToArray();

            resultCollection.AddNoteWithCards(note.Id, deckId, note.ModelId, note.Fields, ids);
        }

        // Ignore RevLogs and Graves

        return resultCollection;
    }
}