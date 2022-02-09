﻿using System.Collections.Generic;
using System.Linq;

namespace MrCMS.Installation.Models
{
    public class InstallationResult
    {
        public InstallationResult()
        {
            Errors = new List<string>();
        }

        public List<string> Errors { get; set; }

        public bool Success => !Errors.Any();

        public void AddModelError(string message)
        {
            Errors.Add(message);
        }
    }
}