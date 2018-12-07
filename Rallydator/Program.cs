using System;
using System.Diagnostics;
using System.Linq;
using Rallydator.Core;

namespace Rallydator
{
    class Program
    {
        static void Main()
        {
            Console.WriteLine("Press any key to start validation.");
            Console.ReadKey(true);

            Console.WriteLine("Validating...");
            var ss1 = SpecialStageFactory.Create("L19-L18:V14-V11:J10-J19:C15-C14:V12-V13:L19-L18:V14-V11:J10-J19:C15-C14:V12-V13", false);
            var raceResult = RaceResultFactory.Create("4(4):2(3):2(4):3(5):5(6):5(4):5(3):1(6):4(6):2(4):5(7):2(4):5(7):2(4):2(4):3(5):5(6):5(4):5(3):1(6):4(6):2(4):5(7):2(4):5(6)", TireType.Snow);
            var specialStageResult = new SpecialStageResult(ss1, raceResult);
            var rally = Rally.Create(new Driver("msaya"), new[] { specialStageResult });

            var stopWatch = Stopwatch.StartNew();
            var rallyValidationResult = rally.Validate();
            stopWatch.Stop();

            Console.WriteLine(rallyValidationResult.ValidationResults.First().Errors.Any());
            Console.WriteLine(stopWatch.Elapsed);
            Console.ReadKey(true);
        }
    }
}
