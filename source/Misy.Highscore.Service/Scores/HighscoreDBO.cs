namespace Misy.Highscore.Service.Scores;

public record HighscoreDBO(DateTime? Created, string Appname, string User, long Score);