using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FluentValidation;
using PongServer.Domain.Utils;

namespace PongServer.Application.Validators.Filters
{
    public class QueryFiltersValidator : AbstractValidator<QueryFilters>
    {
        public QueryFiltersValidator()
        {
            RuleFor(query => query.Page)
                .GreaterThan(0)
                .When(query => query.Page.HasValue)
                .NotNull()
                .When(query => query.PageSize.HasValue);
            RuleFor(query => query.PageSize)
                .GreaterThan(0)
                .When(query => query.PageSize.HasValue)
                .NotNull()
                .When(query => query.Page.HasValue);
            RuleFor(query => query.HostName)
                .MinimumLength(3)
                .When(query => !string.IsNullOrEmpty(query.HostName));
        }
    }
}
