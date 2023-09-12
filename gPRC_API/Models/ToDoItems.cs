using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace gPRC_API.Models
{
    /// <summary>
    /// Represents a To-Do item in the application.
    /// </summary>
    public class ToDOItems
    {
        /// <summary>
        /// Gets or sets the unique identifier for the To-Do item.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the title of the To-Do item.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the description of the To-Do item.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets a value indicating whether the To-Do item is complete.
        /// </summary>
        public bool IsComplete { get; set; } = false;
    }
}