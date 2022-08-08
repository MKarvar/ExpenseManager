
using System.ComponentModel.DataAnnotations;

namespace WebUtilities
{
    public enum ApiResultStatusCode
    {
        [Display(Name = "Is Success")]
        Success = 0,

        [Display(Name = "Server error occured.")]
        ServerError = 1,

        [Display(Name = "Parameters are not valid.")]
        BadRequest = 2,

        [Display(Name = "Not found")]
        NotFound = 3,

        [Display(Name = "List is empty")]
        ListEmpty = 4,

        [Display(Name = "A logic error occured.")]
        LogicError = 5,

        [Display(Name = "Unauthorized")]
        UnAuthorized = 6,

        [Display(Name = "Forbidden")]
        Forbidden = 7
    }
}
