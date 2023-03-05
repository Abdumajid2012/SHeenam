﻿//==================================================
// Copyright (c) Coalition of Good-Hearted Engineers
// Free To Use To Find Comfort and Peace
//==================================================

using System;
using System.Threading.Tasks;
using EFxceptions.Models.Exceptions;
using Microsoft.Data.SqlClient;
using Sheenam.Api.Models.Foundations.Guests;
using Sheenam.Api.Models.Foundations.Guests.Exceptions;
using Xeptions;

namespace Sheenam.Api.Services.Foundations.Guests
{
    public partial class GuestService
    {
        private delegate ValueTask<Guest> ReturningGuestFunction();

        private async ValueTask<Guest> TryCatch(ReturningGuestFunction returningGuestFunction)
        {
            try
            {
                return await returningGuestFunction();
            }

            catch (NullGuestException nullGuestException)
            {
                throw CreateAndLogValidationException(nullGuestException);
            }

            catch(InvalidGuestException invalidGuestException)
            {
                throw CreateAndLogValidationException(invalidGuestException);
            }

            catch(SqlException sqlException)
            {
                var failedGuestStorageException = 
                    new FailedGuestStorageException(sqlException);

                throw CreateAndLogCriticalDependenceException(failedGuestStorageException);
            }

            catch(DuplicateKeyException dublicateKeyException)
            {
                var alreadyExistGuestException =
                    new AlreadyExistGuestException(dublicateKeyException);

                throw CreateAndDependenceValidationException(alreadyExistGuestException);
            }

            catch(Exception exception)
            {
                var failedGuestServiceException =
                    new FailedGuestServiceException(exception);

                throw CreateAndGuestServiceException(failedGuestServiceException);
            }
        }

        private GuestValidationException CreateAndLogValidationException(Xeption exception)
        {
            var guestValidationException =
                new GuestValidationException(exception);

            this.loggingBroker.LogError(guestValidationException);

            return guestValidationException;
        }

        private GuestDependencyException CreateAndLogCriticalDependenceException(Xeption exception)
        {
            var guestDependencyException = new GuestDependencyException(exception);
            this.loggingBroker.LogCritical(guestDependencyException);

            return guestDependencyException;
        }

        private GuestDependencyValidationException CreateAndDependenceValidationException(
            Xeption exception)
        {
            var guestDependencyValidationException = 
                new GuestDependencyValidationException(exception);

            this.loggingBroker.LogError(guestDependencyValidationException);

            return guestDependencyValidationException;
        }

        private GuestServiceException CreateAndGuestServiceException(Xeption exception)
        {
            var guestServiceException = new GuestServiceException(exception);
            this.loggingBroker.LogError(guestServiceException);

            return guestServiceException;
        }
    }
}
