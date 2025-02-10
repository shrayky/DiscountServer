﻿using Domain.Common;

namespace Domain.Entities
{
    public class Card : IHasId
    {
        public string Id { get; set; } = string.Empty;
        public string ClientId { get; set; } = string.Empty;
    }
}
