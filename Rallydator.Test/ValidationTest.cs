using Xunit;
using Rallydator.Validation;

namespace Rallydator.Test
{
    public class ValidationTest
    {
        [Fact]
        public void Single_section_with_a_roll_that_doesnt_cross_the_finish_line_gives_error()
        {

            var specialStage = SpecialStageFactory.Create("J12-J13");
            var raceResult = RaceResultFactory.Create("4(4)");

            var validationResult = new ResultValidator(specialStage, raceResult).Validate();
            Assert.NotEmpty(validationResult.Errors);
        }

        [Fact]
        public void Single_section_with_a_roll_that_crosses_the_finish_line_has_no_error()
        {

            var specialStage = SpecialStageFactory.Create("J12-J13");
            var raceResult = RaceResultFactory.Create("5(5)");

            var validationResult = new ResultValidator(specialStage, raceResult).Validate();
            Assert.Empty(validationResult.Errors);
        }

        [Fact]
        public void Two_sections_with_enough_1_rolls_to_cross_the_finish_line_has_no_error()
        {

            var specialStage = SpecialStageFactory.Create("J12-J13:C7-C6");
            var raceResult = RaceResultFactory.Create("1:1:1:1:1:1:1:1:1:1:1");

            var validationResult = new ResultValidator(specialStage, raceResult).Validate();
            Assert.Empty(validationResult.Errors);
        }

        [Fact]
        public void Two_sections_with_good_rolls_to_cross_the_finish_line_has_no_error()
        {

            var specialStage = SpecialStageFactory.Create("J12-J13:C7-C6");
            var raceResult = RaceResultFactory.Create("5(7):2(4)");

            var validationResult = new ResultValidator(specialStage, raceResult).Validate();
            Assert.Empty(validationResult.Errors);
        }

        [Fact]
        public void Section_with_jump_normal_gear_to_cross_the_finish_line_has_no_error()
        {
            var specialStage = SpecialStageFactory.Create("C3-C8");
            
            var raceResult = RaceResultFactory.Create("1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):4(4):5(3)");

            var validationResult = new ResultValidator(specialStage, raceResult).Validate();
            Assert.Empty(validationResult.Errors);
        }

        [Fact]
        public void Section_with_jump_one_higher_gear_to_cross_the_finish_line_has_no_error()
        {
            var specialStage = SpecialStageFactory.Create("C3-C8");

            var raceResult = RaceResultFactory.Create("1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):1(2):4(4):5(2)");

            var validationResult = new ResultValidator(specialStage, raceResult).Validate();
            Assert.Empty(validationResult.Errors);
        }

        [Fact]
        public void Real_world_example_of_finished_stage()
        {
            var specialStage = SpecialStageFactory.Create("J12-J13:C7-C6:J14-J11:L1-L0:J12-J13:C7-C6:J14-J11:L1-L0");
            var raceResult = RaceResultFactory.Create("5(7):2(6):2(4):2(4):5(7):1(7):4(6):5(5)!S:2(6):4(6):5(5):2(6):2(4):2(4):5(7):1(7):4(6):5(5):5(3):3(3):4(5)!");

            var validationResult = new ResultValidator(specialStage, raceResult).Validate();
            Assert.Empty(validationResult.Errors);
        }
    }
}
