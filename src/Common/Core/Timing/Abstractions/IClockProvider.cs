﻿using System;

namespace Common.Core.Timing.Abstractions
{
    public interface IClockProvider
    {
        DateTime Now { get; }

        DateTimeKind Kind { get; }

        DateTime? NormalizeNullable(DateTime? dateTime);

        DateTime Normalize(DateTime dateTime);

        object NormalizeObject(object dateTime);
    }
}