using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Shouldly;
using Volo.Abp.Validation;

namespace Volo.Abp.AspNetCore.Mvc.Validation
{
    [Route("api/validation-test")]
    public class ValidationTestController : AbpController
    {
        [HttpGet]
        [Route("object-result-action")]
        public Task<string> ObjectResultAction(ValidationTest1Model model)
        {
            ModelState.IsValid.ShouldBeTrue(); //AbpValidationFilter throws exception otherwise
            return Task.FromResult(model.Value1);
        }

        [HttpGet]
        [Route("object-result-action-with-custom_validate")]
        public Task<string> ObjectResultActionWithCustomValidate(CustomValidateModel model)
        {
            ModelState.IsValid.ShouldBeTrue(); //AbpValidationFilter throws exception otherwise
            return Task.FromResult(model.Value1);
        }

        [HttpGet]
        [Route("action-result-action")]
        public IActionResult ActionResultAction(ValidationTest1Model model)
        {
            return Content("ModelState.IsValid: " + ModelState.IsValid.ToString().ToLowerInvariant());
        }
        
        [HttpGet]
        [Route("object-result-action-dynamic-length")]
        public Task<string> ObjectResultActionDynamicLength(ValidationDynamicTestModel model)
        {
            ModelState.IsValid.ShouldBeTrue(); //AbpValidationFilter throws exception otherwise
            return Task.FromResult(model.Value1);
        }

        public class ValidationTest1Model
        {
            [Required]
            [StringLength(5, MinimumLength = 2)]
            public string Value1 { get; set; }
        }
        
        public class ValidationDynamicTestModel
        {
            [DynamicStringLength(typeof(Consts), nameof(Consts.MaxValue1Length), nameof(Consts.MinValue1Length))]
            public string Value1 { get; set; }

            [DynamicMaxLength(typeof(Consts), nameof(Consts.MaxValue2Length))]
            public string Value2 { get; set; }
            
            [DynamicMaxLength(typeof(Consts), nameof(Consts.MaxValue3Length))]
            public int[] Value3 { get; set; }
            
            public static class Consts
            {
                public static int MinValue1Length { get; set; } = 2;
                public static int MaxValue1Length { get; set; } = 7;

                public static int MaxValue2Length { get; set; } = 4;
                
                public static int MaxValue3Length { get; set; } = 2;
            }
        }

        public class CustomValidateModel : IValidatableObject
        {
            public string Value1 { get; set; }

            public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
            {
                if (Value1 != "hello")
                {
                    yield return new ValidationResult("Value1 should be hello");
                }
            }
        }
    }
}