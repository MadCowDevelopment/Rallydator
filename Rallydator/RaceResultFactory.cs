using System.Collections.Generic;

namespace Rallydator
{
    public class RaceResultFactory
    {
        public static RaceResult Create(string result)
        {
            var rolls = new List<Roll>();

            foreach (var roll in result.Split(':'))
            {
                rolls.Add(Roll.Parse(roll));
            }

            return new RaceResult(rolls);
        }
    }
}