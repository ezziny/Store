using System;
using Microsoft.Identity.Client;

namespace Store.Services.HandleResponses;

public class CustomException : Response
{
    public CustomException(int statusCode, string? message = null, string? details = null) : base(statusCode, message)
    {
        Details = details;
    }
    public string? Details { get; set; }
}
