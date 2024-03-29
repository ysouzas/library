﻿using FluentValidation;
using Library.Api.Model;

namespace Library.Api.Validators;

public class BookValidator : AbstractValidator<Book>
{
    public BookValidator()
    {
        RuleFor(book => book.ISBN)
            .Matches(@"^(?:ISBN(?:-13)?:?●)?(?=[0-9]{13}$|(?=(?:[0-9]+[-●]){4})[-●0-9]{17}$)97[89][-●]?[0-9]{1,5}[-●]?[0-9]+[-●]?[0-9]+[-●]?[0-9]$")
            .WithMessage("Value was not a valid ISBN-13");

        RuleFor(book => book.Title).NotEmpty();
        RuleFor(book => book.ShortDescription).NotEmpty();
        RuleFor(book => book.PageCount).GreaterThan(0);
        RuleFor(book => book.Author).NotEmpty();
    }
}
