namespace Misy.Highscore.Service.Scores;

public static class ScoresApi
{
    public static void UseScoresApi(this WebApplication app)
    {
        var scoresApi = app.MapGroup("/scores");

        scoresApi.MapGet("/hello", () => "Hello world");

        scoresApi.MapGet("/{servicename?}", async ([FromServices] ArangoDBClient arangoClient, string? servicename) =>
        {
            var query = new StringBuilder("FOR doc IN highscores ");

            if (!string.IsNullOrEmpty(servicename))
            {
                query.AppendLine($"FILTER doc.appname == '{servicename}' ");
            }

            query.AppendLine("SORT doc.score DESC\nRETURN doc");

            Console.Write(query.ToString());
            return (await arangoClient.Cursor.PostCursorAsync<HighscoreDBO>(query.ToString())).Result;
        });

        scoresApi.MapPost("/insert", async ([FromBody] HighscoreDBO highscore, [FromServices] ArangoDBClient arangoClient) =>
        {
            await arangoClient.Document.PostDocumentAsync("highscores", highscore);
        });
    }
}
