using System.Linq;
using Xunit;

namespace Rallydator.Test
{
    public class ValidationTest
    {
        private readonly Driver _driver = new Driver("Mad");

        [Fact]
        public void Single_section_with_a_roll_that_doesnt_cross_the_finish_line_gives_error()
        {
            SpecialStageDescription = "J12-J13";
            RaceResultDescription = "4(4)";

            ExecuteTest(false);
        }

        [Fact]
        public void Single_section_with_a_roll_that_crosses_the_finish_line_has_no_error()
        {
            SpecialStageDescription = "J12-J13";
            RaceResultDescription = "5(5)";

            ExecuteTest(true);
        }

        [Fact]
        public void Two_sections_with_enough_1_rolls_to_cross_the_finish_line_has_no_error()
        {
            SpecialStageDescription = "J12-J13:C7-C6";
            RaceResultDescription = "1(3):1(3):1(3):1(2)";

            ExecuteTest(true);
        }

        [Fact]
        public void Two_sections_with_good_rolls_to_cross_the_finish_line_has_no_error()
        {
            SpecialStageDescription = "J12-J13:C7-C6";
            RaceResultDescription = "5(7):2(4)";

            ExecuteTest(true);
        }

        [Fact]
        public void Section_with_jump_normal_gear_to_cross_the_finish_line_has_no_error()
        {
            SpecialStageDescription = "C3-C8";
            RaceResultDescription = "1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):4(4):5(3)";

            ExecuteTest(true);
        }

        [Fact]
        public void Section_with_jump_one_higher_gear_to_cross_the_finish_line_has_no_error()
        {
            SpecialStageDescription = "C3-C8";
            RaceResultDescription = "1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):4(4):5(2)";

            ExecuteTest(true);
        }

        [Fact]
        public void Section_with_jump_one_lower_gear_is_one_space_short_of_finish_and_gives_error()
        {
            SpecialStageDescription = "C3-C8";
            RaceResultDescription = "1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(3)";

            ExecuteTest(false);
        }

        [Fact]
        public void Section_with_jump_one_lower_gear_to_cross_the_finish_line_has_no_error()
        {
            SpecialStageDescription = "C3-C8";
            RaceResultDescription = "1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(3):1(3)";

            ExecuteTest(true);
        }

        [Fact]
        public void Real_world_example_of_finished_stage()
        {
            SpecialStageDescription = "J12-J13:C7-C6:J14-J11:L1-L0:J12-J13:C7-C6:J14-J11:L1-L0";
            RaceResultDescription = "5(7):2(6):2(4):2(4):5(7):1(7):4(6):5(5)!S:2(6):4(6):5(5):2(6):2(4):2(4):5(7):1(7):4(6):5(5):5(3):3(3):4(5)!";

            ExecuteTest(true);
        }

        private string RaceResultDescription { get; set; }

        private string SpecialStageDescription { get; set; }

        private void ExecuteTest(bool success)
        {
            var ss1 = SpecialStageFactory.Create(SpecialStageDescription, false);
            var raceResult = RaceResultFactory.Create(RaceResultDescription);
            var specialStageResult = new SpecialStageResult(ss1, raceResult);
            var rally = Rally.Create(_driver, new[] { specialStageResult });

            var rallyValidationResult = rally.Validate();

            if(success) Assert.Empty(rallyValidationResult.ValidationResults.First().Errors);
            else Assert.NotEmpty(rallyValidationResult.ValidationResults.First().Errors);
        }
    }
}
