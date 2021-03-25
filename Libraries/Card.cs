using System;

namespace Poker
{
    public class Card : IEquatable<Card>
    {
        public CardColor Color
        {
            get;
            private set;
        }

        public CardSuit Suit
        {
            get;
            private set;
        }

        public CardKind Kind
        {
            get;
            private set;
        }

        public bool IsWildCard => Poker.WildCard == Kind;

        public Card(CardSuit s, CardKind r)
        {
            Suit  = s;
            Kind  = r;

            if (s is CardSuit.Diamonds or CardSuit.Hearts)
            {
                Color = CardColor.Red;
            }
            else
            {
                Color = CardColor.Black;
            }
        }

        public override string ToString() => $"[{Color}|{Suit}|{Kind}]";

        public bool Equals(Card? other) =>
            other != null &&
            other.Color == Color &&
            other.IsWildCard == IsWildCard &&
            other.Kind == Kind &&
            other.Suit == Suit;

        public override bool Equals(object? obj) => obj is Card other && Equals(other);

        public static bool operator ==(Card? card1, Card? card2)
        {
            if (card1 is null && card2 is null)
            {
                return true;
            }
            if (card1 is null || card2 is null)
            {
                return false;
            }

            return card1.Equals(card2);
        }

        public static bool operator !=(Card? card1, Card? card2) => !(card1 == card2);

        public override int GetHashCode() => base.GetHashCode();
    }
}
