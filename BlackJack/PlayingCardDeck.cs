using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class PlayingCardDeck
    {
        public List<PlayingCard> PlayingCards { get; set; } = new List<PlayingCard>();

        public PlayingCard GetCardFromDeck()
        {
            if (PlayingCards.Count == 0)
                throw new ArgumentException();

            PlayingCard card = this.PlayingCards[0];

            this.PlayingCards.RemoveAt(0);

            return card;
        }

        //public void PlaceCardInBottomOfDeck(PlayingCard card)
        //{
        //    PlayingCards.Add(card);
        //}

        public PlayingCardDeck()
        {
            for (int a = 0; a < 4; a++)
            {
                for (int b = 1; b <= 13; b++)
                {
                    PlayingCard card = new PlayingCard((CardSuit)a, b);
                    PlayingCards.Add(card);
                }
            }
        }
    }
}
