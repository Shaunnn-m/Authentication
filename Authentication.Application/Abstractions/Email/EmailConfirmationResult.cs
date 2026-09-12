using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Application.Abstractions.Email
{
    public sealed record EmailConfirmationResult(
    Guid UserId,
    string Email,
    string ConfirmationToken,
    string ConfirmationLink);
}
