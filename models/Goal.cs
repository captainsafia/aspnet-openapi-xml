namespace Models;

/// <summary>
/// Represents a goal item.
/// </summary>
public class Goal
{
    /// <summary>
    /// Unique identifier for the goal.
    /// </summary>
    public int Id { get; set; }

    /// <summary>
    /// The title or brief description of the goal.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// A detailed description of the goal.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Indicates whether the goal has been achieved.
    /// </summary>
    public bool IsAchieved { get; set; }

    /// <summary>
    /// The target date for achieving the goal.
    /// </summary>
    public DateTime? TargetDate { get; set; }

    /// <summary>
    /// The date when the goal was created.
    /// </summary>
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// The date when the goal was achieved (if applicable).
    /// </summary>
    public DateTime? AchievedAt { get; set; }

    /// <summary>
    /// Category or type of the goal.
    /// </summary>
    public string? Category { get; set; }

    /// <summary>
    /// Progress percentage (0-100).
    /// </summary>
    public int Progress { get; set; } = 0;
}
