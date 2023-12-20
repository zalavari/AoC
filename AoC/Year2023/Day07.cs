using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;

namespace AoC.Year2023
{
    internal class Day07 : ISolvable
    {
        private enum HandType
        {
            None,
            HighCard,
            OnePair,
            TwoPairs,
            ThreeOfAKind,
            FullHouse,
            FourOfAKind,
            FiveOfAKind,
        }

        private class Hand : IComparable<Hand>
        {
            public string CardsOriginal { get; set; }
            public string Cards { get; set; }
            public long Bid { get; set; }
            public HandType HandType { get; set; }

            public int CompareTo([AllowNull] Hand other)
            {
                if (other == null)
                {
                    return 1;
                }

                if (HandType != other.HandType)
                {
                    return HandType.CompareTo(other.HandType);
                }

                return Cards.CompareTo(other.Cards);
            }
        }

        public void Solve(string path)
        {
            Console.WriteLine(path);
            var lines = File.ReadAllLines(path);
            var hands = new List<Hand>();

            foreach (var line in lines)
            {
                var arr = line.Split();
                var hand = new Hand()
                {
                    CardsOriginal = arr[0], // Only for debugging
                    Cards = arr[0]
                        .Replace("A", "E")
                        .Replace("K", "D")
                        .Replace("Q", "C")
                        .Replace("J", "1") // Comment this line out for part 1
                        .Replace("J", "B")
                        .Replace("T", "A"),
                    Bid = long.Parse(arr[1]),
                };

                CalculateHandType(hand);
                hands.Add(hand);
            }

            hands.Sort();

            var rank = 1;
            var winnings = 0L;
            foreach (var hand in hands)
            {
                var winning = rank++ * hand.Bid;
                winnings += winning;
                //Console.WriteLine($"{hand.CardsOriginal}\t{hand.HandType}\t{hand.Bid}\t{winning}");
            }

            Console.WriteLine(winnings);
        }

        private void CalculateHandType(Hand hand)
        {
            var cards = hand.Cards
                .Replace("1", "")
                .ToList();
            var cardCounts = new Dictionary<char, int>();
            foreach (var card in cards)
            {
                if (!cardCounts.ContainsKey(card))
                {
                    cardCounts.Add(card, 0);
                }

                cardCounts[card]++;
            }

            var cardCountValues = cardCounts.Values.ToList();
            cardCountValues.Sort();

            if (cardCountValues.Count == 0 || cardCountValues.Count == 1)
            {
                hand.HandType = HandType.FiveOfAKind;
            }
            else if (cardCountValues.Count == 2)
            {
                if (cardCountValues[0] == 1)
                {
                    hand.HandType = HandType.FourOfAKind;
                }
                else
                {
                    hand.HandType = HandType.FullHouse;
                }
            }
            else if (cardCountValues.Count == 3)
            {
                if (cardCountValues[0] == 1 && cardCountValues[1] == 1)
                {
                    hand.HandType = HandType.ThreeOfAKind;
                }
                else
                {
                    hand.HandType = HandType.TwoPairs;
                }
            }
            else if (cardCountValues.Count == 4)
            {
                hand.HandType = HandType.OnePair;
            }
            else
            {
                hand.HandType = HandType.HighCard;
            }
        }
    }
}
