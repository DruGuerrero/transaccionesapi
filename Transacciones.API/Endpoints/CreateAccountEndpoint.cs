using Ardalis.ApiEndpoints;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Transacciones.API.Endpoints
{
    [Route("/users")]
    public class CreateAccountEndpoint : EndpointBaseAsync.WithRequest<CreateUserRequest>.WithActionResult
    {
        private readonly ICreateUserUseCase _createUserUseCase;

        public CreateAccountEndpoint(ICreateUserUseCase createUserUseCase)
        {
            _createUserUseCase = createUserUseCase;
        }

        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        [SwaggerOperation(
            Summary = "Pre-create a user",
            Description = "Creates a user record before they log in with Google. Email and username are set on first login.",
            Tags = ["Users"])]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [ProducesResponseType(StatusCodes.Status409Conflict)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public override async Task<ActionResult> HandleAsync(CreateUserRequest request, CancellationToken cancellationToken = default)
        {
            try
            {
                var dto = new CreateUserDto
                {
                    FirstName = request.FirstName,
                    LastName = request.LastName,
                    Email = request.Email,
                    RoleId = request.RoleId,
                    BranchId = request.BranchId,
                    UserProfile = request.UserProfile is null ? null : new Core.Models.Users.UserProfile
                    {
                        AdvisorErpCode = request.UserProfile.AdvisorErpCode,
                        AdvisorAioCode = request.UserProfile.AdvisorAioCode
                    }
                };

                var result = await _createUserUseCase.ExecuteAsync(dto, cancellationToken);
                return Created($"/users/{result.Id}", new CreateUserResponse(result.Id, result.FirstName, result.LastName, result.RoleId, result.BranchId));
            }
            catch (InvalidOperationException ex)
            {
                return Conflict(ex.Message);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }
}
