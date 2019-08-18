using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlackJack
{
    public class DataAccess
    {
        private BlackjackContext _context { get; set; }

        public DataAccess()
        {
            _context = new BlackjackContext();
        }

        public Player GetPlayerByName(string playerName)
        {
            Player player = _context.Players.FirstOrDefault(x => x.Name == playerName);

            if (player == null)
            {
                player = new Player
                {
                    Name = playerName,
                    TotalChips = 500,
                    GamesPlayed = 0
                };

                _context.Add(player);
                _context.SaveChanges();
            }

            return player;
        }

        internal void UpdatePlayer(Player player)
        {
            _context.Update(player);
            _context.SaveChanges();
        }

        internal List<Player> GetTopTenPlayers()
        {
            return _context.Players.OrderByDescending(x => x.TotalChips).Take(10).ToList();
        }
    }
}
