﻿namespace Common.Core.Enumerations
{
    public enum ErrorCodeEnum
    {
        LoginFailed = 1,
        IncorrectUser = 2,
        NotAuthorized = 3,
        IncorrectPassword = 4,
        PasswordWrongFormat = 5,
        PasswordExpired = 6,
        UserInactive = 7,
        YouHaveLogged = 8,
        LoginFailed3Time = 9,
        CannotDeleteUser = 10,
        CannotSendEmailToResetPassword = 11,
        DupplicationUser = 12,
        SameWithCurrentPass = 13,
        SessionExpired = 14,
        MultiplePasswordResetting = 15,
        UserDeletedOrNotExisted = 16,
        UserManagerExisted = 17,
        EmailMangerExisted = 18,
        IncorrectManagerUser = 19,
        ExitedSpecialInUsername = 18,
    }
}