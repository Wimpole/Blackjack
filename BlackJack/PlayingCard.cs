using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class PlayingCard
    {
        public int Id { get; set; }
        public CardSuit CardSuit { get; private set; }
        public int Value { get; private set; }

        public PlayingCard (CardSuit cardSuit, int value)
        {
            this.CardSuit = cardSuit;
            this.Value = value;
        }
        public override string ToString()
        {
            string cardString = "";

            switch(this.CardSuit)
            {
                case CardSuit.Hearts:
                    cardString += "♥";
                    break;
                case CardSuit.Diamonds:
                    cardString += "♦";
                    break;
                case CardSuit.Clubs:
                    cardString += "♣";
                    break;
                case CardSuit.Spades:
                    cardString += "♠";
                    break;
            }

            if (this.Value == 1)
                cardString += "A";
            else if (this.Value > 10)
            {
                switch (this.Value)
                {
                    case 11:
                        cardString += "J";
                        break;
                    case 12:
                        cardString += "Q";
                        break;
                    case 13:
                        cardString += "K";
                        break;
                }
            }
            else
                cardString += this.Value;

            return cardString;
        }
    }
}
