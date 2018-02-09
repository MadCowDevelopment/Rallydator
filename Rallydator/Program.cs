using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Rallydator.AIMA;
using Rallydator.Helpers;
using Rallydator.Validation;

namespace Rallydator
{
    partial class Program
    {
        static void Main(string[] args)
        {
            var specialStage = SpecialStageFactory.Create("J12-J13:C7-C6:J14-J11:L1-L0:J12-J13:C7-C6:J14-J11:L1-L0");
            var raceResult = RaceResultFactory.Create("5(7):2(6):2(4):2(4):5(7):1(7):4(6):5(5)!S:2(6):4(6):5(5):2(6):2(4):2(4):5(7):1(7):4(6):5(5):5(3):3(3):4(5)!");

            var validationResult = new ResultValidator(specialStage, raceResult).Validate();
        }
    }
}
