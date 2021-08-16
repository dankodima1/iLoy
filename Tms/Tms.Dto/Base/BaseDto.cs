using Newtonsoft.Json;

namespace Tms.Dto.Base
{
    /// <summary>
    /// Base class for dtos
    /// </summary>
    public abstract class BaseDto
    {
        /// <summary>
        /// Gets or sets the dto identifier
        /// </summary>
        [JsonProperty("id")]
        public int Id { get; set; }
    }
}
