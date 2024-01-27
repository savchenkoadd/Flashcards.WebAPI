﻿using System.ComponentModel.DataAnnotations;

namespace Flashcards.Core.Domain.Entities
{
    public class Flashcard
    {
        [Key]
        public Guid CardId { get; set; }

        public Guid UserId { get; set; }

        public float EFactor { get; set; } = 2.5f;

        public int RepetitionCount { get; set; } = 0;

        public int RepeatInDays { get; set; } = 0;

        [StringLength(100)]
        public string MainSide { get; set; }

        [StringLength(500)]
        public string OppositeSide { get; set; }
    }
}
