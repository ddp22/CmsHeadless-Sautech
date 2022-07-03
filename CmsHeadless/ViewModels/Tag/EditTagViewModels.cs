﻿using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using CmsHeadless.Models;

namespace CmsHeadless.ViewModels.Tag
{
    public class EditTagViewModel
    {
        [Required]
        public string Name { get; set; } = null!;
        [Required]
        public string Url { get; set; } = null!;
    }
}