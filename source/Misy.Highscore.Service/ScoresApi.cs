using System.Text;

namespace Misy.Highscore.Service;

public class ScoresApi
{
    public static void ConfigureApi(WebApplication app)
    {
        var scoresApi = app.MapGroup("/scores");

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
