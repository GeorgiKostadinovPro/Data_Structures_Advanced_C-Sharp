namespace RoyaleArena
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;

    public class Arena : IArena
    {
        private readonly IDictionary<int, BattleCard> cards;

        public Arena()
        {
            this.cards = new Dictionary<int, BattleCard>();
        }

        public int Count => this.cards.Count;

        public void Add(BattleCard card)
        {
            if (!this.Contains(card))
            {
                this.cards.Add(card.Id, card);
            }
        }

        public void ChangeCardType(int id, CardType type)
        {
            BattleCard battleCard =this.GetById(id);

            if (battleCard == null)
            {
                throw new InvalidOperationException();
            }

            if (type != CardType.MELEE
                && type != CardType.RANGED
                && type != CardType.SPELL
                && type != CardType.BUILDING)
            {
                throw new InvalidOperationException();
            }

            battleCard.Type = type;
        }

        public bool Contains(BattleCard card)
        {
            return this.cards.ContainsKey(card.Id);
        }

        public IEnumerable<BattleCard> FindFirstLeastSwag(int n)
        {
            if (n > this.Count)
            {
                throw new InvalidOperationException();
            }

            var battleCardsWithLeastSwag = this.cards.Values
                .OrderBy(c => c.Swag)
                .ThenBy(c => c.Id)
                .Take(n)
                .ToArray();

            return battleCardsWithLeastSwag;
        }

        public IEnumerable<BattleCard> GetAllInSwagRange(double lo, double hi)
        {
            var battleCardInSwagRange = this.cards.Values
                .Where(c => c.Swag >= lo && c.Swag <= hi)
                .OrderBy(c => c.Swag)
                .ToArray();

            return battleCardInSwagRange;
        }

        public IEnumerable<BattleCard> GetByCardType(CardType type)
        {
            var battleCardsWithGivenType = this.cards.Values
                .Where(c => c.Type == type)
                .OrderByDescending(c => c.Damage)
                .ThenBy(c => c.Id)
                .ToArray();

            if (battleCardsWithGivenType.Length == 0)
            {
                throw new InvalidOperationException();
            }

            return battleCardsWithGivenType;
        }

        public IEnumerable<BattleCard> GetByCardTypeAndMaximumDamage(CardType type, double damage)
        {
            var battleCards = this.cards.Values
                .Where(c => c.Type == type && c.Damage <= damage)
                .OrderByDescending(c => c.Damage)
                .ThenBy(c => c.Id)
                .ToArray();

            if (battleCards.Length == 0)
            {
                throw new InvalidOperationException();
            }

            return battleCards;
        }

        public BattleCard GetById(int id)
        {
            try
            {
                BattleCard battleCard = this.cards[id];
                return battleCard;
            }
            catch (KeyNotFoundException)
            {
                throw new InvalidOperationException();
            }
        }

        public IEnumerable<BattleCard> GetByNameAndSwagRange(string name, double lo, double hi)
        {
            var battleCards = this.cards.Values
                .Where(c => c.Name == name && c.Swag >= lo && c.Swag < hi)
                .OrderByDescending(c => c.Swag)
                .ThenBy(c => c.Id)
                .ToArray();

            if (battleCards.Length == 0)
            {
                throw new InvalidOperationException();
            }

            return battleCards;
        }

        public IEnumerable<BattleCard> GetByNameOrderedBySwagDescending(string name)
        {
            var battleCards = this.cards.Values
                .Where(c => c.Name == name)
                .OrderByDescending(c => c.Swag)
                .ThenBy(c => c.Id)
                .ToArray();

            if (battleCards.Length == 0)
            {
                throw new InvalidOperationException();
            }

            return battleCards;
        }

        public IEnumerable<BattleCard> GetByTypeAndDamageRangeOrderedByDamageThenById(CardType type, int lo, int hi)
        {
            var battleCards = this.cards.Values
                .Where(c => c.Type == type && c.Damage >= lo && c.Damage <= hi)
                .OrderByDescending(c => c.Damage)
                .ThenBy(c => c.Id)
                .ToArray();

            if (battleCards.Length == 0)
            {
                throw new InvalidOperationException();
            }

            return battleCards;
        }

        public void RemoveById(int id)
        {
            BattleCard battleCard = this.GetById(id);

            if (battleCard == null)
            {
                throw new InvalidOperationException();
            }

            this.cards.Remove(id);
        }
        
        public IEnumerator<BattleCard> GetEnumerator()
        {
            foreach (var battleCard in this.cards)
            {
                yield return battleCard.Value;
            }
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }
    }
}