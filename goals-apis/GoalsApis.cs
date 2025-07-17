using Models;
using Microsoft.AspNetCore.Http.HttpResults;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Api;

/// <summary>
/// Extension methods for mapping Goal-related API endpoints.
/// </summary>
public static class GoalsApiExtensions
{
    // In-memory storage for demo purposes
    private static readonly List<Goal> Goals = new();
    private static int _nextGoalId = 1;

    /// <summary>
    /// Maps all Goal related API endpoints to the application.
    /// </summary>
    /// <param name="app">The endpoint route builder.</param>
    public static void MapGoalsApis(this IEndpointRouteBuilder app)
    {
        // Goal endpoints
        var goalGroup = app.MapGroup("/api/goals")
            .WithTags("Goals");

        goalGroup.MapGet("/", GetAllGoals)
            .WithName("GetAllGoals");

        goalGroup.MapGet("/{id}", GetGoalById)
            .WithName("GetGoalById");

        goalGroup.MapPost("/", CreateGoal)
            .WithName("CreateGoal");

        goalGroup.MapPut("/{id}", UpdateGoal)
            .WithName("UpdateGoal");

        goalGroup.MapDelete("/{id}", DeleteGoal)
            .WithName("DeleteGoal");
    }

    /// <summary>
    /// Retrieves all goals.
    /// </summary>
    /// <returns>A collection of all goals.</returns>
    /// <response code="200">Returns the list of goals.</response>
    public static Ok<IEnumerable<Goal>> GetAllGoals()
    {
        return TypedResults.Ok(Goals.AsEnumerable());
    }

    /// <summary>
    /// Retrieves a specific goal by ID.
    /// </summary>
    /// <param name="id">The ID of the goal to retrieve.</param>
    /// <returns>The requested goal.</returns>
    /// <response code="200">Returns the requested goal.</response>
    /// <response code="404">If the goal is not found.</response>
    public static Results<Ok<Goal>, NotFound> GetGoalById(int id)
    {
        var goal = Goals.FirstOrDefault(g => g.Id == id);
        if (goal == null)
        {
            return TypedResults.NotFound();
        }

        return TypedResults.Ok(goal);
    }

    /// <summary>
    /// Creates a new goal.
    /// </summary>
    /// <param name="goal">The goal to create.</param>
    /// <returns>The newly created goal.</returns>
    /// <response code="201">Returns the newly created goal.</response>
    /// <response code="400">If the goal data is invalid.</response>
    public static Results<Created<Goal>, BadRequest<string>> CreateGoal([FromBody] Goal goal)
    {
        if (string.IsNullOrWhiteSpace(goal.Title))
        {
            return TypedResults.BadRequest("Goal title is required.");
        }

        if (goal.Progress < 0 || goal.Progress > 100)
        {
            return TypedResults.BadRequest("Progress must be between 0 and 100.");
        }

        goal.Id = _nextGoalId++;
        goal.CreatedAt = DateTime.UtcNow;
        
        Goals.Add(goal);

        return TypedResults.Created($"/api/goals/{goal.Id}", goal);
    }

    /// <summary>
    /// Updates an existing goal.
    /// </summary>
    /// <param name="id">The ID of the goal to update.</param>
    /// <param name="updatedGoal">The updated goal data.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">If the update was successful.</response>
    /// <response code="400">If the goal data is invalid.</response>
    /// <response code="404">If the goal is not found.</response>
    public static Results<NoContent, BadRequest<string>, NotFound> UpdateGoal(int id, [FromBody] Goal updatedGoal)
    {
        var goal = Goals.FirstOrDefault(g => g.Id == id);
        if (goal == null)
        {
            return TypedResults.NotFound();
        }

        if (string.IsNullOrWhiteSpace(updatedGoal.Title))
        {
            return TypedResults.BadRequest("Goal title is required.");
        }

        if (updatedGoal.Progress < 0 || updatedGoal.Progress > 100)
        {
            return TypedResults.BadRequest("Progress must be between 0 and 100.");
        }

        goal.Title = updatedGoal.Title;
        goal.Description = updatedGoal.Description;
        goal.IsAchieved = updatedGoal.IsAchieved;
        goal.TargetDate = updatedGoal.TargetDate;
        goal.Category = updatedGoal.Category;
        goal.Progress = updatedGoal.Progress;

        // Set achieved date if goal is marked as achieved and wasn't before
        if (updatedGoal.IsAchieved && !goal.IsAchieved)
        {
            goal.AchievedAt = DateTime.UtcNow;
        }
        else if (!updatedGoal.IsAchieved)
        {
            goal.AchievedAt = null;
        }

        return TypedResults.NoContent();
    }

    /// <summary>
    /// Deletes a goal.
    /// </summary>
    /// <param name="id">The ID of the goal to delete.</param>
    /// <returns>No content if successful.</returns>
    /// <response code="204">If the deletion was successful.</response>
    /// <response code="404">If the goal is not found.</response>
    public static Results<NoContent, NotFound> DeleteGoal(int id)
    {
        var goal = Goals.FirstOrDefault(g => g.Id == id);
        if (goal == null)
        {
            return TypedResults.NotFound();
        }

        Goals.Remove(goal);
        return TypedResults.NoContent();
    }
}