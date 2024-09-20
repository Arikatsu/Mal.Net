namespace Mal.Net;

/// <summary>
/// Options for configuring optional request parameters of some API endpoints.
/// </summary>
public class MalRequestOptions
{
    /// <summary>
    /// The maximum number of items to return. Default is 100.
    /// </summary>
    public int Limit { get; set; } = 100;
    
    /// <summary>
    /// The number of items to skip before starting to return results. Default is 0.
    /// </summary>
    public int Offset { get; set; } = 0;
    
    /// <summary>
    /// Additional fields to include in the JSON response. Default is an empty collection.
    /// </summary>
    public IEnumerable<string> Fields { get; set; } = Enumerable.Empty<string>();
    
    /// <summary>
    /// Whether to include NSFW content in the results. Default is false.
    /// </summary>
    public bool IncludeNsfw { get; set; } = false;
}

/// <summary>
/// Options for configuring optional request parameters of the forum topic API endpoint.
/// </summary>
public class ForumTopicOptions
{
    /// <summary>
    /// The ID of the board to retrieve topics for. Default is null.
    /// </summary>
    public int? BoardId { get; set; } = null;
    
    /// <summary>
    /// The ID of the sub-board to retrieve topics for. Default is null.
    /// </summary>
    public int? SubBoardId { get; set; } = null;

    /// <summary>
    /// The query string to filter topics by. Default is null.
    /// </summary>
    public string? Query { get; set; } = null;

    /// <summary>
    /// The username of the topic creator to filter topics by. Default is null.
    /// </summary>
    public string? TopicUserName { get; set; } = null;
    
    /// <summary>
    /// The username of the user to filter topics by. Default is null.
    /// </summary>
    public string? UserName { get; set; } = null;
    
    /// <summary>
    /// The maximum number of items to return. Default is 100.
    /// </summary>
    public int Limit { get; set; } = 100;
    
    /// <summary>
    /// The number of items to skip before starting to return results. Default is 0.
    /// </summary>
    public int Offset { get; set; } = 0;
}

/// <summary>
/// Options for updating the status of an anime or manga in the user's list.
/// </summary>
public class AnimeListStatusOptions
{
    /// <summary>
    /// The status of the anime. Default is null.
    /// </summary>
    public string? Status { get; set; } = null;
    
    /// <summary>
    /// Is the user rewatching the anime? Default is null.
    public string? IsRewatching { get; set; } = null;
    
    /// <summary>
    /// The score the user has given the anime. Default is null.
    /// </summary>
    public int? Score { get; set; } = null;
    
    /// <summary>
    /// The number of episodes the user has watched. Default is null.
    /// </summary>
    public int? NumWatchedEpisodes { get; set; } = null;
    
    /// <summary>
    /// The start date of the anime in the user's list. Default is null.
    /// </summary>
    public DateTime? StartDate { get; set; } = null;
    
    /// <summary>
    /// The end date of the anime in the user's list. Default is null.
    /// </summary>
    public DateTime? EndDate { get; set; } = null;
    
    /// <summary>
    /// Priority of the anime in the user's list. Default is null.
    /// </summary>
    public int? Priority { get; set; } = null;
    
    /// <summary>
    /// The number of times the user has rewatched the anime. Default is null.
    /// </summary>
    public int? NumRewatched { get; set; } = null;
    
}