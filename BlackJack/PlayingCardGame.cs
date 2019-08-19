using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Medallion;

namespace BlackJack
{
    public class PlayingCardGame
    {
        private Player _currentPlayer;

        private Player _dealer = new Player();

        private PlayingCardDeck _activeDeck = new PlayingCardDeck();

        private DataAccess _dataAccess = new DataAccess();
        public void Run()
        {
            Console.OutputEncoding = Encoding.UTF8;

            Console.BackgroundColor = ConsoleColor.DarkGreen;
            Console.ForegroundColor = ConsoleColor.White;
            Console.Clear();

            _activeDeck = ShuffleDeck(_activeDeck);

            while(true)
            {
                MenuChoice menuChoice = ShowMainMenu();

                if (menuChoice == MenuChoice.PlayGame)
                {
                    PlayerSelectionScreen();

                    Console.Clear();

                    while (true)
                    {
                        LetPlayerPlaceBet();

                        DealHand(_activeDeck, _currentPlayer, _dealer);

                        PlayerPlayHand();
                        
                        if (_currentPlayer.HandValue <= 21)
                            PlayDealerHand();

                        HandResult handResult = CheckHandResult();

                        DisplayHandResult(handResult);

                        UpdatePlayerChipCount(handResult);

                        _dataAccess.UpdatePlayer(_currentPlayer);

                        ClearHand();

                        if (_activeDeck.PlayingCards.Count < 10)
                            ReshuffleDeck();

                        if (!PlayAnotherHand())
                            break;
                    }
                }
                else if (menuChoice == MenuChoice.ShowHighscore)
                    ShowHighscore();
                else if (menuChoice == MenuChoice.ExitGame)
                {
                    ShowExitScreen();
                    break;
                }
            }
        }

        private void PlayerPlayHand()
        {
            while (true)
            {
                DisplayPlayerHand();

                PlayerAction playerAction = new PlayerAction();

                if (_currentPlayer.HandValue != 21)
                    playerAction = GetPlayerAction();
                else
                    playerAction = PlayerAction.Stand;


                if (playerAction == PlayerAction.Stand)
                    break;
                else if (playerAction == PlayerAction.Hit)
                {
                    _currentPlayer.CurrentHand.Add(_activeDeck.GetCardFromDeck());
                    //Hit(_activeDeck, _currentPlayer);

                    if (_currentPlayer.HandValue > 21)
                        break;

                }
            }
        }

        private MenuChoice ShowMainMenu()
        {
            Console.Clear();

            PrintBlackjackHeader();

            Console.WriteLine();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("-=#|   Main  menu   |#=-".PadLeft(72));
            Console.WriteLine();

            Console.WriteLine("1) Play the game".PadLeft(67));
            Console.WriteLine("2) View highscores".PadLeft(69));
            Console.WriteLine("3) Exit game".PadLeft(63));
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.White;

            Console.SetCursorPosition(51, 14);

            while(true)
            {
                ConsoleKey menuChoice = Console.ReadKey(true).Key;

                switch(menuChoice)
                {
                    case ConsoleKey.D1:
                        return MenuChoice.PlayGame;
                    case ConsoleKey.D2:
                        return MenuChoice.ShowHighscore;
                    case ConsoleKey.D3:
                        return MenuChoice.ExitGame;
                }
            }
        }

        private void WriteRed(string s)
        {
            Console.ForegroundColor = ConsoleColor.DarkRed;
            Console.Write(s);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void WriteBlack(string s)
        {
            Console.ForegroundColor = ConsoleColor.Black;
            Console.Write(s);
            Console.ForegroundColor = ConsoleColor.White;
        }

        private void ShowExitScreen()
        {
            Console.Clear();

            Console.WriteLine("Thanks for playing Blackjack.");

            Console.WriteLine();
            Console.WriteLine("Press any key to exit the game");

            Console.ReadKey(true);
        }

        private void ShowHighscore()
        {
            List<Player> highscorePlayers = _dataAccess.GetTopTenPlayers();

            Console.Clear();

            Console.Write("              ");
            WriteBlack(@" __    __   ");
            WriteRed(@"__    ");
            WriteBlack(@"_______  ");
            WriteRed(@"__    __       ");
            WriteBlack(@"_______.  ");
            WriteRed(@"______   ");
            WriteBlack(@"______   ");
            WriteRed(@".______       ");
            WriteBlack(@"_______ ");
            Console.WriteLine();

            Console.Write("              ");
            WriteBlack(@"|  |  |  | ");
            WriteRed(@"|  |  ");
            WriteBlack(@"/  _____|");
            WriteRed(@"|  |  |  |     ");
            WriteBlack(@"/       | ");
            WriteRed(@"/      | ");
            WriteBlack(@"/  __  \  ");
            WriteRed(@"|   _  \     ");
            WriteBlack(@"|   ____|");
            Console.WriteLine();

            Console.Write("              ");
            WriteBlack(@"|  |__|  | ");
            WriteRed(@"|  | ");
            WriteBlack(@"|  |  __  ");
            WriteRed(@"|  |__|  |    ");
            WriteBlack(@"|   (----`");
            WriteRed(@"|  ,----'");
            WriteBlack(@"|  |  |  | ");
            WriteRed(@"|  |_)  |    ");
            WriteBlack(@"|  |__   ");
            Console.WriteLine();


            Console.Write("              ");
            WriteBlack(@"|   __   | ");
            WriteRed(@"|  | ");
            WriteBlack(@"|  | |_ | ");
            WriteRed(@"|   __   |     ");
            WriteBlack(@"\   \    ");
            WriteRed(@"|  |     ");
            WriteBlack(@"|  |  |  | ");
            WriteRed(@"|      /     ");
            WriteBlack(@"|   __|  ");
            Console.WriteLine();

            Console.Write("              ");
            WriteBlack(@"|  |  |  | ");
            WriteRed(@"|  | ");
            WriteBlack(@"|  |__| | ");
            WriteRed(@"|  |  |  | ");
            WriteBlack(@".----)   |   ");
            WriteRed(@"|  `----.");
            WriteBlack(@"|  `--'  | ");
            WriteRed(@"|  |\  \----.");
            WriteBlack(@"|  |____ ");
            Console.WriteLine();

            Console.Write("              ");
            WriteBlack(@"|__|  |__| ");
            WriteRed(@"|__|  ");
            WriteBlack(@"\______| ");
            WriteRed(@"|__|  |__| ");
            WriteBlack(@"|_______/     ");
            WriteRed(@"\______| ");
            WriteBlack(@"\______/  ");
            WriteRed(@"| _| `._____|");
            WriteBlack(@"|_______|");
            Console.WriteLine();

            Console.WriteLine();
            Console.WriteLine();

            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine("".PadLeft(39) + "Position".PadRight(10) + "Name".PadRight(10) + "Chips".PadRight(10) + "Games played");
            for (int i = 0; i<highscorePlayers.Count; i++)
            {
                Console.WriteLine("".PadLeft(39) + $"{i+1}.".PadRight(10) + highscorePlayers[i].Name.PadRight(10) + highscorePlayers[i].TotalChips.ToString().PadRight(10) + highscorePlayers[i].GamesPlayed);
            }

            Console.WriteLine();
            Console.WriteLine("".PadLeft(39) + "Press any key to return to main menu");

            Console.ForegroundColor = ConsoleColor.White;

            Console.ReadKey();
        }


    private bool PlayAnotherHand()
        {
            Console.SetCursorPosition(39, 24);

            Console.WriteLine("Would you like to play another hand? (y/n)");

            while(true)
            {
                Console.SetCursorPosition(59, 25);
                ConsoleKey userChoice = Console.ReadKey(true).Key;

                if (userChoice == ConsoleKey.Y)
                    return true;
                else if (userChoice == ConsoleKey.N)
                    return false;
            }
        }


        private void UpdatePlayerChipCount(HandResult handResult)
        {
            if(handResult == HandResult.Blackjack)
            {
                _currentPlayer.TotalChips += (int)Math.Round(_currentPlayer.CurrentBet * 1.5);
            }
            else if (handResult == HandResult.PlayerBust || handResult == HandResult.Loss)
            {
                _currentPlayer.TotalChips -= _currentPlayer.CurrentBet;
            }
            else if(handResult == HandResult.Win || handResult == HandResult.DealerBust)
            {
                _currentPlayer.TotalChips += _currentPlayer.CurrentBet;
            }

            if (_currentPlayer.TotalChips == 0)
                GivePlayerNewChips();
        }

        private void GivePlayerNewChips()
        {
            Console.WriteLine("Oops. You managed to lose all your chips. Here are 250 new ones to keep you in the game");
            _currentPlayer.TotalChips = 250;
            Console.WriteLine();
        }

        private void PlayerSelectionScreen()
        {
            Console.Clear();

            PrintBlackjackHeader();

            Console.WriteLine();

            Console.WriteLine("".PadLeft(35) + "Welcome to the Blackjack table. What is your name?");

            Console.SetCursorPosition(55,9);
            string playerName = Console.ReadLine();

            _currentPlayer = _dataAccess.GetPlayerByName(playerName);

            Console.Clear();

            if(_currentPlayer.GamesPlayed == 0)
                Console.WriteLine($"A pleasure to meet you, {_currentPlayer.Name}. Here's 500 chips to get you started!");
            else
                Console.WriteLine($"Welcome back {_currentPlayer.Name}. You currently have {_currentPlayer.TotalChips} chips to play with. You have now played {_currentPlayer.GamesPlayed + 1} game(s)");

            _currentPlayer.GamesPlayed++;

            Console.WriteLine();
            Console.WriteLine("Press any key to get started!");
            Console.ReadKey(true);
        }

        private void LetPlayerPlaceBet()
        {
            Console.WriteLine($"Place your bet please. You currently have {_currentPlayer.TotalChips} to play for");

            while(true)
            {
                string inputBet = Console.ReadLine();

                if(!int.TryParse(inputBet, out int playerBet))
                {
                    Console.WriteLine("Input was not a number. Try again.");
                }
                else if (playerBet > _currentPlayer.TotalChips)
                {
                    Console.WriteLine($"That bet is more than your current chip count. Please place a lower bet. Your current chip count is {_currentPlayer.TotalChips}");
                }
                else
                {
                    _currentPlayer.CurrentBet = playerBet;
                    break;
                }
            }
        }

        private void ReshuffleDeck()
        {
            _activeDeck = new PlayingCardDeck();
            _activeDeck = ShuffleDeck(_activeDeck);

            Console.WriteLine("The deck was reshuffled!");
        }

        private void ClearHand()
        {
            _currentPlayer.CurrentHand = new List<PlayingCard>();
            _dealer.CurrentHand = new List<PlayingCard>();
            _currentPlayer.CurrentBet = 0;
        }

        private void DisplayHandResult(HandResult handResult)
        {
            Console.SetCursorPosition(40, 9);

            if(handResult == HandResult.PlayerBust)
                Console.WriteLine($"You were busted! You lost your bet of {_currentPlayer.CurrentBet} chips.");
            else if(handResult == HandResult.DealerBust)
                Console.WriteLine($"The dealer was busted. You win {_currentPlayer.CurrentBet} chips!");
            else if (handResult == HandResult.Push)
                Console.WriteLine($"It's a push! You get your bet {_currentPlayer.CurrentBet} back.");
            else if (handResult == HandResult.Blackjack)
                Console.WriteLine($"Blackjack. You win {_currentPlayer.CurrentBet * 1.5} chips!");
            else if (handResult == HandResult.Loss)
                Console.WriteLine($"You lost! {_currentPlayer.CurrentBet} will be taken from you.");
            else if (handResult == HandResult.Win)
                Console.WriteLine($"You win {_currentPlayer.CurrentBet} fresh shiny chips.");

            Console.WriteLine();
        }

        private void PlayDealerHand()
        {
            while(true)
            {
                DisplayPlayerHand();

                int handValue = _dealer.HandValue;
                int hardValue = 0;

                foreach (PlayingCard card in _dealer.CurrentHand)
                {
                    hardValue += card.Value;
                }

                if(handValue < 17 || handValue == 17 && hardValue < 17)
                {
                    _dealer.CurrentHand.Add(_activeDeck.GetCardFromDeck());
                    //Hit(_activeDeck, _dealer);
                }
                else
                {
                    break;
                }

                Thread.Sleep(2000);
            }
        }

        public HandResult CheckHandResult()
        {
            DisplayPlayerHand();

            int playerResult = _currentPlayer.HandValue;
            int dealerResult = _dealer.HandValue;

            if (playerResult > 21)
                return HandResult.PlayerBust;
            else if (dealerResult > 21)
                return HandResult.DealerBust;
            else if (dealerResult > playerResult)
                return HandResult.Loss;
            else if (dealerResult == 21 && playerResult == 21 && _dealer.CurrentHand.Count == 2 && _currentPlayer.CurrentHand.Count > 2)
                return HandResult.Loss;
            else if (dealerResult == 21 && playerResult == 21 && _dealer.CurrentHand.Count > 2 && _currentPlayer.CurrentHand.Count == 2)
                return HandResult.Blackjack;
            else if (dealerResult == playerResult)
                return HandResult.Push;
            else
            {
                if (playerResult == 21 && _currentPlayer.CurrentHand.Count == 2)
                    return HandResult.Blackjack;
                else
                    return HandResult.Win;
            }
            
            
        }

        public PlayerAction GetPlayerAction()
        {
            Console.WriteLine("Would you like to (h)it, or (s)tand?".PadLeft(78));
            while (true)
            {
                ConsoleKey action = Console.ReadKey(true).Key;

                switch (action)
                {
                    case ConsoleKey.H:
                        return PlayerAction.Hit;
                    case ConsoleKey.S:
                        return PlayerAction.Stand;
                }
            }
        }

        public void DisplayPlayerHand()
        {
            Console.Clear();

            Console.WriteLine();
            Console.WriteLine("Dealer".PadLeft(63));
            Console.WriteLine();

            PrintPlayerHand(_dealer);

            Console.WriteLine();
            Console.WriteLine("Value: ".PadLeft(62) + _dealer.HandValue);

            Console.WriteLine("\n\n\n\n\n\n");

            Console.WriteLine("Player".PadLeft(63));
            Console.WriteLine();

            PrintPlayerHand(_currentPlayer);


            Console.WriteLine();
            Console.WriteLine("Value: ".PadLeft(62) + _currentPlayer.HandValue);


            Console.WriteLine();
            Console.WriteLine("Current bet: ".PadLeft(65) + _currentPlayer.CurrentBet);
            Console.WriteLine();


            Console.WriteLine();
            Console.WriteLine();
            Console.WriteLine("Cards left in deck: ".PadLeft(70) + _activeDeck.PlayingCards.Count);

            Console.WriteLine();
        }

        private void PrintPlayerHand(Player player)
        {
            Console.Write("".PadLeft(55));

            foreach (PlayingCard card in player.CurrentHand)
            {
                Console.Write("[");

                if (card.CardSuit == CardSuit.Diamonds || card.CardSuit == CardSuit.Hearts)
                    Console.ForegroundColor = ConsoleColor.DarkRed;
                else
                    Console.ForegroundColor = ConsoleColor.Black;

                Console.Write(card.ToString());

                Console.ForegroundColor = ConsoleColor.White;

                Console.Write("] ");
            }
        }

        public void DealHand(PlayingCardDeck activeDeck, Player player, Player dealer)
        {
            for (int i = 0; i < 2; i++)
            {
                player.CurrentHand.Add(activeDeck.PlayingCards.First());
                activeDeck.PlayingCards.RemoveAt(0);
            }

            dealer.CurrentHand.Add(activeDeck.PlayingCards.First());
            activeDeck.PlayingCards.RemoveAt(0);

        }

        public PlayingCardDeck ShuffleDeck(PlayingCardDeck deck)
        {
            deck.PlayingCards = deck.PlayingCards.Shuffled().ToList();

            return deck;
        }

        //public void Hit(PlayingCardDeck deck, Player player)
        //{
        //    player.CurrentHand.Add(deck.PlayingCards[0]);
        //    deck.PlayingCards.RemoveAt(0);
        //    // Skapa en metod för att ta bort kortet istället. Skyddar listan mot angrepp
        //}

        public void PrintBlackjackHeader()
        {
            Console.WriteLine();

            Console.Write("\t       ");
            WriteBlack(@".______    ");
            WriteRed(@"__          ");
            WriteBlack(@"___       ");
            WriteRed(@"______  ");
            WriteBlack(@"__  ___        ");
            WriteRed(@"__       ");
            WriteBlack(@"___       ");
            WriteRed(@"______  ");
            WriteBlack(@"__  ___");
            Console.WriteLine();

            Console.Write("\t       ");
            WriteBlack(@"|   _  \  ");
            WriteRed(@"|  |        ");
            WriteBlack(@"/   \     ");
            WriteRed(@"/      |");
            WriteBlack(@"|  |/  /       ");
            WriteRed(@"|  |     ");
            WriteBlack(@"/   \     ");
            WriteRed(@"/      |");
            WriteBlack(@"|  |/  / ");
            Console.WriteLine();

            Console.Write("\t       ");
            WriteBlack(@"|  |_)  | ");
            WriteRed(@"|  |       ");
            WriteBlack(@"/  ^  \   ");
            WriteRed(@"|  ,----'");
            WriteBlack(@"|  '  /        ");
            WriteRed(@"|  |    ");
            WriteBlack(@"/  ^  \   ");
            WriteRed(@"|  ,----'");
            WriteBlack(@"|  '  /");
            Console.WriteLine();

            Console.Write("\t       ");
            WriteBlack(@"|   _  <  ");
            WriteRed(@"|  |      ");
            WriteBlack(@"/  /_\  \  ");
            WriteRed(@"|  |     ");
            WriteBlack(@"|    <   ");
            WriteRed(@".--.  |  |   ");
            WriteBlack(@"/  /_\  \  ");
            WriteRed(@"|  |     ");
            WriteBlack(@"|    <");
            Console.WriteLine();

            Console.Write("\t       ");
            WriteBlack(@"|  |_)  | ");
            WriteRed(@"|  `----.");
            WriteBlack(@"/  _____  \ ");
            WriteRed(@"|  `----.");
            WriteBlack(@"|  .  \  ");
            WriteRed(@"|  `--'  |  ");
            WriteBlack(@"/  _____  \ ");
            WriteRed(@"|  `----.");
            WriteBlack(@"|  .  \");
            Console.WriteLine();

            Console.Write("\t       ");
            WriteBlack(@"|______/  ");
            WriteRed(@"|_______");
            WriteBlack(@"/__/     \__\ ");
            WriteRed(@"\______|");
            WriteBlack(@"|__|\__\  ");
            WriteRed(@"\______/  ");
            WriteBlack(@"/__/     \__\ ");
            WriteRed(@"\______|");
            WriteBlack(@"|__|\__\ ");
            Console.WriteLine();

        }
    }
}
