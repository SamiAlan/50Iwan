using FluentValidation;
using Iwan.Shared.Dtos.Media;
using Iwan.Shared.Extensions;
using Microsoft.Extensions.Localization;

namespace Iwan.Shared.Validators.Media.Admin
{
    public class UploadTempImageDtoValidator : AbstractValidator<UploadTempImageDto>
    {
        public UploadTempImageDtoValidator(IStringLocalizer<Localization> localizer)
        {
            RuleFor(r => r.Image)
                .Cascade(CascadeMode.Stop)
                .CustomImageNotNullValidation(localizer)
                .ChildRules(i => i.RuleFor(i => i.FileName).CustomSupportedImageExtensionValidation(localizer));
        }
    }
}
