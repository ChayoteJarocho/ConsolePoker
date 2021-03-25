using System;
using System.Collections.Generic;
using Xunit;

namespace Poker.Tests
{
    public class PokerTests
    {
        private Card? card1;
        private Card? card2;
        private Card? card3;
        private Card? card4;
        private Card? card5;
        private Hand? hand;

        /// <summary>
        /// This test should be the first one.
        /// </summary>
        [Fact]
        public void NewGame_FirstRun_NextWildCard()
        {
            // The first WildCard should be Ace
            Poker.NewGame(resetWildCard: true);
            Assert.Equal(CardKind.Ace, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Two, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Three, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Four, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Five, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Six, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Seven, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Eight, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Nine, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Ten, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Joker, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Queen, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.King, Poker.WildCard);

            Poker.NewGame();
            // Should go back to Ace after King
            Assert.Equal(CardKind.Ace, Poker.WildCard);

            Poker.NewGame();
            Assert.Equal(CardKind.Two, Poker.WildCard);

            // Ensure the card can be force-restored to Ace
            Poker.NewGame(resetWildCard: true);
            Assert.Equal(CardKind.Ace, Poker.WildCard);
        }

        [Fact]
        public void NewGame_PackCardCount()
        {
            Poker.NewGame();
            Assert.Equal(52, Poker.Count);

            _ = Poker.GetCard();
            Assert.Equal(51, Poker.Count);

            Poker.NewGame();
            Assert.Equal(52, Poker.Count);

            for (int i = 52; i > 0; i--)
            {
                _ = Poker.GetCard();
            }
            Assert.Null(Poker.GetCard());
        }

        [Fact]
        public void NewGame_CannotRepeatAddedCard()
        {
            Poker.NewGame(resetWildCard: true);
            card1 = new Card(CardSuit.Clubs, CardKind.Eight);
            card2 = new Card(CardSuit.Diamonds, CardKind.Eight);
            card3 = new Card(CardSuit.Clubs, CardKind.Eight);
            card4 = new Card(CardSuit.Spades, CardKind.Eight);
            card5 = new Card(CardSuit.Hearts, CardKind.Five);
            Assert.Throws<InvalidOperationException>(() => hand = new Hand(card1, card2, card3, card4, card5));
        }

        [Fact]
        public void Hand_ImpossibleFiveCards()
        {
            // 5 equal cards
            Poker.NewGame(resetWildCard: true);
            card1 = new Card(CardSuit.Spades, CardKind.Ace);
            card2 = new Card(CardSuit.Clubs, CardKind.Ace);
            card3 = new Card(CardSuit.Diamonds, CardKind.Ace);
            card4 = new Card(CardSuit.Spades, CardKind.Ace);
            card5 = new Card(CardSuit.Clubs, CardKind.Ace);
            Assert.Throws<InvalidOperationException>(() => hand = new Hand(card1, card2, card3, card4, card5));
        }

        [Theory]
        [MemberData(nameof(FiveOfAKindData))]
        public void Hand_IsFiveOfAKind(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.True(hand.IsFiveOfAKind);
        }

        [Theory]
        [MemberData(nameof(RoyalFlushData))]
        public void Hand_isRoyalFlush(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.True(hand.IsRoyalFlush);
        }

        [Theory]
        [MemberData(nameof(NotRoyalFlushData))]
        public void Hand_IsNotRoyalFlush(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.False(hand.IsRoyalFlush);
        }

        [Theory]
        [MemberData(nameof(StraightFlushData))]
        public void Hand_IsStraightFlush(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.True(hand.IsStraightFlush);
        }

        [Theory]
        [MemberData(nameof(NotStraightFlushData))]
        public void Hand_IsNotStraightFlush(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.False(hand.IsStraightFlush);
        }

        [Theory]
        [MemberData(nameof(FourOfAKindData))]
        public void Hand_IsFourOfAKind(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.True(hand.IsFourOfAKind);
        }

        [Theory]
        [MemberData(nameof(FullHouseData))]
        public void Hand_IsFullHouse(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.True(hand.IsFullHouse);
        }

        [Theory]
        [MemberData(nameof(NotFullHouseData))]
        public void Hand_IsNotFullHouse(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.False(hand.IsFullHouse);
        }

        [Theory]
        [MemberData(nameof(FlushData))]
        public void Hand_IsFlush(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.True(hand.IsFlush);
        }

        [Theory]
        [MemberData(nameof(StraightData))]
        public void Hand_IsStraight(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.True(hand.IsStraight);
        }

        [Theory]
        [MemberData(nameof(NotStraightData))]
        public void Hand_IsNotStraight(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.False(hand.IsStraight);
        }

        [Theory]
        [MemberData(nameof(StraightWithThreeAsWildCardData))]
        public void Hand_IsStraight_WithThreeAsWildCard(Card card1, Card card2, Card card3, Card card4, Card card5)
        {

            Poker.NewGame(resetWildCard: true); // Ace
            Poker.NewGame(); // Two
            Poker.NewGame(); // Three
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.True(hand.IsStraight);
        }

        [Theory]
        [MemberData(nameof(ThreeOfAKindData))]
        public void Hand_IsThreeOfAKind(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.True(hand.IsThreeOfAKind);
        }

        [Theory]
        [MemberData(nameof(NotThreeOfAKindData))]
        public void Hand_IsNotThreeOfAKind(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.False(hand.IsThreeOfAKind);
        }

        [Theory]
        [MemberData(nameof(TwoPairsData))]
        public void Hand_IsTwoPairs(Card card1, Card card2, Card card3, Card card4, Card card5)
        {
            Poker.NewGame(resetWildCard: true);
            hand = new Hand(card1, card2, card3, card4, card5);
            Assert.True(hand.IsTwoPairs);
        }

        #region Test data

        public static IEnumerable<object[]> FiveOfAKindData()
        {
            // 4 wildcards, 1 non-wildcard
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Eight)
            };

            // 3 wildcards, 2 equal non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Spades, CardKind.Two),
                new Card(CardSuit.Clubs, CardKind.Two)
            };

            // 2 wildcards, 3 equal non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Four),
                new Card(CardSuit.Spades, CardKind.Four),
                new Card(CardSuit.Clubs, CardKind.Four)
            };

            // 1 wildcard, 4 equal non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Five),
                new Card(CardSuit.Diamonds, CardKind.Five),
                new Card(CardSuit.Spades, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Five)
            };
        }

        public static IEnumerable<object[]> RoyalFlushData()
        {
            // All Clubs hand (10,J,Q,K,A) is Royal Flush
            yield return new object[] {
                new Card(CardSuit.Clubs, CardKind.Ten),
                new Card(CardSuit.Clubs, CardKind.Joker),
                new Card(CardSuit.Clubs, CardKind.Queen),
                new Card(CardSuit.Clubs, CardKind.King),
                new Card(CardSuit.Clubs, CardKind.Ace)
            };

            // All Diamonds hand (10,J,Q,K,A) is Royal Flush
            yield return new object[] {
                new Card(CardSuit.Diamonds, CardKind.Ten),
                new Card(CardSuit.Diamonds, CardKind.Joker),
                new Card(CardSuit.Diamonds, CardKind.Queen),
                new Card(CardSuit.Diamonds, CardKind.King),
                new Card(CardSuit.Diamonds, CardKind.Ace)
            };

            // All Hearts hand (10,J,Q,K,A) is Royal Flush
            yield return new object[] {
                new Card(CardSuit.Hearts, CardKind.Ten),
                new Card(CardSuit.Hearts, CardKind.Joker),
                new Card(CardSuit.Hearts, CardKind.Queen),
                new Card(CardSuit.Hearts, CardKind.King),
                new Card(CardSuit.Hearts, CardKind.Ace)
            };

            // All Spades hand (10,J,Q,K,A) is Royal Flush
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ten),
                new Card(CardSuit.Spades, CardKind.Joker),
                new Card(CardSuit.Spades, CardKind.Queen),
                new Card(CardSuit.Spades, CardKind.King),
                new Card(CardSuit.Spades, CardKind.Ace)
            };
        }

        public static IEnumerable<object[]> NotRoyalFlushData()
        {
            // Spades (10,J,Q,K) and Clubs Ace is NOT Royal Flush
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ten),
                new Card(CardSuit.Spades, CardKind.Joker),
                new Card(CardSuit.Spades, CardKind.Queen),
                new Card(CardSuit.Spades, CardKind.King),
                new Card(CardSuit.Clubs, CardKind.Ace)
            };

            // Spades (10,J,Q,K) and Spades Two is NOT Royal Flush
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ten),
                new Card(CardSuit.Spades, CardKind.Joker),
                new Card(CardSuit.Spades, CardKind.Queen),
                new Card(CardSuit.Spades, CardKind.King),
                new Card(CardSuit.Spades, CardKind.Two)
            };
        }

        public static IEnumerable<object[]> StraightFlushData()
        {
            // 4 wildcards, 1 non-wildcard
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Two)
            };

            // 3 wildcards, 2 non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Two),
                new Card(CardSuit.Hearts, CardKind.Three)
            };

            // 2 wildcards, 3 non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Two),
                new Card(CardSuit.Hearts, CardKind.Three),
                new Card(CardSuit.Hearts, CardKind.Four)
            };

            // 1 wildcard, 4 non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Two),
                new Card(CardSuit.Hearts, CardKind.Three),
                new Card(CardSuit.Hearts, CardKind.Four),
                new Card(CardSuit.Hearts, CardKind.Five)
            };

            // 0 wildcards, 5 non-wildcards
            yield return new object[] {
                new Card(CardSuit.Hearts, CardKind.Two),
                new Card(CardSuit.Hearts, CardKind.Three),
                new Card(CardSuit.Hearts, CardKind.Four),
                new Card(CardSuit.Hearts, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Six)
            };

            // Edge case: 0 wildcards, 5 non-wildcards, in the middle
            yield return new object[] {
                new Card(CardSuit.Hearts, CardKind.Four),
                new Card(CardSuit.Hearts, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Six),
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Eight)
            };

            // Edge case: 0 wildcards, 5 non-wildcards, top with Ace
            yield return new object[] {
                new Card(CardSuit.Hearts, CardKind.Ten),
                new Card(CardSuit.Hearts, CardKind.Joker),
                new Card(CardSuit.Hearts, CardKind.Queen),
                new Card(CardSuit.Hearts, CardKind.King),
                new Card(CardSuit.Hearts, CardKind.Ace)
            };
        }

        public static IEnumerable<object[]> NotStraightFlushData()
        {
            // Negative case: 0 wildcards, 5 non-wildcards, spread
            yield return new object[] {
                new Card(CardSuit.Hearts, CardKind.Four),
                new Card(CardSuit.Hearts, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Six),
                new Card(CardSuit.Hearts, CardKind.Eight),
                new Card(CardSuit.Hearts, CardKind.Joker)
            };

            // Negative case: 0 wildcards, 5 non-wildcards, wrong suit
            yield return new object[] {
                new Card(CardSuit.Hearts, CardKind.Four),
                new Card(CardSuit.Hearts, CardKind.Five),
                new Card(CardSuit.Spades, CardKind.Six),
                new Card(CardSuit.Hearts, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Eight)
            };
        }

        public static IEnumerable<object[]> FourOfAKindData()
        {
            // 4 non-wildcards, 1 non-wildcard
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Five),
                new Card(CardSuit.Clubs, CardKind.Five),
                new Card(CardSuit.Diamonds, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Five),
                new Card(CardSuit.Clubs, CardKind.Three)
            };

            // 3 non-wildcards, 1 wildcard, 1 non-wildcard
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Five),
                new Card(CardSuit.Clubs, CardKind.Five),
                new Card(CardSuit.Diamonds, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Four)
            };

            // 2 wildcards, 2 non-wildcards, 1 non-wildcard
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Six),
                new Card(CardSuit.Clubs, CardKind.Six),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Nine)
            };

            // 3 wildcards, 1 non-wildcard, 1 non-wildcard
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Eight),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ten)
            };

            // 4 wildcards, 1 non-wildcard
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Queen)
           };
        }

        public static IEnumerable<object[]> FullHouseData()
        {
            // Four wildcards and any other card
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Spades, CardKind.Eight)
            };

            // Three wildcards and any other two cards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Spades, CardKind.Eight)
            };

            // Two wildcards, one any card and one non-wildcard duo
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Seven),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Spades, CardKind.Eight)
            };

            // Two wildcards and one non-wildcard trio
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Seven),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Spades, CardKind.Seven)
            };

            // One wildcard and two non-wildcard duos
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Seven),
                new Card(CardSuit.Clubs, CardKind.Seven),
                new Card(CardSuit.Diamonds, CardKind.Eight),
                new Card(CardSuit.Spades, CardKind.Eight)
            };

            // One wildcard, one non-wildcard duos and one any other card
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Seven),
                new Card(CardSuit.Clubs, CardKind.Seven),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Spades, CardKind.Nine)
            };

            // Zero wildcards, one non-wildcard duo and one non-wildcard trio
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Seven),
                new Card(CardSuit.Clubs, CardKind.Seven),
                new Card(CardSuit.Diamonds, CardKind.Nine),
                new Card(CardSuit.Spades, CardKind.Nine)
            };
        }

        public static IEnumerable<object[]> NotFullHouseData()
        {
            // Negative case: Zero wildcards, one quartet
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Seven),
                new Card(CardSuit.Clubs, CardKind.Seven),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Spades, CardKind.Nine)
            };

            // Negative case: Zero wildcards, one quartet, one wildcard
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Seven),
                new Card(CardSuit.Clubs, CardKind.Seven),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Spades, Poker.WildCard)
            };
        }

        public static IEnumerable<object[]> FlushData()
        {
            // Zero wildcards, five non-wildcards
            yield return new object[] {
                new Card(CardSuit.Clubs, CardKind.Nine),
                new Card(CardSuit.Clubs, CardKind.Ten),
                new Card(CardSuit.Clubs, CardKind.Six),
                new Card(CardSuit.Clubs, CardKind.Two),
                new Card(CardSuit.Clubs, CardKind.King)
            };

            // One wildcard, four non-wildcards
            yield return new object[] {
                new Card(CardSuit.Clubs, CardKind.Nine),
                new Card(CardSuit.Clubs, CardKind.Ten),
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Two),
                new Card(CardSuit.Clubs, CardKind.King)
            };

            // Two wildcards, three non-wildcards
            yield return new object[] {
                new Card(CardSuit.Clubs, CardKind.Nine),
                new Card(CardSuit.Clubs, CardKind.Ten),
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Two),
                new Card(CardSuit.Hearts, CardKind.Ace)
            };

            // Three wildcards, two non-wildcards
            yield return new object[] {
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ten),
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Two),
                new Card(CardSuit.Hearts, CardKind.Ace)
            };

            // Four wildcards, one non-wildcard
            yield return new object[] {
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ten),
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace)
            };
        }

        public static IEnumerable<object[]> StraightData()
        {
            // 4 wildcards, 1 non-wildcard
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Two)
            };

            // 3 wildcards, 2 non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Two),
                new Card(CardSuit.Diamonds, CardKind.Three)
            };

            // 2 wildcards, 3 non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Spades, CardKind.Two),
                new Card(CardSuit.Hearts, CardKind.Three),
                new Card(CardSuit.Diamonds, CardKind.Four)
            };

            // 1 wildcard, 4 non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Spades, CardKind.Two),
                new Card(CardSuit.Hearts, CardKind.Three),
                new Card(CardSuit.Diamonds, CardKind.Four),
                new Card(CardSuit.Clubs, CardKind.Five)
            };

            // 0 wildcards, 5 non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Two),
                new Card(CardSuit.Hearts, CardKind.Three),
                new Card(CardSuit.Diamonds, CardKind.Four),
                new Card(CardSuit.Hearts, CardKind.Five),
                new Card(CardSuit.Clubs, CardKind.Six)
            };
        }

        public static IEnumerable<object[]> NotStraightData()
        {
            // Negative case: 1 wildcard, 4 non-wildcards, in order
            yield return new object[] {
                new Card(CardSuit.Diamonds, CardKind.Ace), // Could be used as Two or Six
                new Card(CardSuit.Hearts, CardKind.Three),
                new Card(CardSuit.Hearts, CardKind.Four),
                new Card(CardSuit.Clubs, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Eight)
            };

            // Negative case: 0 wildcards, 5 non-wildcards, in order
            yield return new object[] {
                new Card(CardSuit.Diamonds, CardKind.Two),
                new Card(CardSuit.Hearts, CardKind.Three),
                new Card(CardSuit.Hearts, CardKind.Four),
                new Card(CardSuit.Clubs, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Seven)
            };

            // Negative case: 0 wildcards, 5 non-wildcards, spread
            yield return new object[] {
                new Card(CardSuit.Diamonds, CardKind.Four),
                new Card(CardSuit.Hearts, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Two),
                new Card(CardSuit.Clubs, CardKind.Eight),
                new Card(CardSuit.Hearts, CardKind.Joker)
            };
        }

        public static IEnumerable<object[]> StraightWithThreeAsWildCardData()
        {
            // All these test cases assume Three is the WildCard

            // Edge case: 0 wildcards, 5 non-wildcards, wildcard at the beginning
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Three), // Should be used as Seven
                new Card(CardSuit.Hearts, CardKind.Four),
                new Card(CardSuit.Clubs, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Six),
                new Card(CardSuit.Diamonds, CardKind.Eight)
            };

            // Edge case: 0 wildcards, 5 non-wildcards, in the middle
            yield return new object[] {
                new Card(CardSuit.Hearts, CardKind.Four),
                new Card(CardSuit.Clubs, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Six),
                new Card(CardSuit.Spades, CardKind.Three),
                new Card(CardSuit.Diamonds, CardKind.Eight)
            };

            // Edge case: 0 wildcards, 5 non-wildcards, top with Ace
            yield return new object[] {
                new Card(CardSuit.Hearts, CardKind.Ten),
                new Card(CardSuit.Diamonds, CardKind.Joker),
                new Card(CardSuit.Spades, CardKind.Queen),
                new Card(CardSuit.Clubs, CardKind.King),
                new Card(CardSuit.Hearts, CardKind.Ace) // Not wildcard - its the next card after King
            };
        }

        public static IEnumerable<object[]> ThreeOfAKindData()
        {
            // 4 wildcards, 1 non-wildcard
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Five)
            };

            // 3 wildcards, 2 non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Six),
                new Card(CardSuit.Diamonds, CardKind.Five)
            };

            // 2 wildcards, 3 non-wildcards
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Six),
                new Card(CardSuit.Diamonds, CardKind.Five)
            };

            // 1 wildcard, 4 non-wildcard (two equal)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Seven),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Six),
                new Card(CardSuit.Diamonds, CardKind.Five)
            };

            // 0 wildcards, 5 non-wildcard (three equal)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Seven),
                new Card(CardSuit.Clubs, CardKind.Seven),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Six),
                new Card(CardSuit.Diamonds, CardKind.Five)
            };
        }

        public static IEnumerable<object[]> NotThreeOfAKindData()
        {

            // 1 wildcard, 4 non-wildcard (all different)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Eight),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Six),
                new Card(CardSuit.Diamonds, CardKind.Five)
            };

            // 0 wildcards, 5 non-wildcard (two equal)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Eight),
                new Card(CardSuit.Clubs, CardKind.Eight),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Six),
                new Card(CardSuit.Diamonds, CardKind.Five)
            };

            // 0 wildcards, 5 non-wildcard (all different)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Nine),
                new Card(CardSuit.Clubs, CardKind.Eight),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Six),
                new Card(CardSuit.Diamonds, CardKind.Five)
            };
        }

        public static IEnumerable<object[]> TwoPairsData()
        {
            // 4 wildcards, 1 non-wildcard (must match color with one wildcard)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Ace),
                new Card(CardSuit.Spades, CardKind.Three)
            };

            // 3 wildcards (at least one red and at least one black), 2 non-wildcards (at least one red and at least one black)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Three),
                new Card(CardSuit.Spades, CardKind.Five)
            };

            // 2 wildcards (1 red, 1 black), 3 non-wildcards (at least 1 red and at least 1 black)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Diamonds, CardKind.Ace),
                new Card(CardSuit.Hearts, CardKind.Three),
                new Card(CardSuit.Diamonds, CardKind.Five),
                new Card(CardSuit.Spades, CardKind.Seven)
            };

            // 1 wildcard, 4 non-wildcards (1 pair)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Ace),
                new Card(CardSuit.Clubs, CardKind.Three), // Should be paired with the wildcard
                new Card(CardSuit.Diamonds, CardKind.Five),
                new Card(CardSuit.Hearts, CardKind.Seven),
                new Card(CardSuit.Spades, CardKind.Seven)
            };

            // 0 wildcards, 5 non-wildcards (1 black pair, 1 red pair)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Three),
                new Card(CardSuit.Clubs, CardKind.Three),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Seven),
                new Card(CardSuit.Spades, CardKind.Joker)
            };

            // 0 wildcards, 5 non-wildcards (2 black pairs)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Two),
                new Card(CardSuit.Clubs, CardKind.Two),
                new Card(CardSuit.Spades, CardKind.Three),
                new Card(CardSuit.Clubs, CardKind.Three),
                new Card(CardSuit.Diamonds, CardKind.Four)
            };

            // 0 wildcards, 5 non-wildcards (2 red pairs)
            yield return new object[] {
                new Card(CardSuit.Spades, CardKind.Three),
                new Card(CardSuit.Hearts, CardKind.Seven),
                new Card(CardSuit.Diamonds, CardKind.Seven),
                new Card(CardSuit.Hearts, CardKind.Eight),
                new Card(CardSuit.Diamonds, CardKind.Eight)
            };
        }

        #endregion
    }
}