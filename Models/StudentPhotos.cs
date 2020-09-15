namespace goOfflineE.Models
{
    using System.Collections.Generic;

    /// <summary>
    /// Defines the <see cref="StudentPhotos" />.
    /// </summary>
    public class StudentPhotos
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="StudentPhotos"/> class.
        /// </summary>
        public StudentPhotos()
        {
            this.Photos = new List<string>();
        }

        /// <summary>
        /// Gets or sets the Photos.
        /// </summary>
        public List<string> Photos { get; set; }
    }
}
