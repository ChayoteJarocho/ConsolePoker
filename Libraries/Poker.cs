using System;
using System.Collections.Generic;

namespace Poker
{
    public static class Poker
    {
        private static Pack? _pack = null;

        public static CardKind WildCard
        {
            get;
            private set;
        }

        public static int Count => _pack != null ? _pack.Count : 0;

        static Poker() => WildCard = CardKind.King;

        public static void NewGame(bool resetWildCard = false)
        {
            if (resetWildCard)
            {
                WildCard = CardKind.Ace;
            }
            else
            {
                SetNextWildCard();
            }

            _pack = new Pack();
            _pack.Randomize();
        }

        public static Card? GetCard()
        {
            if (_pack == null)
            {
                throw new InvalidOperationException("You have not started a game.");
            }
            return _pack.GetRandomCard();
        }

        private static void SetNextWildCard()
        {
            if (CardKind.King == WildCard)
            {
                WildCard = CardKind.Ace;
            }
            else
            {
                int iwc = (int) WildCard;
                iwc++;
                WildCard = Enum.Parse<CardKind>(iwc.ToString());
            }
        }

        private class Pack
        {
            private readonly List<Card> cards = new();
            private readonly Random random = new();

            public int Count => cards.Count;

            public Pack()
            {
                foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
                {
                    foreach (CardKind kind in Enum.GetValues(typeof(CardKind)))
                    {
                        cards.Add(new Card(suit, kind));
                    }
                }
            }

            public void Randomize()
            {
                var mirror = new List<Card>(cards);

                cards.Clear();

                while (mirror.Count > 0)
                {
                    int position = random.Next(0, mirror.Count);
                    Card extracted = mirror[position];
                    cards.Add(extracted);
                    mirror.RemoveAt(position);
                }
            }

            public Card? GetRandomCard()
            {
                Card? extracted = null;

                if (cards.Count > 0)
                {
                    int position = random.Next(0, cards.Count);
                    extracted = cards[position];
                    cards.RemoveAt(position);
                }

                return extracted;
            }

            public override string ToString()
            {
                string str = string.Empty;
                foreach (Card card in cards)
                {
                    str += $"{card}{Environment.NewLine}";
                }
                str += Environment.NewLine;
                return str;
            }
        }
    }
}