public record CreatePollDto(
    string Name,
    string Description,
    List<PollOptionDto> Options,
    DateTime StartDate,
    DateTime EndDate,
    bool IsAnonymous);

public record PollOptionDto(string Name, string Description);