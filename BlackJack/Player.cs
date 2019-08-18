using System.Collections.Generic;

namespace BlackJack
{
    public class Player
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public List<PlayingCard> CurrentHand { get; set; }
        public int CurrentBet { get; set; }
        public int TotalChips { get; set; }
        public int GamesPlayed { get; set; }


        public int HandValue
        {
            get
            {
                int handValue = 0;
                int numberOfAces = 0;

                foreach (PlayingCard card in this.CurrentHand)
                {
                    if (card.Value >= 10)
                        handValue += 10;
                    else if (card.Value == 1)
                        handValue += 11;
                    else
                        handValue += card.Value;

                    if (card.Value == 1)
                        numberOfAces++;
                }

                int acesToSkip = 1;
                int acesSkipped = 0;

                while (handValue > 21 && numberOfAces > acesSkipped)
                {
                    acesSkipped = 0;
                    handValue = 0;

                    for (int i = 0; i < this.CurrentHand.Count; i++)
                    {
                        if (this.CurrentHand[i].Value >= 10)
                            handValue += 10;
                        else if (this.CurrentHand[i].Value == 1 && acesToSkip > acesSkipped)
                        {
                            handValue += 1;
                            acesSkipped++;
                        }
                        else if (this.CurrentHand[i].Value == 1)
                            handValue += 11;
                        else
                            handValue += this.CurrentHand[i].Value;
                    }

                    acesToSkip++;
                }

                return handValue;
            }

            set { }
        }

        public Player()
        {
            this.CurrentHand = new List<PlayingCard>();
        }
    }
}