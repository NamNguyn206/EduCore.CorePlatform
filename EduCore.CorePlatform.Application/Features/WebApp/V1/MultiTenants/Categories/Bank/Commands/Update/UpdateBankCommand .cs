using MediatR;
using System;
using System.Collections.Generic;
using System.Text;
using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.DTOs.Commands;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.Categories.Bank.Commands.Update;

public sealed record UpdateBankCommand(UpdateBankRequest RequestData) : IRequest<Result<BankDto>>;