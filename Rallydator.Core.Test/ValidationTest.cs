using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using Xunit;

namespace Rallydator.Core.Test
{
    public class ValidationTest
    {
        [Fact]
        public void Single_section_with_a_roll_that_doesnt_cross_the_finish_line_gives_error()
        {
            SS1Tire = TireType.Snow;
            SS1Description = "J12-J13";
            SS1Result = "4(4)";

            ExecuteTest(false);
        }

        [Fact]
        public void Single_section_with_a_roll_that_crosses_the_finish_line_has_no_error()
        {
            SS1Tire = TireType.Snow;
            SS1Description = "J12-J13";
            SS1Result = "5(5)";

            ExecuteTest(true);
        }

        [Fact]
        public void Two_sections_with_enough_1_rolls_to_cross_the_finish_line_has_no_error()
        {
            SS1Tire = TireType.Snow;
            SS1Description = "J12-J13:C7-C6";
            SS1Result = "1(3):1(3):1(3):1(2)";

            ExecuteTest(true);
        }

        [Fact]
        public void Two_sections_with_good_rolls_to_cross_the_finish_line_has_no_error()
        {
            SS1Tire = TireType.Snow;
            SS1Description = "J12-J13:C7-C6";
            SS1Result = "5(7):2(4)";

            ExecuteTest(true);
        }

        [Fact]
        public void Section_with_jump_normal_gear_to_cross_the_finish_line_has_no_error()
        {
            SS1Description = "C3-C8";
            SS1Result = "1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):4(4):5(3)";

            ExecuteTest(true);
        }

        [Fact]
        public void Section_with_jump_one_higher_gear_to_cross_the_finish_line_has_no_error()
        {
            SS1Description = "C3-C8";
            SS1Result = "1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):4(4):5(2)";

            ExecuteTest(true);
        }

        [Fact]
        public void Section_with_jump_one_lower_gear_is_one_space_short_of_finish_and_gives_error()
        {
            SS1Description = "C3-C8";
            SS1Result = "1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(3)";

            ExecuteTest(false);
        }

        [Fact]
        public void Section_with_jump_one_lower_gear_to_cross_the_finish_line_has_no_error()
        {
            SS1Description = "C3-C8";
            SS1Result = "1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(3):1(3)";

            ExecuteTest(true);
        }

        [Fact]
        public void Real_world_example_of_finished_stage()
        {
            SS1Tire = TireType.Snow;
            SS1Description = "J12-J13:C7-C6:J14-J11:L1-L0:J12-J13:C7-C6:J14-J11:L1-L0";
            SS1Result = "5(7):2(6):2(4):2(4):5(7):1(7):4(6):5(5)!S:2(6):4(6):5(5):2(6):2(4):2(4):5(7):1(7):4(6):5(5):5(3):3(3):4(5)!";

            ExecuteTest(true);
        }

        [Fact]
        public void Real_world_example_of_finished_stage_HAS_BUG()
        {
            SS1Tire = TireType.Snow;
            DriverName = "MadMihi";
            SS1Description = "L19-L18:V14-V11:J10-J19:C15-C14:V12-V13:L19-L18:V14-V11:J10-J19:C15-C14:V12-V13";
            SS1Result = "4(4):2(4):3(4):1(4):5(7):5(4):1(7):3(5):2(3):5(7):2(5)!S:5(5):0(6)!:4(4):1(4):5:5(4):1(7):3(4):2(3):5:2(5):5(5)";

            ExecuteTest(true);
        }

        [Fact]
        public void Dropping_Inward_From_Drift_Space()
        {
            SS1Tire = TireType.Asphalt;
            SS1Description = "C0-C9";
            SS1Result = "1(3):1(3)";

            ExecuteTest(true);
        }
        
        [Fact]
        public void No_time_attack()
        {
            SS1Tire = TireType.Asphalt;
            SS1Description = "C0-C9"; // 5 Spaces
            SS1Result = "3"; // 1,2,3,A,A

            ExecuteTest(true);
        }

        [Fact]
        public void Real_world_example_2018_October()
        {
            var errors = new List<string>();

            SS1Tire = TireType.Snow;
            var data = File.ReadAllLines(@".\TestData\2018_October.txt");
            SS1Description = data[0];
            for (int i = 1; i < data.Length; i += 2)
            {
                var indexOfDash = data[i].IndexOf('-');
                DriverName = data[i].Substring(6, indexOfDash - 6).Trim();
                SS1Result = data[i + 1].Trim();

                try
                {
                    ExecuteTest(true);
                }
                catch (Exception e)
                {
                    var error = $"'{DriverName}': {e.Message}";
                    errors.Add(error);
                    Debug.WriteLine(error);
                }
            }

            AssertErrors(errors);
        }

        [Fact]
        public void Real_world_example_2018_November()
        {
            var errors = new List<string>();

            SS1Tire = TireType.Snow;
            var data = File.ReadAllLines(@".\TestData\2018_November.txt");

            SpecialStageDescriptions.Add(data[0]);
            SpecialStageDescriptions.Add(data[1]);
            for (int i = 2; i < data.Length; i += 4)
            {
                SpecialStageResults.Clear();

                var indexOfDash = data[i].IndexOf('-');
                DriverName = data[i].Substring(5, indexOfDash - 5).Trim();

                SpecialStageResults.Add(new SpecialStageResult(data[i + 1].Trim(), ParseTire(data, i)));
                SpecialStageResults.Add(new SpecialStageResult(data[i + 3].Trim(), ParseTire(data, i+2)));

                try
                {
                    ExecuteMultiStageTest(true);
                }
                catch (Exception e)
                {
                    var error = $"'{DriverName}': {e.Message}";
                    errors.Add(error);
                    Debug.WriteLine(error);
                }
            }

            AssertErrors(errors);
        }

        private static void AssertErrors(List<string> errors)
        {
            foreach (var error in errors)
            {
                Debug.WriteLine(error);
            }

            Assert.Empty(errors);
        }

        private static TireType ParseTire(string[] data, int i)
        {
            var indexOfTires = data[i].IndexOf("Tires:");
            var indexOfNextDash = data[i].IndexOf('-', indexOfTires);
            var tireString = data[i].Substring(indexOfTires + 6, indexOfNextDash - indexOfTires - 6).Trim();
            var tireType = Enum.Parse<TireType>(tireString);
            return tireType;
        }


        private string SS1Result { get; set; }
        private string SS1Description { get; set; }
        private TireType SS1Tire { get; set; } = TireType.Asphalt;

        private string DriverName { get; set; } = "Dummy";

        private void ExecuteTest(bool success)
        {
            var ss1 = SpecialStageFactory.Create(SS1Description, false);
            var raceResult = RaceResultFactory.Create(SS1Result, SS1Tire);
            var specialStageResult = new Core.SpecialStageResult(ss1, raceResult);
            var rally = Rally.Create(new Driver(DriverName), new[] { specialStageResult });

            var rallyValidationResult = rally.Validate();

            if (success) Assert.Empty(rallyValidationResult.ValidationResults.First().Errors);
            else Assert.NotEmpty(rallyValidationResult.ValidationResults.First().Errors);
        }

        private void ExecuteMultiStageTest(bool success, bool hasAssistance = false)
        {
            var specialStages = new List<SpecialStage>();
            foreach (var specialStageDescription in SpecialStageDescriptions)
            {
                specialStages.Add(SpecialStageFactory.Create(specialStageDescription, hasAssistance));
            }

            var results = new List<Core.SpecialStageResult>();
            for (var i = 0; i < specialStages.Count; i++)
            {
                var raceResult = RaceResultFactory.Create(SpecialStageResults[i].Result, SpecialStageResults[i].TireType);
                results.Add(new Core.SpecialStageResult(specialStages[i], raceResult));
            }

            var rally = Rally.Create(new Driver(DriverName), results);
            var rallyValidationResult = rally.Validate();

            if (success) Assert.Empty(rallyValidationResult.ValidationResults.First().Errors);
            else Assert.NotEmpty(rallyValidationResult.ValidationResults.First().Errors);
        }

        private List<string> SpecialStageDescriptions { get; } = new List<string>();

        private List<SpecialStageResult> SpecialStageResults { get; } = new List<SpecialStageResult>();

        private class SpecialStageResult
        {
            public string Result { get; }
            public TireType TireType { get; }

            public SpecialStageResult(string result, TireType tireType)
            {
                Result = result;
                TireType = tireType;
            }
        }
    }
}
