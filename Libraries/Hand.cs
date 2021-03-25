using System;
using System.Collections.Generic;
using System.Linq;

namespace Poker
{
    public class Hand
    {
        private readonly int[] _cardKindsInARow = new int[15]; // 0 is skipped, 1 and 14 are Ace
        private readonly List<Card> _cards = new();
        private readonly Dictionary<CardKind, int> _cardKindCounts = new();
        private readonly Dictionary<CardSuit, int> _cardSuitCounts = new();

        private int? _totalWildCards = null;
        /// <summary>
        /// Property that returns the total number of wildcards in this hand.
        /// </summary>
        public int TotalWildCards
        {
            get
            {
                if (!_totalWildCards.HasValue)
                {
                    _totalWildCards = _cards.Count(c => c.IsWildCard);
                }
                return _totalWildCards.Value;
            }
        }

        private CardKind? _minCardInARow = null;
        public CardKind MinCardInARow
        {
            get
            {
                if (!_minCardInARow.HasValue)
                {
                    _minCardInARow = CardKind.Ace;
                }

                return _minCardInARow.Value;
            }
        }

        private CardKind? _maxCardInARow = null;
        public CardKind MaxCardInARow
        {
            get
            {
                if (!_maxCardInARow.HasValue)
                {
                    _maxCardInARow = CardKind.Ace;
                }

                return _maxCardInARow.Value;
            }
        }

        public int MaxCardsInARow
        {
            get; private set;
        }

        private Tuple<CardSuit?, int>? _nonWildCardMaxSameSuitCards = null;
        /// <summary>
        /// Returns the maximum number of cards that are not wildcards and are of the same suit (Clubs, Diamonds, Hearts, Spades).
        /// </summary>
        public Tuple<CardSuit?, int> NonWildCardMaxSameSuitCards
        {
            get
            {
                if (_nonWildCardMaxSameSuitCards == null)
                {
                    _nonWildCardMaxSameSuitCards = new Tuple<CardSuit?, int>(null, 0);
                    if (_cardSuitCounts.Count > 0)
                    {
                        // Generate a list of tuples <suit, count> of non-wildcards
                        // The count indicates the number of card of that suit
                        // They will be ordered by count, descending
                        var cardSuitCounts = from c in _cards
                                             where c.IsWildCard == false
                                             group c by c.Suit into g
                                             orderby g.Count(), g.Key descending
                                             select new { suit = g.Key, count = g.Count() };

                        if (cardSuitCounts.Any())
                        {
                            var first = cardSuitCounts.First();
                            _nonWildCardMaxSameSuitCards = new Tuple<CardSuit?, int>(first.suit, first.count);
                        }
                    }
                }

                return _nonWildCardMaxSameSuitCards;
            }
        }

        private CardKind? _nonWildCardSameKindQuartet = null;
        /// <summary>
        /// If the deck has a quartet (four of the same kind) and none of them are wildcards,
        /// returns the kind of that quartet. Otherwise, returns null.
        /// </summary>
        public CardKind? NonWildCardSameKindQuartet
        {
            get
            {
                if (!_nonWildCardSameKindQuartet.HasValue)
                {
                    var quartet = GetNonWildCardSameKindEnsemble(4);
                    if (quartet.Any())
                    {
                        _nonWildCardSameKindQuartet = quartet.FirstOrDefault();
                    }
                }

                return _nonWildCardSameKindQuartet;
            }
        }

        private CardKind? _nonWildCardSameKindTrio = null;
        /// <summary>
        /// If the deck has a trio (three of the same kind) and none of them are wildcards,
        /// returns the kind of that trio. Otherwise, returns null.
        /// </summary>
        public CardKind? NonWildCardSameKindTrio
        {
            get
            {
                if (!_nonWildCardSameKindTrio.HasValue)
                {
                    var trio = GetNonWildCardSameKindEnsemble(3);
                    if (trio.Any())
                    {
                        _nonWildCardSameKindTrio = trio.FirstOrDefault();
                    }
                }

                return _nonWildCardSameKindTrio;
            }
        }

        private List<CardKind>? _nonWildCardSameKindDuos = null;
        /// <summary>
        /// If the deck has duos (two cards of the same kind) and none of them are wildcards, returns 
        /// a list of the kinds of that duo or those duos. Otherwise, returns an empty list.
        /// </summary>
        public List<CardKind> NonWildCardSameKindDuos
        {
            get
            {
                if (_nonWildCardSameKindDuos == null)
                {
                    _nonWildCardSameKindDuos = GetNonWildCardSameKindEnsemble(2).ToList();
                }

                return _nonWildCardSameKindDuos;
            }
        }

        private bool? _isFiveOfAKind = null;
        /// <summary>
        /// Highest rank. Only possible with wildcards.
        /// Returns true only if the hand has at least one of these properties:
        /// - Has 4 wildcards and 1 any other card.
        /// - Has 3 wildcards and 1 non-wildcard duo.
        /// - Has 2 wildcards and 1 non-wildcard trio.
        /// - Has 1 wildcard and 1 non-wildcard quartet.
        /// </summary>
        public bool IsFiveOfAKind
        {
            get
            {
                if (!_isFiveOfAKind.HasValue)
                {
                    _isFiveOfAKind = TotalWildCards == 4 ||
                    (TotalWildCards == 3 && NonWildCardSameKindDuos.Count > 0) ||
                    (TotalWildCards == 2 && NonWildCardSameKindTrio != null) ||
                    (TotalWildCards == 1 && NonWildCardSameKindQuartet != null);
                }

                return _isFiveOfAKind.Value;
            }
        }

        private bool? _isRoyalFlush = null;
        /// <summary>
        /// Retutns true only if the hand has 5 cards of the same suit, and they are 10, J, Q, K and A.
        /// </summary>
        public bool IsRoyalFlush
        {
            get
            {
                if (!_isRoyalFlush.HasValue)
                {
                    _isRoyalFlush = _cardSuitCounts.ContainsValue(5) &&
                                   _cardKindCounts.ContainsKey(CardKind.Ten) &&
                                   _cardKindCounts.ContainsKey(CardKind.Joker) &&
                                   _cardKindCounts.ContainsKey(CardKind.Queen) &&
                                   _cardKindCounts.ContainsKey(CardKind.King) &&
                                   _cardKindCounts.ContainsKey(CardKind.Ace);
                }

                return _isRoyalFlush.Value;
            }
        }

        private bool? _isStraightFlush = null;
        /// <summary>
        /// Returns true only if the hand has at least one of these properties:
        /// - Has 4 wildcards and any other card.
        /// - Has 3 wildcards and 2 non-wildcards of the same suit, all of them in a row.
        /// - Has 2 wildcards and 3 non-wildcards of the same suit, all of them in a row.
        /// - Has 1 wildcard and 4 non-wildcards of the same suit, all of them in a row.
        /// - Has no wildcards, and 5 non-wildcards of the same suit, all of them in a row.
        /// </summary>
        public bool IsStraightFlush
        {
            get
            {
                if (!_isStraightFlush.HasValue)
                {
                    _isStraightFlush = IsStraight && IsFlush;
                }

                return _isStraightFlush.Value;
            }
        }

        private bool? _isFourOfAKind = null;
        /// <summary>
        /// Returns true only if the hand has at least one of these properties:
        /// - Has 4 wildcards and any other card.
        /// - Has 3 wildcards and any other 2 cards.
        /// - Has 2 wildcards and at least one non-wildcard duo.
        /// - Has 1 wildcard and one non-wildcard trio.
        /// - Has a non-wildcard quartet.
        /// </summary>
        public bool IsFourOfAKind
        {
            get
            {
                if (!_isFourOfAKind.HasValue)
                {
                    _isFourOfAKind = TotalWildCards == 4 ||
                    TotalWildCards == 3 ||
                    (TotalWildCards == 2 && NonWildCardSameKindDuos.Count > 0) ||
                    (TotalWildCards == 1 && NonWildCardSameKindTrio != null) ||
                    NonWildCardSameKindQuartet != null;
                }

                return _isFourOfAKind.Value;
            }
        }

        private bool? _isFullHouse = null;
        /// <summary>
        /// Returns true only if the hand has at least one of these properties:
        /// - Four wildcards and any other card.
        /// - Three wildcards and any other two cards.
        /// - Two wildcards and:
        ///     - One any card and one non-wildcard duo, or
        ///     - One non-wildcard trio.
        /// - One wildcard and:
        ///     - Two non-wildcard duos, or
        ///     - One any card and one non-wildcard trio.
        /// - Zero wildcards, one non-wildcard duo and one non-wildcard trio.
        /// </summary>
        public bool IsFullHouse
        {
            get
            {
                if (!_isFullHouse.HasValue)
                {
                    _isFullHouse = NonWildCardSameKindQuartet == null && (TotalWildCards == 4 ||
                    TotalWildCards == 3 ||
                    (TotalWildCards == 2 && (NonWildCardSameKindDuos.Count == 1 || NonWildCardSameKindTrio != null)) ||
                    (TotalWildCards == 1 && (NonWildCardSameKindDuos.Count == 2 || NonWildCardSameKindTrio != null)) ||
                    (TotalWildCards == 0 && NonWildCardSameKindDuos.Count == 1 && NonWildCardSameKindTrio != null));
                }

                return _isFullHouse.Value;
            }
        }

        private bool? _isFlush = null;
        /// <summary>
        /// Returns true only if the hand has at least one of these properties:
        /// - Four wildcards plus any other card.
        /// - Three wildcards plus two non-wildcards of the same suit.
        /// - Two wildcards plus three non-wildcards of the same suit.
        /// - One wildcard plus four non-wildcards of the same suit.
        /// - Five cards of the same suit.
        /// </summary>
        public bool IsFlush
        {
            get
            {
                if (!_isFlush.HasValue)
                {
                    _isFlush = _cardSuitCounts.ContainsValue(5) || TotalWildCards == 4 ||
                              (TotalWildCards == 3 && NonWildCardMaxSameSuitCards.Item2 == 2) ||
                              (TotalWildCards == 2 && NonWildCardMaxSameSuitCards.Item2 == 3) ||
                              (TotalWildCards == 1 && NonWildCardMaxSameSuitCards.Item2 == 4) ||
                              NonWildCardMaxSameSuitCards.Item2 == 5;
                }

                return _isFlush.Value;
            }
        }

        private bool? _isStraight = null;
        /// <summary>
        /// Return true only if the hand has at least one of these properties:
        /// - Four wildcards and any other card.
        /// - Three wildcards and two non-wildcards, all of them in a row.
        /// - Two wildcards and three non-wildcards, all of them in a row.
        /// - One wildcard and four non-wildcards, all of them in a row.
        /// - Zero wildcards and five non-wildcards, all of them in a row.
        /// </summary>
        public bool IsStraight
        {
            get
            {
                if (!_isStraight.HasValue)
                {
                    _isStraight = MaxCardsInARow == 5;
                }

                return _isStraight.Value;
            }
        }

        private bool? _isThreeOfAKind = null;
        /// <summary>
        /// Return true only if the hand has at least one of these properties:
        /// - Four wildcards plus any other card.
        /// - Three wildcards plus any other two cards.
        /// - Two wildcards plus any other three cards.
        /// - One wildcard plus at least one non-wildcard kind duo (same kind, color/suit does not matter).
        /// - Zero wildcards plus one non-wildcard kind trio (same kind, color/suit does not matter).
        /// </summary>
        public bool IsThreeOfAKind
        {
            get
            {
                if (!_isThreeOfAKind.HasValue)
                {
                    _isThreeOfAKind = TotalWildCards >= 2 ||
                        (TotalWildCards == 1 && NonWildCardSameKindDuos.Count > 0) ||
                        (TotalWildCards == 0 && (NonWildCardSameKindTrio != null || NonWildCardSameKindQuartet != null));
                }

                return _isThreeOfAKind.Value;
            }
        }

        private bool? _isTwoPairs = null;
        /// <summary>
        /// Return true only if the hand has at least one of these properties:
        /// - Four wildcards plus any other card.
        /// - Three wildcards plus any other two cards.
        /// - Two wildcards plus any other three cards.
        /// - One wildcard plus one non-wildcard same kind duo.
        /// - Zero wildcards plus two non-wildcards same kind duos.
        /// - It's a quartet.
        /// </summary>
        public bool IsTwoPairs
        {
            get
            {
                if (!_isTwoPairs.HasValue)
                {
                    _isTwoPairs = TotalWildCards >= 2 ||
                                    NonWildCardSameKindDuos.Count == 2 ||
                                    NonWildCardSameKindQuartet != null ||
                                    (TotalWildCards == 1 && NonWildCardSameKindDuos.Count >= 1);
                }

                return _isTwoPairs.Value;
            }
        }

        private bool? _isPair = null;
        /// <summary>
        /// Returns true only if there is any kind of pair of cards.
        /// </summary>
        private bool IsPair
        {
            get
            {
                if (!_isPair.HasValue)
                {
                    _isPair = TotalWildCards > 0 ||
                                NonWildCardSameKindDuos.Count == 2 ||
                                IsTwoPairs ||
                                NonWildCardSameKindTrio != null ||
                                NonWildCardSameKindQuartet != null;
                }

                return false;
            }
        }

        public HandRank Rank
        {
            get
            {
                if (IsFiveOfAKind)
                {
                    return HandRank.FiveOfAKind;
                }
                else if (IsStraightFlush)
                {
                    return HandRank.StraightFlush;
                }
                else if (IsFourOfAKind)
                {
                    return HandRank.FourOfAKind;
                }
                else if (IsFullHouse)
                {
                    return HandRank.FullHouse;
                }
                else if (IsFlush)
                {
                    return HandRank.Flush;
                }
                else if (IsStraight)
                {
                    return HandRank.Straight;
                }
                else if (IsThreeOfAKind)
                {
                    return HandRank.ThreeOfAKind;
                }
                else if (IsTwoPairs)
                {
                    return HandRank.TwoPairs;
                }
                else if (IsPair)
                {
                    return HandRank.Pair;
                }

                return HandRank.HighCard;
            }
        }

        /// <summary>
        /// Constructor. Creates a hand with the provided cards.
        /// </summary>
        /// <param name="card1">Card 1.</param>
        /// <param name="card2">Card 2.</param>
        /// <param name="card3">Card 3.</param>
        /// <param name="card4">Card 4.</param>
        /// <param name="card5">Card 5.</param>
        public Hand(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            AddWithoutRepetition(card1);
            AddWithoutRepetition(card2);
            AddWithoutRepetition(card3);
            AddWithoutRepetition(card4);
            AddWithoutRepetition(card5);
            ResetHand();
        }

        /// <summary>
        /// Substitutes one card with another.
        /// </summary>
        /// <param name="cardOld">The old card in the hand.</param>
        /// <param name="cardNew">The new card to insert in the place of the old card.</param>
        public void Switch(Card cardOld, Card cardNew)
        {
            if (_cards.Remove(cardOld))
            {
                AddWithoutRepetition(cardNew);
            }

            ResetHand();
        }

        /// <summary>
        /// Organizes the cards by Kind.
        /// </summary>
        public void Sort() => _cards.Sort(CompareCards);

        private void AddWithoutRepetition(Card card)
        {
            if (_cards.Find(x => card.Equals(x)) != null)
            {
                throw new InvalidOperationException(string.Format("Card {0} already exists in the hand.", card));
            }
            _cards.Add(card);
        }

        /// <summary>
        /// Returns a list with the card kinds (A2345678910JKQ) that are found in the card with the total number provided.
        /// </summary>
        /// <param name="count">The amount of card kinds that we want to check if the hand has.</param>
        /// <returns>An enumerable of card kinds.</returns>
        private IEnumerable<CardKind> GetNonWildCardSameKindEnsemble(int count)
        {
            // From the list that contains the count of each card kind
            // retrieve the kind that is not a wildcard
            // and has <count> elements in this hand, if any
            var kind = from kv in _cardKindCounts
                       where kv.Value == count && kv.Key != Poker.WildCard
                       select kv.Key;

            return kind;
        }

        private static int CompareCards(Card card1, Card card2) => card1.Kind.CompareTo(card2.Kind);

        private void UpdateCardKindCounts(Card card)
        {
            if (_cardKindCounts.ContainsKey(card.Kind))
            {
                _cardKindCounts[card.Kind]++;
            }
            else
            {
                _cardKindCounts.Add(card.Kind, 1);
            }

            if (_cardKindCounts[card.Kind] > 4)
            {
                throw new IndexOutOfRangeException("You are trying to create a hand with more than 4 cards of the same kind.");
            }
        }

        private void UpdateCardSuitCounts(Card card)
        {
            if (_cardSuitCounts.ContainsKey(card.Suit))
            {
                _cardSuitCounts[card.Suit]++;
            }
            else
            {
                _cardSuitCounts.Add(card.Suit, 1);
            }
        }

        private void CalculateCardsInARow()
        {
            // Every time a new hand is created, we need to revert the array to zeros
            // This array is 1-indexed: Ace=1, King=13, so we always skip 0
            foreach (CardKind kind in Enum.GetValues(typeof(CardKind)))
            {
                _cardKindsInARow[(int)kind] = 0;
            }
            // Add the Ace as the last element as well
            _cardKindsInARow[14] = 0;

            // Now add up each card kind except wildcards
            foreach (Card card in _cards)
            {
                if (!card.IsWildCard)
                {
                    _cardKindsInARow[(int)card.Kind]++;
                }
            }
            // Put the same number of Aces at the end
            _cardKindsInARow[14] = _cardKindsInARow[(int)CardKind.Ace];

            // Now iterate through the array of card counts and save the max number of contiguous cards
            MaxCardsInARow = 1;
            FindMaxCardsInARow();

            // Now save the wildcards from bottom to top and do the recount
            int initialLeft = 1;
            while (initialLeft + TotalWildCards < _cardKindsInARow.Length)
            {
                int left = initialLeft;
                int availableWildCards = TotalWildCards;
                while (left < _cardKindsInARow.Length && availableWildCards > 0)
                {
                    if (_cardKindsInARow[left] == 0)
                    {
                        _cardKindsInARow[left] = -1; // wildcard
                        availableWildCards--;
                    }
                    left++;
                }

                FindMaxCardsInARow();

                // Reset all the wildcards
                left = 1;
                while (left < _cardKindsInARow.Length)
                {
                    if (_cardKindsInARow[left] == -1)
                    {
                        _cardKindsInARow[left] = 0;
                    }
                    left++;
                }

                initialLeft++;
            }
        }

        private void FindMaxCardsInARow()
        {
            int left = 1;

            // Count the number of cards in a row (with no wildcards)
            while (left < _cardKindsInARow.Length)
            {
                if (_cardKindsInARow[left] > 0 || _cardKindsInARow[left] == -1)
                {
                    int right = left + 1;
                    while (right < _cardKindsInARow.Length &&
                        (_cardKindsInARow[right] > 0 || _cardKindsInARow[right] == -1))
                    {
                        right++;
                    }
                    int tempMaxCount = right - left;

                    // Greater OR equal, because the higher the min and max, the better
                    if (tempMaxCount >= MaxCardsInARow)
                    {
                        MaxCardsInARow = tempMaxCount;
                        _minCardInARow = (CardKind)left;
                        _maxCardInARow = (CardKind)right-1;
                    }
                    left = right;
                }
                left++;
            }
        }

        private void UpdateHashes()
        {
            _cardKindCounts.Clear();
            _cardSuitCounts.Clear();

            foreach(Card card in _cards)
            {
                UpdateCardKindCounts(card);
                UpdateCardSuitCounts(card);
            }

            CalculateCardsInARow();
        }

        private void ResetHand()
        {
            MaxCardsInARow = 0;
            _totalWildCards = null;
            _minCardInARow = null;
            _maxCardInARow = null;
            _nonWildCardMaxSameSuitCards = null;
            _nonWildCardSameKindQuartet = null;
            _nonWildCardSameKindTrio = null;
            _nonWildCardSameKindDuos = null;
            _isFiveOfAKind = null;
            _isRoyalFlush = null;
            _isStraightFlush = null;
            _isFourOfAKind = null;
            _isFullHouse = null;
            _isFlush = null;
            _isStraight = null;
            _isThreeOfAKind = null;
            _isTwoPairs = null;
            _isPair = null;

            UpdateHashes();
        }

        public override string ToString()
        {
            string str = string.Empty;
            foreach (Card card in _cards)
            {
                str += $"{card}{Environment.NewLine}";
            }
            str += Environment.NewLine;
            return str;
        }

    }
}
