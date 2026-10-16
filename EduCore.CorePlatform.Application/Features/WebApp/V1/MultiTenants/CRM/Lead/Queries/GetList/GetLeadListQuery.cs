using EduCore.CorePlatform.Application.Common.Models;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs;
using EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.DTOs.Queries;
using MediatR;
using System;
using System.Collections.Generic;
using System.Text;

namespace EduCore.CorePlatform.Application.Features.WebApp.V1.MultiTenants.CRM.Lead.Queries.GetList;

public record GetLeadListQuery(LeadListQueryRequest RequestData) : IRequest<Result<List<LeadDto>>>;