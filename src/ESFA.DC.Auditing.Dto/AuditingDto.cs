namespace ESFA.DC.Auditing.Dto
{
    /// <summary>
    /// The Auditing DTO, must be serialisable.
    /// </summary>
    public class AuditingDto
    {
        // ReSharper disable once MemberCanBePrivate.Global
        // Empty constructor for [Serializable]
        public AuditingDto()
        {
        }

        public AuditingDto(string source, int eventType, string userId, long jobId, string filename, string ukPrn, string extraInfo)
        {
            Source = source;
            EventType = eventType;
            UserId = userId;
            JobId = jobId;
            Filename = filename;
            UkPrn = ukPrn;
            ExtraInfo = extraInfo;
        }

        public string Source { get; set; }

        public int /* AuditEventType */ EventType { get; set; }

        public string UserId { get; set; }

        public long JobId { get; set; }

        public string Filename { get; set; }

        public string UkPrn { get; set; }

        public string ExtraInfo { get; set; }
    }
}
