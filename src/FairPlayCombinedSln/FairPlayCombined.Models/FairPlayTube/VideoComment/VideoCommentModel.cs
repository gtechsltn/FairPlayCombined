﻿using FairPlayCombined.Common.GeneratorsAttributes;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FairPlayCombined.Models.FairPlayTube.VideoComment
{
    public class VideoCommentModel : IListModel
    {
        public long VideoCommentId { get; set; }

        public long VideoInfoId { get; set; }

        [Required]
        [StringLength(450)]
        public string? ApplicationUserId { get; set; }

        [Required]
        [StringLength(500)]
        public string? Comment { get; set; }

        public DateTimeOffset RowCreationDateTime { get; set; }

        [StringLength(256)]
        public string? RowCreationUser { get; set; }
    }
}
