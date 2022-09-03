// Copyright (c) 2021 Welcome Bonginhlahla Sithole
// Reference: https://github.com/SitholeWB/Pagination.EntityFrameworkCore.Extensions
// Modified by V-nyang

// Released under the MIT License.
// See the LICENSE file in the project root for more information.

using System.Runtime.Serialization;

namespace Domain.Shared.Models;

[Serializable]
public sealed class PaginationException : Exception
{
    public PaginationException(string message) : base(message)
    {
    }

    private PaginationException(SerializationInfo info, StreamingContext context)
        : base(info, context)
    {
    }
}
