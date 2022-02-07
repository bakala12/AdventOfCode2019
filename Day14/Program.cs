using System;
using System.IO;
using System.Collections.Generic;

namespace Day14
{
    public class Program
    {
        static void Main()
        {   
            var reactions = ParseReactions(File.ReadAllLines("input.txt"));
            var order = FindOrder(reactions);
            var (cost, leftovers) = FindSingleFuelUnitCost(reactions, order);
            Console.WriteLine(cost);
            var fuelUnits = HowMuchCanProduceWithGivenOres(reactions, leftovers, cost, 1_000_000_000_000);
            Console.WriteLine(fuelUnits);
        }

        private static long HowMuchCanProduceWithGivenOres(Dictionary<string, Reaction> reactions, List<Reagent> unitLeftovers, long unitCost, long availableOres)
        {
            var fuelLowerBound = availableOres / unitCost;
            var fuelUpperBound = availableOres;
            while(fuelLowerBound < fuelUpperBound-1)
            {
                long fuelTested = (fuelLowerBound + fuelUpperBound) / 2;
                var remaining = reactions.ToDictionary(r => r.Key, r => 0L);
                remaining.Add("ORE", availableOres);
                if(TryProduceFromLeftovers(reactions, remaining, "FUEL", fuelTested))
                    fuelLowerBound = fuelTested;
                else
                    fuelUpperBound = fuelTested;
            }
            return fuelLowerBound;
        }

        private static bool TryProduceFromLeftovers(Dictionary<string, Reaction> reactions, Dictionary<string, long> remaining, string item, long quantity)
        {
            var got = remaining[item];
            if(got >= quantity)
            {
                remaining[item] -= quantity;
                return true;
            }
            if(item == "ORE")
                return false;
            var stillNeeded = quantity - got;
            remaining[item] = 0;
            var reaction = reactions[item];
            var producedInSingleReaction = reaction.Product.Quantity;
            var times = stillNeeded / producedInSingleReaction + (stillNeeded % producedInSingleReaction != 0 ? 1 : 0);
            foreach(var sub in reaction.Substrats)
                if(!TryProduceFromLeftovers(reactions, remaining, sub.Name, sub.Quantity * times))
                    return false;
            remaining[item] = producedInSingleReaction * times - stillNeeded;
            return true;
        }

        private static (long, List<Reagent>) FindSingleFuelUnitCost(Dictionary<string, Reaction> reactions, Dictionary<string, int> order)
        {
            long cost = 0;
            var leftovers = new List<Reagent>();
            var required = new List<Reagent>();
            required.Add(reactions["FUEL"].Product);
            while(required.Count > 0)
            {
                var item = required.MaxBy(i => order[i.Name]);
                required.Remove(item);
                if(item.Name == "ORE")
                {
                    cost += item.Quantity;
                    break;
                }
                var reaction = reactions[item.Name];
                var needed = item.Quantity;
                var producedInReaction = reaction.Product.Quantity;
                var times = needed / producedInReaction + (needed % producedInReaction != 0 ? 1 : 0);
                var produced = times * producedInReaction;
                if(produced > needed)
                    leftovers.Add(new Reagent(item.Name, produced - needed));
                foreach(var sub in reaction.Substrats)
                {
                    bool found = false;
                    for(int i = 0; i < required.Count; i++)
                    if(required[i].Name == sub.Name)
                    {
                        required[i] = new Reagent(required[i].Name, required[i].Quantity + sub.Quantity * times);
                        found = true;
                        break;
                    }
                    if(!found)
                        required.Add(new Reagent(sub.Name, sub.Quantity * times));
                }
            }
            return (cost, leftovers);
        }

        private static Dictionary<string, int> FindOrder(Dictionary<string, Reaction> reactions)
        {
            var order = new Dictionary<string, int>();
            int ord = 1;
            order.Add("ORE", ord++);
            bool changes = true;
            while(changes)
            {
                changes = false;
                foreach(var (name, reaction) in reactions)
                {
                    if(!order.ContainsKey(name) && reaction.Substrats.All(s => order.ContainsKey(s.Name)))
                    {
                        order[name] = ord++;
                        changes = true;
                    }
                }
            }
            return order;
        }

        private static Dictionary<string, Reaction> ParseReactions(string[] lines)
        {
            var reactions = new Reaction[lines.Length];
            for(int i = 0; i < lines.Length; i++)
            {
                var s = lines[i].Split(new char[] { ',', ' ', '=', '>' }, StringSplitOptions.RemoveEmptyEntries);
                var substrats = new List<Reagent>();
                for(int j = 1; j < s.Length - 2; j += 2)
                    substrats.Add(new Reagent(s[j], long.Parse(s[j-1])));
                reactions[i] = new Reaction(substrats.ToArray(), new Reagent(s[s.Length-1], long.Parse(s[s.Length-2])));
            }
            return reactions.ToDictionary(r => r.Product.Name, r => r);
        }

        public record struct Reagent(string Name, long Quantity);

        public record struct Reaction(Reagent[] Substrats, Reagent Product);
    }
}