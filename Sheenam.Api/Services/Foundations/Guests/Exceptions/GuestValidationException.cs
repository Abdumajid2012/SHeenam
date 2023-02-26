//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests.Exceptions
{
    public class GuestValidationException : Xeption
    {
        public GuestValidationException(Xeption innerException)
            :base(message: "Guest validation error occored, fix the errors and try agein.",
                 innerException)    
        { }
    }
}
